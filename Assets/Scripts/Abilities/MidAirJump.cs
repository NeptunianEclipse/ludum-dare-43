using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidAirJump : AbilityBase
{
	public float JumpPower;
	public int NumJumps = 1;

	private int jumpCount;
	private PlayerMovement movement;

	protected override void OnEquip()
	{
		base.OnEquip();

		movement = Controller.GameObject.GetComponent<PlayerMovement>();
		movement.Grounded += OnGrounded;
	}

	protected override void OnUnequip()
	{
		movement.Grounded -= OnGrounded;
		base.OnUnequip();
	}

	public override void Activate()
	{
		base.Activate();

		if (movement.IsGrounded() == false && jumpCount < NumJumps)
		{
			movement.Jump();
			jumpCount++;
		}
	}

	private void OnGrounded()
	{
		jumpCount = 0;
	}

}