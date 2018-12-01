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

	private float cumulativeXShake = 0f;
	private float cumulativeYShake = 0f;

	private float xShake;
	private float yShake;

	private int fixedUpdateCounter = 0;

	void Update()
	{
		Vector3 position = transform.position;

		float deltaX = myVelocity.x;
		float deltaY = myVelocity.y;

		deltaX += xShake;
		deltaY += yShake;

		cumulativeXShake += (xShake * Time.deltaTime);
		cumulativeYShake += (yShake * Time.deltaTime);

		deltaX *= Time.deltaTime;
		deltaY *= Time.deltaTime;

		transform.position = position.NewWithChange(deltaX, deltaY);
	}

	private void FixedUpdate()
	{
		if (++fixedUpdateCounter % shakeyUpdateFrequency == 0) CalculateShakeyness();
	}

	private void CalculateShakeyness()
	{
		Vector2 perpendicularToVelocity = myVelocity.Perpendicular().ElementwiseAbosolute();

		var proportionOfMaxVarianceCovered = Mathf.Min(maxVariance / cumulativeXShake, 1);

		var probOfPositiveXDelta = Mathf.Clamp01(Mathf.InverseLerp(maxVariance, -maxVariance, cumulativeXShake));
		var probOfPositiveYDelta = Mathf.Clamp01(Mathf.InverseLerp(maxVariance, -maxVariance, cumulativeYShake));

		xShake = (Extensions.RandomBit(probOfPositiveXDelta) ? 1f : -1f) * shakeyness * perpendicularToVelocity.x;
		yShake = (Extensions.RandomBit(probOfPositiveYDelta) ? 1f : -1f) * shakeyness * perpendicularToVelocity.y;

		// Currently unused, should we bias the randomness based on how much we've already shaken?
		// Only in the long run, so if we've shaken more than a certain amount we start to bias?
		// Once we start to correct we would want to keep the correction going for a while.
		// Maybe we want a different way about this where we generate a noise graph and then follow that?
	}
}
