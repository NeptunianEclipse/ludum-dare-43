using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour, IAbilityController
{

	public List<AbilityBase> Abilities = new List<AbilityBase>();

	public GameObject GameObject => gameObject;

	public event Action Activate;

	private void Awake()
	{
		Abilities.Add(new MidAirJump());

		foreach(AbilityBase ability in Abilities)
		{
			ability.Controller = this;
		}
	}

	private void Update()
	{
		foreach(AbilityBase ability in Abilities)
		{
			if(Input.GetKeyDown(ability.ActivateKey))
			{
				ability.Activate();
			}
		}
	}

}
