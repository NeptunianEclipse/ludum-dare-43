using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeyMovement : MonoBehaviour
{
	public float shakeyness = 1f;
	public int shakeyUpdateFrequency = 1;
	public Vector2 myVelocity = Vector2.zero;

	public float maxVariance = 1f;

	public float distanceBeforeDisposing = 100f;

	private float cumulativeXShake = 0f;
	private float cumulativeYShake = 0f;

	private float xShake;
	private float yShake;

	private int fixedUpdateCounter = 0;

	private float cumulativeDistance = 0f;

	void Update()
	{
		Vector3 position = transform.position;
		float deltaTime = Time.deltaTime;

		float deltaX = myVelocity.x;
		float deltaY = myVelocity.y;

		deltaX += xShake;
		deltaY += yShake;

		cumulativeXShake += (xShake * deltaTime);
		cumulativeYShake += (yShake * deltaTime);

		deltaX *= deltaTime;
		deltaY *= deltaTime;

		transform.position = position.NewWithChange(deltaX, deltaY);

		cumulativeDistance += myVelocity.magnitude * deltaTime;

		if (cumulativeDistance >= distanceBeforeDisposing) Destroy(gameObject);
	}

	private void FixedUpdate()
	{
		if (++fixedUpdateCounter >= shakeyUpdateFrequency)
		{
			fixedUpdateCounter = 0;
			CalculateShakeyness();
		}
	}

	private void CalculateShakeyness()
	{
		Vector2 perpendicularToVelocity = myVelocity.Perpendicular().ElementwiseAbosolute();

		var probOfPositiveXDelta = Mathf.Clamp01(Mathf.InverseLerp(maxVariance, -maxVariance, cumulativeXShake));
		var probOfPositiveYDelta = Mathf.Clamp01(Mathf.InverseLerp(maxVariance, -maxVariance, cumulativeYShake));

		xShake = (Extensions.RandomBit(probOfPositiveXDelta) ? 1f : -1f) * shakeyness * perpendicularToVelocity.x;
		yShake = (Extensions.RandomBit(probOfPositiveYDelta) ? 1f : -1f) * shakeyness * perpendicularToVelocity.y;

		// Should we bias the randomness based on how much we've already shaken?
		// Only in the long run, so if we've shaken more than a certain amount we start to bias?
		// Once we start to correct we would want to keep the correction going for a while.
		// Maybe we want a different way about this where we generate a noise graph and then follow that?
	}
}
