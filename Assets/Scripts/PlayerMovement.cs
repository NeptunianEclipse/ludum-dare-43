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

	private Rigidbody2D rb2d;
	private ContactFilter2D feetFilter;


	private void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		feetFilter = new ContactFilter2D();
		//feetFilter./*SetLayerMask*/();
	}

	private void FixedUpdate()
	{
		Vector2 velocity = rb2d.velocity;

		if(Input.GetKey(KeyCode.D))
		{
			velocity += Vector2.right * Acceleration;
		}
		if(Input.GetKey(KeyCode.A))
		{
			velocity -= Vector2.right * Acceleration;
		}

		

		if(Grounded())
		{
			Debug.Log("grounded");
			if(Input.GetKey(KeyCode.Space))
			{
				velocity += Vector2.up * JumpPower;
			}
			velocity.x /= GroundDrag;
		}

		rb2d.velocity = new Vector2(Mathf.Clamp(velocity.x, -MaxSpeed, MaxSpeed), velocity.y);
	}

	

	private bool Grounded()
	{
		return Physics2D.OverlapBox(FeetArea.offset + (Vector2)FeetArea.transform.position, FeetArea.size / 2, 0, ~LayerMask.GetMask("Player"));
	}

	
}
