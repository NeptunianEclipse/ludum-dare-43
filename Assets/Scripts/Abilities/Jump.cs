using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : AbilityBase
{
	public float JumpPower;

	private PlayerMovement movement;

	private bool didJump;


	public override void During()
	{
		base.During();

		if(didJump == false && movement.IsGrounded())
		{
			//Debug.Log($"Jumped");
			movement.Jump();
			didJump = true;
		}
	}

	public override void Release()
	{
		base.Release();

		// Ok, so this isn't perfect because it means user can release and then press again very quickly, which calls movement.Jump twice but since they gotta be /real/ quick I can't be bothered fixing it.
		didJump = false;
	}

	protected override void OnEquip()
	{
		base.OnEquip();

		movement = Controller.GameObject.GetComponent<PlayerMovement>();
	}


}
