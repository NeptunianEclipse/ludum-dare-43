using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityController
{
	GameObject GameObject { get; }
	event Action Activate;
}

public abstract class AbilityBase
{
	public abstract KeyCode ActivateKey { get; }

	protected IAbilityController controller;

	public IAbilityController Controller {
		get
		{
			return controller;
		}
		set
		{
			controller = value;
			Initialize();
		}
	}

	public abstract void OnActivate();

	protected virtual void Initialize()
	{
	}

}

public class MidAirJump : AbilityBase
{
	public override KeyCode ActivateKey => KeyCode.F;

	public int NumJumps = 1;

	private int jumpCount;
	private PlayerMovement movement;

	protected override void Initialize()
	{
		base.Initialize();

		controller.Activate += OnActivate;

		movement = controller.GameObject.GetComponent<PlayerMovement>();
		movement.Grounded += OnGrounded;
	}

	public override void OnActivate()
	{
		if(movement.IsGrounded() == false && jumpCount < NumJumps)
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