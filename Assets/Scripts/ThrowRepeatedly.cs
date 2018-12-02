using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Thrower))]
public class ThrowRepeatedly : MonoBehaviour
{
	public float TimeBetweenThrows = 1f;
	public bool FlipThrowDirectionWhenFacingOtherDirection = true;

	public Vector2 DirectionToThrowIn;
	public float ForceToThrowWith = 100f;

	private float timeUntilNextThrow;
	private Thrower throwerComponent;

	void Awake()
	{
		timeUntilNextThrow = TimeBetweenThrows;

		throwerComponent = gameObject.GetComponent<Thrower>();
	}

	void Update()
	{
		if (timeUntilNextThrow <= 0)
		{
			Vector2 direction = DirectionToThrowIn.normalized;

			if (FlipThrowDirectionWhenFacingOtherDirection)
			{
				direction.x *= Mathf.Sign(gameObject.transform.right.x);
			}

			Vector2 force = direction * ForceToThrowWith;

			throwerComponent.ThrowProjectile(force);

			timeUntilNextThrow = TimeBetweenThrows;
		}
		else
		{
			timeUntilNextThrow -= Time.deltaTime;
		}
	}
}
