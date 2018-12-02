using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float GroundDrag;
	public float AirHorizontalDrag;
	public float JumpPower;
	public float MaxSpeed;

	public GameObject Feet;

	public bool HorizontalMovementAllowed { get; set; } = true;

	public event Action Grounded;
	public event Action<Collider2D, Vector2> FeetTriggerEnter;

	public Rigidbody2D Rigidbody2d { get; private set; }

	private BoxCollider2D feetArea;
	private FeetCollisions feetCollisions;

	private bool willJump;
	private bool wasGrounded;

	private void Awake()
	{
		Rigidbody2d = GetComponent<Rigidbody2D>();
		feetCollisions = Feet.GetComponent<FeetCollisions>();
		feetArea = Feet.GetComponent<BoxCollider2D>();
	}

	private void OnEnable()
	{
		feetCollisions.TriggerEnter += OnFeetTriggerEnter;
	}

	private void OnDisable()
	{
		feetCollisions.TriggerEnter -= OnFeetTriggerEnter;
	}

	private void FixedUpdate()
	{
		Vector2 velocity = Rigidbody2d.velocity;
		bool isGrounded = IsGrounded();

		if (HorizontalMovementAllowed)
		{
			//if (Input.GetKey(KeyCode.D))
			//{
			//	velocity += Vector2.right * Acceleration;
			//}
			//if (Input.GetKey(KeyCode.A))
			//{
			//	velocity -= Vector2.right * Acceleration;
			//}
		}

		if (isGrounded)
		{
			velocity.x /= GroundDrag;
		}
		else
		{
			velocity.x /= AirHorizontalDrag;
		}

		if (!wasGrounded && isGrounded)
		{
			Grounded?.Invoke();
		}

		if (willJump)
		{
			velocity += Vector2.up * JumpPower;
			willJump = false;
		}

		Rigidbody2d.velocity = new Vector2(Mathf.Clamp(velocity.x, -MaxSpeed, MaxSpeed), velocity.y);

		wasGrounded = isGrounded;
	}

	public void Jump()
	{
		willJump = true;
	}

	public bool IsGrounded()
	{
		return Physics2D.OverlapBox(feetArea.offset + (Vector2)feetArea.transform.position, feetArea.size / 2, 0, ~LayerMask.GetMask("Player"));
	}

	public int MovingDirection()
	{
		return (int)Mathf.Sign(Rigidbody2d.velocity.x);
	}

	private void OnFeetTriggerEnter(Collider2D collider)
	{
		FeetTriggerEnter?.Invoke(collider, Feet.transform.position);
	}


}
