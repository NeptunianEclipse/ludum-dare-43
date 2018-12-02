using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Thrower : MonoBehaviour
{
	public GameObject ProjectilePrefab;

	private ICollection<GameObject> projectileInstances = new List<GameObject>();

	private Collider2D myCollider;
	private Rigidbody2D myRigidbody;

	void Awake()
	{
		myCollider = GetComponent<Collider2D>();
		myRigidbody = GetComponent<Rigidbody2D>();
	}

	public void ThrowProjectile(Vector2 force)
	{
		GameObject newProjectile = Instantiate(original: ProjectilePrefab, position: transform.position, rotation: Quaternion.identity);
		Throwable projectilesThrowable = newProjectile.GetComponent<Throwable>();
		if (projectilesThrowable == null)
		{
			Debug.LogError($"A {gameObject.name} tried to throw a {newProjectile.name} but it did not have a {nameof(Throwable)} compnent.");
			Destroy(newProjectile);
			return;
		}
		// Throwable requires a Rigidbody2D
		Rigidbody2D projectilesRigidbody = newProjectile.GetComponent<Rigidbody2D>();

		float forceDuration = 0.1f;
		Vector2 forcePerUpdate = force / forceDuration;
		StartCoroutine(projectilesRigidbody.ApplyForce(forcePerUpdate, forceDuration));
		StartCoroutine(myRigidbody.ApplyForce(-forcePerUpdate, forceDuration));

		if (myCollider != null)
		{
			Collider2D projectilesCollider = newProjectile.GetComponent<Collider2D>();
			Physics2D.IgnoreCollision(myCollider, projectilesCollider);
		}
		
		projectilesThrowable.Expired += ThrowableComponent_Expired;
		projectilesThrowable.parent = this;
		
		projectileInstances.Add(newProjectile);
	}

	private void ThrowableComponent_Expired(object sender, System.EventArgs e)
	{
		var throwable = sender as Throwable;
		if (throwable != null)
		{
			throwable.Expired -= ThrowableComponent_Expired;
			projectileInstances.Remove(throwable.gameObject);
		}
		else
		{
			Debug.LogError($"{nameof(Throwable)} {nameof(Throwable.Expired)} event called from something that's not a {nameof(Throwable)}.");
		}
	}

	public IEnumerable<GameObject> CurrentProjectiles => projectileInstances;

}
