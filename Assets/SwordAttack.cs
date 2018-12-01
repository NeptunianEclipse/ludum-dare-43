using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
	public float damage = 0f;
	public float timeBetweenAttacks = 0f;
	public float attackDuration = 1f;
	public float recoveryDuration = 1f;
	public Vector2 attackLength;

	public float attackImpact = 1f;

	public LayerMask targetLayer;
	public CameraShake cameraShake;
	//public Animator playerAnimator;

	private Vector3 initialPosition;

	private float timeUntilNextAttack;
	private float timeUntilAttackEnds;
	private float timeUntilRecoveryEnds;

	private bool isAttacking = false;
	private bool isRecovering = false;

	void Start()
	{
		timeUntilNextAttack = timeBetweenAttacks;
		timeUntilAttackEnds = 0f;
		timeUntilRecoveryEnds = 0f;
		initialPosition = transform.localPosition;
	}

	void Update()
	{
		float deltaTime = Time.deltaTime;

		if (isAttacking)
		{
			if (timeUntilAttackEnds <= 0)
			{
				EndAttack();
			}
			else
			{
				float perentageOfAttackSinceLastFrame = deltaTime / attackDuration;
				float deltaX = attackLength.x * perentageOfAttackSinceLastFrame;
				float deltaY = attackLength.y * perentageOfAttackSinceLastFrame;

				transform.localPosition = transform.localPosition.NewWithChange(deltaX, deltaY);

				timeUntilAttackEnds -= deltaTime;
			}
		}
		else if (isRecovering)
		{
			if (timeUntilRecoveryEnds <= 0)
			{
				EndRecovery();
			}
			else
			{
				float perentageOfRecoverySinceLastFrame = deltaTime / recoveryDuration;
				Vector2 recoveryLength = -attackLength; //Recovery is oposite direction to the attack.

				float deltaX = recoveryLength.x * perentageOfRecoverySinceLastFrame;
				float deltaY = recoveryLength.y * perentageOfRecoverySinceLastFrame;

				transform.localPosition = transform.localPosition.NewWithChange(deltaX, deltaY);

				timeUntilRecoveryEnds -= deltaTime;
			}
		}
		else
		{
			if (timeUntilNextAttack <= 0)
			{
				bool PlayerAttacking = Input.GetKey(KeyCode.V);
				if (PlayerAttacking)
				{
					StartAttack();
				}
			}
			else
			{
				timeUntilNextAttack -= deltaTime;
			}
		}
	}

	private void StartAttack()
	{	
		timeUntilAttackEnds = attackDuration;
		isAttacking = true;

	}

	private void EndAttack()
	{
		isAttacking = false;
		isRecovering = true;
		timeUntilRecoveryEnds = recoveryDuration;
	}

	private void EndRecovery()
	{
		transform.localPosition = initialPosition; // To deal with slight differences in the exact amount moved per frame, ie. if the duration of frames didn't exactly sum to the attack's duration.
		isRecovering = false;
		timeUntilNextAttack = timeBetweenAttacks;
	}

	// Currently not yet called from anywhere, need to set up collisions first.
	private void WhenHittingSomething()
	{
		float duration = attackImpact / 2;
		float magnitude = attackImpact;

		StartCoroutine(cameraShake.Shake(duration, magnitude));
	}
}
