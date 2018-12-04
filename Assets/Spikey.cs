using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikey : MonoBehaviour
{
	public float Damage = 5;
	public float DamageCooldown = 0.5f;
	public Vector2 KnockbackDirection = Vector2.up;
	public float KnockbackMagnitude = 5f;
	public float KnockbackCooldown = 0.5f;
	public bool KnockbackAllMassesTheSame = true;

	private readonly List<Rigidbody2D> spikeBodies = new List<Rigidbody2D>();
	private readonly List<Rigidbody2D> recentlySpiked = new List<Rigidbody2D>();

	// All spikey things share the same list of recently damage objects.
	private static readonly List<Damageable> recentlyDamaged = new List<Damageable>();

	private void FixedUpdate()
	{
		try
		{
			spikeBodies.ForEach((rigidbody) =>
			{
				var force = KnockbackDirection.normalized * KnockbackMagnitude * (KnockbackAllMassesTheSame ? rigidbody.mass : 5f);
				rigidbody.AddForce(force, ForceMode2D.Impulse);
				recentlySpiked.Add(rigidbody);
				StartCoroutine(Extensions.InvokeAfter(() => recentlySpiked.Remove(rigidbody), KnockbackCooldown));
			});
		}
		catch (MissingReferenceException mre)
		{
			Debug.LogError(mre);
			
			// lmao don't throw. fuck there's no time left.
		}
		spikeBodies.Clear();
	}

	private void OnTriggerStay2D(Collider2D collider)
	{
		{
			Rigidbody2D rigidbody = collider.gameObject.GetComponent<Rigidbody2D>();
			if (rigidbody != null && !recentlySpiked.Contains(rigidbody))
			{
				spikeBodies.AddIfMissing(rigidbody);
			}
		}
		{
			Damageable damageable = collider.gameObject.GetComponent<Damageable>();
			if (damageable != null && !recentlyDamaged.Contains(damageable))
			{
				damageable.InflictDamage(Damage);
				recentlyDamaged.Add(damageable);
				StartCoroutine(Extensions.InvokeAfter(() => recentlyDamaged.Remove(damageable), DamageCooldown));
			}
		}
	}
}
