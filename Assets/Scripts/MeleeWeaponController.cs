using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackState
{
	Inactive,
	Attacking,
	Returning
}

public class MeleeWeaponController : MonoBehaviour
{
	public float attackDamage = 0f;
	public float timeBetweenAttacks = 0f;
	public float attackDuration = 1f;
	public float returnDuration = 1f;
	public Vector2 attackLength;
	public float attackImpact = 1f;

	//public LayerMask targetLayer;
	public CameraShake cameraShake;

	public bool logEverything = false;

	public float RecoveryPercent {
		get
		{
			if(AttackState == AttackState.Returning)
			{
				return Mathf.Clamp01(timeUntilReturnEnds / Mathf.Max(timeBetweenAttacks, 0.01f));
			}
			else if(AttackState == AttackState.Attacking)
			{
				return 0;
			}
			else
			{
				return 1;
			}
		}
	}

	private Vector3 initialPosition;

	private float timeUntilNextAttack;
	private float timeUntilAttackEnds;
	private float timeUntilReturnEnds;

	private AttackState AttackState = AttackState.Inactive;
	private bool willAttack = false;

	private readonly ICollection<GameObject> damagedObjects = new List<GameObject>();

	private void Awake()
	{
		timeUntilNextAttack = timeBetweenAttacks;
		timeUntilAttackEnds = 0f;
		timeUntilReturnEnds = 0f;
		initialPosition = transform.localPosition;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Log($"This ({gameObject.name}) entred a {collision.gameObject.name}");
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (AttackState == AttackState.Attacking)
		{
			GameObject other = collision.gameObject;

			var damageable = other.GetComponent<Damageable>();

			if(damageable != null && !damagedObjects.Contains(other))
			{
				cameraShake?.StartShaking(attackImpact);

				damageable.InflictDamage(attackDamage);
				damagedObjects.Add(other);
			}
		}
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;

		if(AttackState == AttackState.Attacking)
		{
			if(timeUntilAttackEnds <= 0)
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
		else if (AttackState == AttackState.Returning)
		{
			if (timeUntilReturnEnds <= 0)
			{
				EndReturn();
			}
			else
			{
				float perentageOfReturnSinceLastFrame = deltaTime / returnDuration;
				Vector2 returnLength = -attackLength; // Return is opposite direction to the attack.

				float deltaX = returnLength.x * perentageOfReturnSinceLastFrame;
				float deltaY = returnLength.y * perentageOfReturnSinceLastFrame;

				transform.localPosition = transform.localPosition.NewWithChange(deltaX, deltaY);

				timeUntilReturnEnds -= deltaTime;
			}
		}
		else
		{
			if(timeUntilNextAttack <= 0)
			{
				if(willAttack)
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

	public void TryAttack()
	{
		Debug.Log("try attack");
		willAttack = AttackState == AttackState.Inactive;
		Debug.Log(willAttack);
	}

	private void StartAttack()
	{	
		timeUntilAttackEnds = attackDuration;
		AttackState = AttackState.Attacking;
	}

	private void EndAttack()
	{
		Log($"This ({this.gameObject.name}) has damaged {(damagedObjects.Count == 1 ? "an object" : $"{damagedObjects.Count} objects")}.");
		damagedObjects.Clear();
		AttackState = AttackState.Returning;
		timeUntilReturnEnds = returnDuration;
	}

	private void EndReturn()
	{
		transform.localPosition = initialPosition; // To deal with slight differences in the exact amount moved per frame, ie. if the duration of frames didn't exactly sum to the attack's duration.
		AttackState = AttackState.Inactive;
		timeUntilNextAttack = timeBetweenAttacks;
		willAttack = false;
	}

	private void Log(string message)
	{
		if (logEverything)
		{
			Debug.Log(message);
		}
	}
}
