using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
	public Animator Animator;

	private Rigidbody2D myRigidbody;

	void Awake()
	{
		myRigidbody = GetComponent<Rigidbody2D>();

		if (Animator == null) Debug.LogError($"A {gameObject.name} hasn't set an {nameof(Animator)} on the {nameof(PlayerAnimator)} component.");
	}

	void Update()
	{
		var speed = Mathf.Abs(myRigidbody.velocity.x);
		Animator.SetFloat("Speed", speed);
	}
}
