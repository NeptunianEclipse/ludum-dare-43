using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Jump")]
public class Jump : AbilityBase
{
	public override string Name => "Jump";

	public float JumpPower;

	private PlayerMovement movement;

	public override void Activate()
	{
		if(movement.IsGrounded())
		{
			movement.Jump();
		}
	}

	protected override void Initialize()
	{
		base.Initialize();

		movement = controller.GameObject.GetComponent<PlayerMovement>();
	}


}
