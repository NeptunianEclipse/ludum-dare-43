using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeyMovement : MonoBehaviour
{
	public float shakeyness = 1f;
	public Vector2 myVelocity = Vector2.zero;

	private float cumulativeXShake = 0f;
	private float cumulativeYShake = 0f;

	void Update()
	{
		Vector3 position = transform.position;

		float deltaX = myVelocity.x;
		float deltaY = myVelocity.y;

		float xShake, yShake;
		CalculateShakeyness(out xShake, out yShake);

		deltaX += xShake;
		deltaY += yShake;

		deltaX *= Time.deltaTime;
		deltaY *= Time.deltaTime;

		transform.position = position.NewWithChange(deltaX, deltaY);
	}

	private void CalculateShakeyness(out float xShake, out float yShake)
	{
		Vector2 perpendicularToVelocity = myVelocity.Perpendicular();

		xShake = (Extensions.RandomBit() ? 1f : -1f) * shakeyness * perpendicularToVelocity.x;
		yShake = (Extensions.RandomBit() ? 1f : -1f) * shakeyness * perpendicularToVelocity.y;

		// Currently unused, should we bias the randomness based on how much we've already shaken?
		// Only in the long run, so if we've shaken more than a certain amount we start to bias?
		// Once we start to correct we would want to keep the correction going for a while.
		// Maybe we want a different way about this where we generate a noise graph and then follow that?
		cumulativeXShake += xShake;
		cumulativeYShake += yShake;
	}
}
