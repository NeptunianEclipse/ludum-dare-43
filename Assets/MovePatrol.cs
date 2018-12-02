using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatrol : MonoBehaviour
{
	private const int bigNumber = 200;

	public float moveSpeed;
	public float turnDuration = 1f;

	public float downDistance = 1f;
	public float rightDistance = 1f;

	public Vector2 moveDirection;

	private bool turning = false;

	private Collider2D myCollider;

	void Awake()
	{
		myCollider = GetComponent<Collider2D>();
	}

	void Update()
	{
		if (!turning)
		{
			transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime);

			Bounds bounds = myCollider.bounds;

			Vector3 rightEdge = bounds.ClosestPoint((transform.right * bigNumber) + transform.position);
			Vector3 topEdge = bounds.ClosestPoint((transform.up * bigNumber) + transform.position);
			Vector3 bottomEdge = bounds.ClosestPoint((-transform.up * bigNumber) + transform.position);

			Vector2 BottomRightCorner = new Vector2(rightEdge.x, bottomEdge.y);

			RaycastHit2D groundInFrontOfMe = Physics2D.Raycast(BottomRightCorner, Vector2.down, downDistance);
			if (groundInFrontOfMe.collider == false)
			{
				StartTurning();
			}

			Vector2 boxCentre = rightEdge.NewWithChange(deltaX: rightDistance / 2);
			Vector2 boxSize = new Vector2(rightDistance / 2, topEdge.y - bottomEdge.y - 0.2f);

			Collider2D somethingInFrontOfMe = Physics2D.OverlapBox(boxCentre, boxSize, 0, LayerMask.GetMask("Ground"));
			if (somethingInFrontOfMe == true)
			{
				StartTurning();
			}
		}
	}
	
	private void StartTurning()
	{
		turning = true;
		StartCoroutine(Turn(180, turnDuration));
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
