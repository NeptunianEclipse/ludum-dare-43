using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Dash")]
public class Dash : AbilityBase
{
	public float Duration;
	public float Speed;

	private PlayerMovement playerMovement;
	private bool dashing;
	private float dashStartTime;

	protected override void Initialize()
	{
		base.Initialize();

		controller.Tick += OnUpdate;

		playerMovement = controller.GameObject.GetComponent<PlayerMovement>();
	}

	public override void OnActivate()
	{
		playerMovement.HorizontalMovementAllowed = false;
		dashStartTime = Time.time;
		dashing = true;
	}

	private void OnUpdate()
	{
		if(dashing == false)
		{
			return;
		}

		if(Time.time > dashStartTime + Duration)
		{
			dashing = false;
			playerMovement.HorizontalMovementAllowed = true;
		}

		playerMovement.transform.position += Vector3.right * Speed;
	}

	

}
