using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	protected override void OnEquip()
	{
		base.OnEquip();

		movement = Controller.GameObject.GetComponent<PlayerMovement>();
	}


}
