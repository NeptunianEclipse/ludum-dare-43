using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornyFeet : AbilityBase
{
	public float DamageAmount = 1;
	public bool RequireFalling = true;

	public override string Name => "Thorny Feet";

	private PlayerMovement playerMovement;

	protected override void OnEquip()
	{
		base.OnEquip();

		playerMovement = Controller.GameObject.GetComponent<PlayerMovement>();
		playerMovement.FeetTriggerEnter += OnFeetTriggerEnter;
	}

	protected override void OnUnequip()
	{
		base.OnUnequip();

		playerMovement.FeetTriggerEnter -= OnFeetTriggerEnter;
	}

	private void OnFeetTriggerEnter(Collider2D collider, Vector2 point)
	{
		if(playerMovement.Rigidbody2d.velocity.y > 0)
		{
			return;
		}

		var damageable = collider.gameObject.GetComponent<Damageable>();
		if(damageable != null)
		{
			damageable.InflictDamage(DamageAmount, point);
		}
	}

}
