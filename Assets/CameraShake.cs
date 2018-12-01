using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public IEnumerator Shake(float duration, float magnitude)
	{
		Vector3 startingPosition = transform.localPosition;

		float elapsed = 0f;
		while (elapsed < duration)
		{
			float deltaX = Random.Range(-1f, 1f) * magnitude;
			float deltaY = Random.Range(-1f, 1f) * magnitude;

			transform.localPosition = transform.localPosition.NewWithChange(deltaX, deltaY);

			elapsed += Time.deltaTime;

			yield return null;
		}

		transform.localPosition = startingPosition;
	}
}
