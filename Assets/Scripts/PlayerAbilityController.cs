using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour, IAbilityController
{
	public List<AbilitySlot> Abilities = new List<AbilitySlot>();

	public GameObject GameObject => gameObject;

	public event Action Tick;
	public event Action AbilitiesChanged;

	private void Awake()
	{
		foreach (AbilitySlot slot in Abilities)
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
				slot.Ability.Activate();
			}
		}

		Tick?.Invoke();
	}

	public void ChangedAbilities()
	{
		AbilitiesChanged?.Invoke();
	}

}

[System.Serializable]
public class AbilitySlot
{
	public KeyCode ActivateKey;
	public AbilityBase Ability;
}