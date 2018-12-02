using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour 
{
	public int DamageAmount;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		var damageable = collision.gameObject.GetComponent<Damageable>();
		if (damageable != null)
		{
			damageable.InflictDamage(DamageAmount, collision.GetContact(0).point);
		}
	}
}
