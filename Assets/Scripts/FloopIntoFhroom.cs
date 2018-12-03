using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Throwable))]
public class FloopIntoFhroom : MonoBehaviour
{
	public float HeightToAdd = 0f;

	public GameObject FhroomPrefab;

	private Throwable myThrowable;

	public Vector2 startScale = new Vector2(0.01f, 0.01f);
	public Vector2 endScale = new Vector2(1f, 1f);

	void Awake()
	{
		myThrowable = gameObject.GetComponent<Throwable>();
		
		if (FhroomPrefab == null) Debug.LogError($"A {gameObject.name} doesn't have a prefab set in the {nameof(FloopIntoFhroom)} component.");
	}

	void OnEnable()
	{
		myThrowable.Expired += MyThrowable_Expired;
	}

	void OnDisable()
	{
		myThrowable.Expired -= MyThrowable_Expired;
	}

	void Start()
	{
		transform.localScale = startScale;

		var deltaScale = endScale - startScale;
		var duration = myThrowable.MaxTimeAlive;
		StartCoroutine(Grow(deltaScale, duration, endScale));
	}

	private void MyThrowable_Expired(object sender, System.EventArgs e)
	{
		Vector3 position = transform.position.NewWithChange(deltaY: HeightToAdd);
		Quaternion rotation = FhroomPrefab.transform.rotation;
		Instantiate(FhroomPrefab, position, rotation);
	}

	private IEnumerator Grow(Vector2 deltaScale, float totalDuration, Vector2 endingScale)
	{
		Vector2 totalGrowth = Vector2.zero;

		while (totalGrowth.magnitude < endingScale.magnitude)
		{
			float duration = myThrowable.MaxTimeAlive;
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
