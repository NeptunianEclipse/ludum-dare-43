using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Jump")]
public class Jump : AbilityBase
{
	public float JumpPower;

	private PlayerMovement movement;

	public override void OnActivate()
	{
		if(movement.IsGrounded())
		{
			movement.Jump();
		}
	}

	protected override void Initialize()
	{
		base.Initialize();

		controller.Activate += OnActivate;

		movement = controller.GameObject.GetComponent<PlayerMovement>();
	}


}
