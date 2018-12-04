using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : AbilityBase
{
	public MoveSettings MoveSettings;
	public int Direction;

	private PlayerMovement playerMovement;
	private Rigidbody2D playerRigidbody;
	private ITurnable maybeTurnable = null;


	protected override void OnEquip()
	{
		base.OnEquip();

		playerMovement = Controller.GameObject.GetComponent<PlayerMovement>();
		maybeTurnable = Controller.GameObject.GetComponent<ITurnable>();

		playerRigidbody = playerMovement.Rigidbody2d;

		if (maybeTurnable == null)
		{
			Debug.LogWarning($"No {nameof(ITurnable)} component on a {Controller.GameObject.name}");
		}
	}

	protected override void FixedTick()
	{
		if(Activated)
		{
			var deltaVelocity = Vector2.right * Direction * MoveSettings.Acceleration;
			var newVelocity = playerRigidbody.velocity + deltaVelocity;

			if (maybeTurnable != null && maybeTurnable.IsTurning == false)
			{
				float movingDirection = Mathf.Sign(newVelocity.x);
				float facingDirection = Mathf.Sign(transform.right.x);
				if (movingDirection != facingDirection)
				{
					maybeTurnable.StartTurning();
				}
			}

			playerRigidbody.velocity = newVelocity;
		}
	}

	


}
