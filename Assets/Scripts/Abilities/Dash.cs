using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Dash")]
public class Dash : AbilityBase
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

	protected override void Initialize()
	{
		base.Initialize();

		controller.Tick += OnUpdate;

		playerMovement = controller.GameObject.GetComponent<PlayerMovement>();
		playerRigidbody = controller.GameObject.GetComponent<Rigidbody2D>();

		lastDashEndTime = Time.time - Duration;
	}

	public override void OnActivate()
	{
		if(Time.time >= lastDashEndTime + TimeBetweenDashes && dashing == false)
		{ 
			StartDash();
		}
	}

	private void OnUpdate()
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
