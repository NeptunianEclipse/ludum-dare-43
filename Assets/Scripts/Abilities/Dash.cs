using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : AbilityBase, IRecoverable
{
	public override string Name => "Dash";

	public float Duration;
	public float Speed;

	public float TimeBetweenDashes;

	private PlayerMovement playerMovement;
	private Rigidbody2D playerRigidbody;
	private bool dashing;

	private float dashStartTime;
	private float lastDashEndTime;

	public float RecoveryPercent => dashing ? 0 : Mathf.Clamp01(Mathf.InverseLerp(lastDashEndTime, lastDashEndTime + TimeBetweenDashes, Time.time));

	protected override void OnEquip()
	{
		base.OnEquip();

		playerMovement = Controller.GameObject.GetComponent<PlayerMovement>();
		playerRigidbody = Controller.GameObject.GetComponent<Rigidbody2D>();

		lastDashEndTime = Time.time - Duration;
	}

	public override void Activate()
	{
		base.Activate();

		if(Time.time >= lastDashEndTime + TimeBetweenDashes && dashing == false)
		{ 
			StartDash();
		}
	}

	protected override void Tick()
	{
		if(dashing == false)
		{
			return;
		}

		if(Time.time >= dashStartTime + Duration)
		{
			EndDash();
		}

		playerMovement.transform.position += Vector3.right * playerMovement.MovingDirection() * Speed;
	}

	private void StartDash()
	{
		playerMovement.HorizontalMovementAllowed = false;
		playerRigidbody.velocity = Vector2.right * playerMovement.MovingDirection();
		dashStartTime = Time.time;
		dashing = true;
	}

	private void EndDash()
	{
		playerMovement.HorizontalMovementAllowed = true;
		lastDashEndTime = Time.time;
		dashing = false;
	}

	

}
