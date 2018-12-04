using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
	public bool ShouldDrift = true;
	public Vector2 DriftDirection = new Vector2(1, 0);
	public float DriftSpeed = 1f;
	public float OscillationAmplitude = 0.5f;
	public float OscillationVariance = 0.2f;

	private float oscillationOffset;

	void Start()
	{
		oscillationOffset = Random.Range(0, 2 * Mathf.PI);
	}

	// Update is called once per frame
	void Update()
	{
		if (ShouldDrift)
		{
			float driftAmount = DriftSpeed * Time.deltaTime;

			float dirftX = DriftDirection.normalized.x * driftAmount;
			float driftY = DriftDirection.normalized.y * driftAmount;
			float randomMovement = Mathf.Sin(Time.time + oscillationOffset) + Random.Range(min: -OscillationVariance, max: OscillationVariance);
			float oscillationY = randomMovement * OscillationAmplitude * Time.deltaTime;
			transform.position = transform.position.NewWithChange(deltaX: dirftX, deltaY: driftY + oscillationY);
		}
	}
}
