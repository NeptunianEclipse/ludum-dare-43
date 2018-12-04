using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
	public float Damage;

	private Throwable throwable;

	private void Awake()
	{
		throwable = GetComponent<Throwable>();
		throwable.Impacted += Impacted;
	}

	private void Impacted(Collision2D collision)
	{
		var damageable = collision.gameObject.GetComponent<Damageable>();
		if(damageable != null)
		{
			damageable.InflictDamage(Damage);
		}
		Destroy(gameObject);
	}

}
