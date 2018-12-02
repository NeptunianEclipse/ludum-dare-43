using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatrol : MonoBehaviour
{
	private const int bigNumber = 200;

	public float MoveSpeed = 0f;
	public float TurnDuration = 1f;

	public float DownDistance = 1f;
	public float RightDistance = 0.1f;

	public Vector2 MoveDirection;

	public LayerMask Notices;

	private bool turning = false;

	private Collider2D myCollider;

	void Awake()
	{
		myCollider = GetComponent<Collider2D>();

		if (Notices == default(LayerMask)) Debug.LogWarning($"A {gameObject.name} has no layer mask set.");
		
	}

	void Update()
	{
		if (!turning)
		{
			transform.Translate(MoveDirection.normalized * MoveSpeed * Time.deltaTime);

			Bounds bounds = myCollider.bounds;

			Vector3 rightEdge = bounds.ClosestPoint((transform.right * bigNumber) + transform.position);
			Vector3 topEdge = bounds.ClosestPoint((transform.up * bigNumber) + transform.position);
			Vector3 bottomEdge = bounds.ClosestPoint((-transform.up * bigNumber) + transform.position);

			Vector2 BottomRightCorner = new Vector2(rightEdge.x, bottomEdge.y);

			RaycastHit2D groundInFrontOfMe = Physics2D.Raycast(BottomRightCorner, Vector2.down, DownDistance, Notices.value);
			if (groundInFrontOfMe.collider == false)
			{
				StartTurning();
			}

			Vector2 boxCentre = rightEdge.NewWithChange(deltaX: RightDistance / 2);
			Vector2 boxSize = new Vector2(RightDistance / 2, topEdge.y - bottomEdge.y - 0.2f);

			Collider2D somethingInFrontOfMe = Physics2D.OverlapBox(boxCentre, boxSize, 0, Notices.value);
			if (somethingInFrontOfMe == true)
			{
				StartTurning();
			}
		}
	}
	
	private void StartTurning()
	{
		turning = true;
		StartCoroutine(Turn(180, TurnDuration));
		Debug.Log($"{gameObject.name} started turning.");
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

		turning = false;
	}
}
