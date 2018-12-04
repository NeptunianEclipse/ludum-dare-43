using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPound : AbilityBase, IRecoverable
{
	public float Duration;
	public float Speed;
	public float TimeBetweenPounds;
	public float Damage;

	public override bool IsPassive => false;

	private PlayerMovement playerMovement;
	private Rigidbody2D playerRigidbody;
	private bool pounding;

	private float poundStartTime;
	private float lastPoundEndTime;

	public float RecoveryPercent => pounding ? 0 : Mathf.Clamp01(Mathf.InverseLerp(lastPoundEndTime, lastPoundEndTime + TimeBetweenPounds, Time.time));

	protected override void OnEquip()
	{
		base.OnEquip();

		playerMovement = Controller.GameObject.GetComponent<PlayerMovement>();
		playerRigidbody = Controller.GameObject.GetComponent<Rigidbody2D>();

		playerMovement.Grounded += OnGrounded;
		playerMovement.CollisionEnter += OnPlayerCollisionEnter;

		lastPoundEndTime = Time.time - Duration;
	}

	protected override void OnUnequip()
	{
		base.OnUnequip();

		playerMovement.Grounded -= OnGrounded;
		playerMovement.CollisionEnter -= OnPlayerCollisionEnter;
	}

	public override void Activate()
	{
		base.Activate();

		if (Time.time >= lastPoundEndTime + TimeBetweenPounds && pounding == false && playerMovement.IsGrounded() == false)
		{
			StartPound();
		}
	}

	protected override void Tick()
	{
		if (pounding == false)
		{
			return;
		}

		if (Time.time >= poundStartTime + Duration)
		{
			EndPound();
		}

		playerMovement.transform.position += Vector3.down * Speed;
	}

	private void OnGrounded()
	{
		EndPound();
	}

	private void OnPlayerCollisionEnter(Collision2D collision)
	{
		var damageable = collision.gameObject.GetComponent<Damageable>();
		if(damageable != null)
		{
			damageable.InflictDamage(Damage);
		}
	}

	private void StartPound()
	{
		playerRigidbody.velocity = Vector2.down;
		poundStartTime = Time.time;
		pounding = true;
	}

	private void EndPound()
	{
		lastPoundEndTime = Time.time;
		pounding = false;
	}
}
