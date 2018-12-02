using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AbilityPool))]
public class PlayerAbilityController : MonoBehaviour, IAbilityController
{
	public Transform SlotsContainer;

	public GameObject GameObject => gameObject;

	public event Action AbilitiesChanged;

	public IList<AbilitySlot> AllAbilitySlots => passiveAbilitySlots.Concat(activableAbilitySlots).ToList();

	private List<AbilitySlot> passiveAbilitySlots;
	private List<ActivableAbilitySlot> activableAbilitySlots;

	private AbilityPool abilityPool;

	private void Awake()
	{
		abilityPool = GetComponent<AbilityPool>();
		UpdateInternalData();
	}

	private void Update()
	{
		foreach(ActivableAbilitySlot slot in activableAbilitySlots)
		{
			if(Input.GetKeyDown(slot.ActivateKey) && slot.Ability != null)
			{
				slot.Ability.Activate();
			}
			if(Input.GetKeyUp(slot.ActivateKey) && slot.Ability != null)
			{
				slot.Ability.Release();
			}
		}

	}

	public AbilityBase EquipAbility(AbilityBase ability, AbilitySlot slot)
	{
		AbilityBase oldAbility = slot.SwapInAbility(ability);
		if(oldAbility != null)
		{
			abilityPool.AddAbility(oldAbility);
		}
		ChangedAbilities();

		return oldAbility;
	}

	public void ChangedAbilities()
	{
		AbilitiesChanged?.Invoke();
	}

	public void UpdateInternalData()
	{
		passiveAbilitySlots = new List<AbilitySlot>();
		activableAbilitySlots = new List<ActivableAbilitySlot>();

		foreach (Transform child in SlotsContainer)
		{
			AbilitySlot slot = child.GetComponent<AbilitySlot>();
			if (slot is ActivableAbilitySlot)
			{
				var activableSlot = (ActivableAbilitySlot)slot;
				activableAbilitySlots.Add(activableSlot);
			}
			else
			{
				passiveAbilitySlots.Add(slot);
			}
			slot.UpdateInternalData();

			if (slot.Ability != null)
			{
				slot.Ability.Equip(this);
			}
		}
	}

}