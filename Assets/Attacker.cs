using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
	public float Damage = 5;
	public float DamageCooldown = 0.5f;
	public Vector2 KnockbackDirection = new Vector2(1f, 0.2f);
	public float KnockbackMagnitude = 3f;
	public float KnockbackCooldown = 0.5f;
	public bool KnockbackAllMassesTheSame = true;

	public LayerMask Damages;

	private readonly List<Rigidbody2D> spikeBodies = new List<Rigidbody2D>();
	private readonly List<Rigidbody2D> recentlySpiked = new List<Rigidbody2D>();

	// All spikey things share the same list of recently damage objects.
	private readonly List<Damageable> recentlyDamaged = new List<Damageable>();

	//private void FixedUpdate()
	//{
	//	spikeBodies.ForEach((rigidbody) =>
	//	{
	//		var force = KnockbackDirection.normalized * KnockbackMagnitude * (KnockbackAllMassesTheSame ? rigidbody.mass : 5f);
	//		rigidbody.AddForce(force, ForceMode2D.Impulse);
	//		recentlySpiked.Add(rigidbody);
	//		StartCoroutine(Extensions.InvokeAfter(() => recentlySpiked.Remove(rigidbody), KnockbackCooldown));
	//	});

	//	spikeBodies.Clear();
	//}

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
			if (damageable != null && !recentlyDamaged.Contains(damageable) && damageable.gameObject.layer == Damages.value)
			{
				damageable.InflictDamage(Damage);
				recentlyDamaged.Add(damageable);
				StartCoroutine(Extensions.InvokeAfter(() => recentlyDamaged.Remove(damageable), DamageCooldown));
			}
		}
	}
}
