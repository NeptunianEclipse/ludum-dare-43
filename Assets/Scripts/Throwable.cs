using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Throwable : MonoBehaviour
{
	public LayerMask CollidesWith;

	public float MaxTimeAlive = 10f;

	private Collider2D myCollider;

	private float timeAlive = 0f;

	/// <summary>
	/// Raised when this reaches it's max life time.
	/// </summary>
	public event System.EventHandler Expired;

	/// <summary>
	/// Raised when this collides with a valid object.
	/// </summary>
	public event System.EventHandler Collided;

	internal Thrower parent;

	void Awake()
	{
		myCollider = GetComponent<Collider2D>();

		if (CollidesWith == default(LayerMask)) Debug.LogWarning($"A {gameObject.name} has no layer mask set.");

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		//// Don't collide with our parent
		//if (parent != null && collision.gameObject == parent.gameObject)
		//{
		//	Physics2D.IgnoreCollision(myCollider, collision.collider);
		//}
		//else
		//{
		//	OnCollided(new System.EventArgs());
		//}
	}

	void Update()
	{
		timeAlive += Time.deltaTime;

		if (timeAlive > MaxTimeAlive) Expire();
	}

	public void Expire()
	{
		OnExpired(new System.EventArgs());

		Destroy(gameObject);
	}

	private void OnCollided(System.EventArgs eventArgs)
	{
		Collided?.Invoke(this, eventArgs);
	}

	private void OnExpired(System.EventArgs e)
	{
		Expired?.Invoke(this, e);
	}

}
