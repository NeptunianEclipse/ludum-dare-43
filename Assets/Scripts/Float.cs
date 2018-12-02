using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour 
{
	public float Frequency;
	public float Amplitude;
	public Vector2 Direction = Vector2.up;
	public bool TimeRandomOffset = true;

	private float timeOffset;

	private void Awake()
	{
		timeOffset = TimeRandomOffset ? Random.value * Mathf.PI * 2 : 0;
	}

	private void Update()
	{
		transform.localPosition = Direction * Mathf.Sin(Time.time * Frequency + timeOffset) * Amplitude;
	}

}
