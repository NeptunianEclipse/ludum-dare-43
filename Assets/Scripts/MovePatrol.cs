using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovePatrol : MonoBehaviour
{
	private const int bigNumber = 200;

	public float MaxSpeed = 1f;
	public float TurnDuration = 1f;

	public float DownDistance = 1f;
	public float RightDistance = 0.1f;

	public float Acceleration = 10f;

	// If changing this make sure you change the code which inverses the velocity if we've turned around.
	private Vector2 MoveDirection = new Vector2(1, 0);

	public LayerMask Notices;

	private bool turning = false;

	private Collider2D myCollider;
	private Rigidbody2D myRigidbody;

	private bool shouldCheckToTurn = true;

	void Awake()
	{
		myCollider = GetComponent<Collider2D>();
		myRigidbody = GetComponent<Rigidbody2D>();

		if (Notices == default(LayerMask)) Debug.LogWarning($"A {gameObject.name} has no layer mask set.");

	}

	void FixedUpdate()
	{
		if (!turning)
		{
			//transform.Translate(MoveDirection.normalized * MoveSpeed * Time.deltaTime);
			Vector2 maxVelocity = MoveDirection.normalized * MaxSpeed;

			// If we've turned around, inverse the velocity, so multiply by sign.
			maxVelocity *= Mathf.Sign(transform.right.x);

			Vector2 currentVelocity = myRigidbody.velocity;

			bool belowMaxSpeed = maxVelocity.x >= 0 ? currentVelocity.x < maxVelocity.x : currentVelocity.x > maxVelocity.x;
			if (belowMaxSpeed)
			{
				var force = maxVelocity * Acceleration * myRigidbody.mass;
				myRigidbody.AddForce(force);
			}

			bool noticeablyAboveMaxSpeed = maxVelocity.x >= 0 ? currentVelocity.x > maxVelocity.x + 0.2f : currentVelocity.x > maxVelocity.x + 0.2f;

			shouldCheckToTurn = noticeablyAboveMaxSpeed == false;
		}
	}

	void Update()
	{
		if (!turning && shouldCheckToTurn)
		{
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
			else
			{
				Vector2 boxCentre = rightEdge.NewWithChange(deltaX: RightDistance / 2);
				Vector2 boxSize = new Vector2(RightDistance / 2, topEdge.y - bottomEdge.y - 0.2f);

				Collider2D somethingInFrontOfMe = Physics2D.OverlapBox(boxCentre, boxSize, 0, Notices.value);
				if (somethingInFrontOfMe == true)
				{
					StartTurning();
				}
			}
		}
	}

	private void StartTurning()
	{
		myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
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
