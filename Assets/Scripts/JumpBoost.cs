using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoost : MonoBehaviour
{
	public float BoostMagnitude = 10f;
	public Vector2 BoostDirection = Vector2.up;
	public float BoostTimeout = 1f;

	public bool BoostAllMassesTheSame = true;

	public bool Debug_LogJumps = false;

	private readonly List<Rigidbody2D> boostItems = new List<Rigidbody2D>();
	private readonly List<Rigidbody2D> recentlyBoosted = new List<Rigidbody2D>();

	private void FixedUpdate()
	{
		boostItems.ForEach((rigidbody) =>
		{
			var force = BoostDirection.normalized * BoostMagnitude * (BoostAllMassesTheSame ? rigidbody.mass : 5f);
			rigidbody.AddForce(force, ForceMode2D.Impulse);
			recentlyBoosted.Add(rigidbody);
			StartCoroutine(Extensions.InvokeAfter(() => recentlyBoosted.Remove(rigidbody), BoostTimeout));
		});

		boostItems.Clear();

		//// Gives an about 1% chance of getting double boost (for 5 frames of contact).
		//if (Time.frameCount % 500 == 0) recentlyBoosted.Clear();
	}

	private int collisions = 0;

	void OnCollisionEnter2D(Collision2D collision)
	{
		Rigidbody2D rigidbody = collision.rigidbody;
		if (rigidbody != null)
		{
			if (!recentlyBoosted.Contains(rigidbody))
			{
				bool added = boostItems.AddIfMissing(rigidbody);
				if (added && Debug_LogJumps)
				{
					Debug.Log($"Jumps boosted: {++collisions}");
				};
			}
		}
		else
		{
			Debug.LogWarning($"A {gameObject.name}'s {nameof(JumpBoost)} collided with something without a rigidbody.");
		}
	}

	//private class BoostItem
	//{
	//	public Rigidbody2D rigidbody;
	//	public Vector2 forceToAdd;
	//}
	
}
