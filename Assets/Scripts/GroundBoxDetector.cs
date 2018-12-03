using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GroundBoxDetector : MonoBehaviour, IGrounded
{
	private BoxCollider2D myBoxCollider;

	void Awake()
	{
		myBoxCollider = GetComponent<BoxCollider2D>();
	}

	public bool IsGrounded()
	{
		Vector2 position = myBoxCollider.offset + (Vector2)myBoxCollider.gameObject.transform.position;
		Vector2 sizeDiameterStyle = myBoxCollider.size;
		Vector2 sizeRadiusStyle = sizeDiameterStyle / 2;
		Collider2D[] colliders = Physics2D.OverlapBoxAll(point: position, size: sizeRadiusStyle, angle: 0);
		if (colliders.Any(collider => 
			collider.gameObject != gameObject &&
			collider.gameObject == collider.transform.parent.gameObject))
		{
			return true;
		}

		return false;
	}

}
