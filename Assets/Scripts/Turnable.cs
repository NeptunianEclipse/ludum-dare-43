using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Turnable : MonoBehaviour, ITurnable
{
	public float TurnDuration = 1f;

	public bool Debug_LogTurning = false;

	private Rigidbody2D myRigidbody;

	public bool IsTurning { get; private set; }

	void Awake()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
	}

	public void StartTurning()
	{
		StartTurning(180);
	}

	public void StartTurning(float degrees)
	{
		myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
		IsTurning = true;
		StartCoroutine(Turn(degrees, TurnDuration));
		if (Debug_LogTurning) Debug.Log($"A {gameObject.name} started turning.");
	}

	private IEnumerator Turn(float YRotation, float duration)
	{
		Vector3 startingLocalEulerAngles = transform.localEulerAngles;

		float totalRotation = 0f;

		while (totalRotation < YRotation)
		{
			float percentageOfRotationSinceLastFrame = Time.deltaTime / duration;
			float rotation = YRotation * percentageOfRotationSinceLastFrame;

			transform.localEulerAngles = transform.localEulerAngles.NewWithChange(deltaY: rotation);

			totalRotation += rotation;

			yield return null;
		}

		// Tidy up the turn to be exactly correct, so that we don't end up with minor rounding erros if the total rotation grows to be more than the intended YRotation.
		transform.localEulerAngles = startingLocalEulerAngles.NewWithChange(deltaY: YRotation);

		IsTurning = false;
	}
}
