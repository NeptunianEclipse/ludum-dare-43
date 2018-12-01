using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour, IAbilityController
{
	public List<AbilitySlot> Abilities = new List<AbilitySlot>();

	public GameObject GameObject => gameObject;

	public event Action Activate;
	public event Action Tick;

	private void Awake()
	{
		foreach(AbilitySlot slot in Abilities)
		{
			slot.Ability.Controller = this;
		}
	}

	private void Update()
	{
		foreach(AbilitySlot slot in Abilities)
		{
			if(Input.GetKeyDown(slot.ActivateKey))
			{
				slot.Ability.OnActivate();
			}
		}

		Tick?.Invoke();
	}

	

}

[System.Serializable]
public class AbilitySlot
{
	public KeyCode ActivateKey;
	public AbilityBase Ability;
}