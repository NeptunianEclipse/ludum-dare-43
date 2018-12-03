using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growable : MonoBehaviour
{
	public Vector2 startScale = new Vector2(0.01f, 0.01f);
	public Vector2 endScale = new Vector2(1f, 1f);

	public void StartGrow(float duration)
	{
		transform.localScale = startScale;

		var deltaScale = endScale - startScale;

		StartCoroutine(Grow(deltaScale, duration, endScale));
	}
	
	private IEnumerator Grow(Vector2 deltaScale, float duration, Vector2 endingScale)
	{
		Vector2 totalGrowth = Vector2.zero;

		while (totalGrowth.magnitude < endingScale.magnitude)
		{
			float percentageOfGrowthSinceLastFrame = Time.deltaTime / duration;

			Vector2 growth = deltaScale * percentageOfGrowthSinceLastFrame;
			transform.localScale += (Vector3)growth;

			totalGrowth += growth;

			yield return null;
		}

		// Remove any rounding errors from frame times not summing to the duration exactly.
		transform.localScale = endingScale;
	}
}
