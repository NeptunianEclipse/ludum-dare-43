using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Mid-Air Jump")]
public class MidAirJump : AbilityBase
{
	public override string Name => "Mid-Air Jump";

	public float JumpPower;
	public int NumJumps = 1;

	private int jumpCount;
	private PlayerMovement movement;

	protected override void Initialize()
	{
		base.Initialize();

		movement = controller.GameObject.GetComponent<PlayerMovement>();
		movement.Grounded += OnGrounded;
	}

	public override void OnActivate()
	{
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