using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Throwable : MonoBehaviour
{
	public LayerMask ImpactsWith;

	public float MaxTimeAlive = 10f;

	//private Collider2D myCollider;

	private float timeAlive = 0f;

	/// <summary>
	/// Raised when this reaches it's max life time.
	/// </summary>
	public event System.EventHandler Expired;

	/// <summary>
	/// Raised when this collides with a valid object.
	/// </summary>
	public event System.EventHandler Impacted;

	void Awake()
	{
		//myCollider = GetComponent<Collider2D>();

		if (ImpactsWith == default(LayerMask)) Debug.LogWarning($"A {gameObject.name} has no layer mask set in the {nameof(Throwable)} component.");

	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.IsTouchingLayers(ImpactsWith.value)) OnImpacted(new System.EventArgs());
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

	private void OnImpacted(System.EventArgs eventArgs)
	{
		Impacted?.Invoke(this, eventArgs);
	}

	private void OnExpired(System.EventArgs e)
	{
		Expired?.Invoke(this, e);
	}

}
