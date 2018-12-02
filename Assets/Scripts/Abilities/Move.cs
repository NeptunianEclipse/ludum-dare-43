using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : AbilityBase
{
	public override string Name => "Move";

	public MoveSettings MoveSettings;
	public int Direction;

	private PlayerMovement playerMovement;
	private Rigidbody2D playerRigidbody;

	protected override void OnEquip()
	{
		base.OnEquip();

		playerMovement = Controller.GameObject.GetComponent<PlayerMovement>();
		playerRigidbody = playerMovement.Rigidbody2d;
	}

	protected override void FixedTick()
	{
		if(Activated)
		{
			playerRigidbody.velocity += Vector2.right * Direction * MoveSettings.Acceleration;
		}
	}

	


}
