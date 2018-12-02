using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour 
{
	public float Frequency;
	public float Amplitude;
	public bool TimeRandomOffset = true;

	private float timeOffset;

	private void Awake()
	{
		timeOffset = TimeRandomOffset ? Random.value * Mathf.PI * 2 : 0;
	}

	private void Update()
	{
		transform.localRotation = Quaternion.AngleAxis(Mathf.Sin(Time.time * Frequency + timeOffset) * Amplitude, Vector3.forward);
	}
}
