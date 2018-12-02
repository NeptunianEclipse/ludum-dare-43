using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnStart : MonoBehaviour 
{
	public Behaviour ComponentToEnable;
	public bool Enable = true;

	public enum EnableMode
	{
		Awake,
		Start
	}

	public EnableMode When;

	private void Awake()
	{
		if(When == EnableMode.Awake)
		{
			ComponentToEnable.enabled = Enable;
		}
	}

	private void Start()
	{
		if(When == EnableMode.Start)
		{
			ComponentToEnable.enabled = Enable;
		}
	}
}
