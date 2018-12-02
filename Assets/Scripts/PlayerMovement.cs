﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float Acceleration;
	public float MaxSpeed;
	public float GroundDrag;
	public float JumpPower;

	public BoxCollider2D FeetArea;

	public bool HorizontalMovementAllowed { get; set; } = true;

	private Rigidbody2D rb2d;
	private ContactFilter2D feetFilter;

	public event Action Grounded;

	private bool willJump;
	private bool wasGrounded;

	private void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		feetFilter = new ContactFilter2D();
	}

	private void FixedUpdate()
	{
		Vector2 velocity = rb2d.velocity;
		bool isGrounded = IsGrounded();
		
		if(HorizontalMovementAllowed)
		{
			if (Input.GetKey(KeyCode.D))
			{
				velocity += Vector2.right * Acceleration;
			}
			if (Input.GetKey(KeyCode.A))
			{
				velocity -= Vector2.right * Acceleration;
			}
		}

		if(isGrounded)
		{
			velocity.x /= GroundDrag;
		}

		if(!wasGrounded && isGrounded)
		{
			Grounded?.Invoke();
		}

		if(willJump)
		{
			velocity += Vector2.up * JumpPower;
			willJump = false;
		}

		rb2d.velocity = new Vector2(Mathf.Clamp(velocity.x, -MaxSpeed, MaxSpeed), velocity.y);

		wasGrounded = isGrounded;
	}

	public void Jump()
	{
		willJump = true;
	}

	public bool IsGrounded()
	{
		return Physics2D.OverlapBox(FeetArea.offset + (Vector2)FeetArea.transform.position, FeetArea.size / 2, 0, ~LayerMask.GetMask("Player"));
	}

	public int MovingDirection()
	{
		return (int)Mathf.Sign(rb2d.velocity.x);
	}

	
}
