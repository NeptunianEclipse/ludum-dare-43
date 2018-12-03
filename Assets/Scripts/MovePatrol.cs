using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovePatrol : MonoBehaviour
{
	private const int bigNumber = 200;

	public bool PatrolActive = true;

	public float MaxSpeed = 1f;
	public float TurnDuration = 1f;

	public float DownDistance = 1f;
	public float RightDistance = 0.1f;

	public float Acceleration = 10f;
	public float Decelleration = 10f;

	public bool Debug_LogTurning = false;
	public bool Debug_DrawDetectionBox = false;

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

	public bool Falsey { get; set; }

	void FixedUpdate()
	{
		if (Falsey)
		{
			float mag = Falsey ? 12f : 10f;
			var projs = this.GetComponent<Thrower>().CurrentProjectiles;

			var force = new Vector2(-1, 0);
			force *= mag;
			force *= mag;
			foreach (var proj in projs)
			{
				if (Falsey)
				{
					StartCoroutine(proj.GetComponent<Rigidbody2D>().ApplyForce(force, 0.05f));
				}
			}
		}
		if (PatrolActive) Move();
		else SlowDown(Decelleration);
	}

	void Update()
	{
		if (PatrolActive)
		if (shouldCheckToTurn) CheckToTurn();
	}

	public void Move()
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
				Accelerate(maxVelocity * Acceleration);
			}

			bool noticeablyAboveMaxSpeed = maxVelocity.x >= 0 ? currentVelocity.x > maxVelocity.x + 1f : currentVelocity.x > maxVelocity.x + 1f;

			shouldCheckToTurn = noticeablyAboveMaxSpeed == false;
		}
	}

	public void SlowDown(float decelleration)
	{
		if (!turning)
		{
			if (myRigidbody.velocity.x > 0.1f)
			{
				Accelerate(-myRigidbody.velocity * decelleration);
			}
		}
	}

	private void Accelerate(Vector2 acceleration)
	{
		myRigidbody.AddForce(acceleration * myRigidbody.mass);
	}

	public void CheckToTurn()
	{
		if (!turning)
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
				// Remeber to multiply the adjustment (RightDistance) by the direction we're heading to check
				Vector2 boxCentre = rightEdge.NewWithChange(deltaX: (RightDistance / 2) * Mathf.Sign(myRigidbody.velocity.x));
				Vector2 boxSize = new Vector2(RightDistance / 2, topEdge.y - bottomEdge.y - 0.2f);

				if (Debug_DrawDetectionBox) DrawBox(boxCentre, boxSize);

				//Collider2D somethingInFrontOfMe = Physics2D.OverlapBox(boxCentre, boxSize, 0, Notices.value);

				Collider2D[] thingsInFrontOfMe = Physics2D.OverlapBoxAll(point: boxCentre, size: boxSize, angle: 0, layerMask: Notices.value);

				if (thingsInFrontOfMe.Any(collider => collider.gameObject != gameObject))
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
		if (Debug_LogTurning) Debug.Log($"A {gameObject.name} started turning.");
		if (Debug_DrawDetectionBox) StopDrawing();
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


	#region BoxArtist

	private Vector3? boxPosition;
	private Vector3? boxSize;

	void OnDrawGizmos()
	{
		if (boxPosition.HasValue && boxSize.HasValue)
		{
			Gizmos.DrawCube(center: boxPosition.Value, size: boxSize.Value);
		}
	}

	public void DrawBox(Vector3 position, Vector3 size)
	{
		boxPosition = position;
		boxSize = size;
	}

	public void StopDrawing()
	{
		boxPosition = boxSize = null;
	}

	#endregion BoxArtist
}
