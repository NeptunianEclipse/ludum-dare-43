using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Thorny Feet")]
public class ThornyFeet : AbilityBase
{
	public float DamageAmount = 1;
	public bool RequireFalling = true;

	public override string Name => "Thorny Feet";

	private PlayerMovement playerMovement;

	protected override void Initialize()
	{
		base.Initialize();

		playerMovement = controller.GameObject.GetComponent<PlayerMovement>();
		playerMovement.FeetTriggerEnter += OnFeetTriggerEnter;
	}

	private void OnFeetTriggerEnter(Collider2D collision)
	{
		if(playerMovement.Rigidbody2d.velocity.y > 0)
		{
			return;
		}

		var damageable = collision.gameObject.GetComponent<Damageable>();
		if(damageable != null)
		{
			damageable.InflictDamage(DamageAmount);
		}
	}

}
