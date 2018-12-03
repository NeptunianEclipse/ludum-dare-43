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

	public AbilityPool AbilityPool;

	private void Awake()
	{
		AbilityPool = GetComponent<AbilityPool>();
	}

	private void Start()
	{
		UpdateInternalData();
	}

	private void Update()
	{
		foreach(ActivableAbilitySlot slot in activableAbilitySlots)
		{
			if(Input.GetKeyDown(slot.ActivateKey))
			{
				slot.Ability?.Activate();
			}
			if (Input.GetKey(slot.ActivateKey))
			{
				slot.Ability?.During();
			}
			if (Input.GetKeyUp(slot.ActivateKey))
			{
				slot.Ability?.Release();
			}
		}

	}

	public AbilityBase EquipAbility(AbilityBase ability, AbilitySlot slot)
	{
		AbilityBase oldAbility = slot.SwapInAbility(ability);
		if(oldAbility != null)
		{
			AbilityPool.AddAbility(oldAbility);
			oldAbility.Unequip();
		}
		ChangedAbilities();
		ability.Equip(this);

		return oldAbility;
	}

	public AbilityBase UnequipAbility(AbilitySlot slot)
	{
		AbilityBase ability = slot.Ability;
		slot.Ability.Unequip();
		slot.AbilityWasRemoved();
		ChangedAbilities();
		return ability;
	}

	public int GetNumberOfFullSlots()
	{
		int n = 0;

		foreach(AbilitySlot slot in AllAbilitySlots)
		{
			if(slot.Ability != null)
			{
				n++;
			}
		}

		return n;
	}

	public AbilitySlot GetRandomFullSlot()
	{
		List<AbilitySlot> allSlots = AllAbilitySlots.ToList();
		float probabilitySum = allSlots.Sum(s => s.Ability == null ? 0 : AbilityPool.AbilityPickProbability(s.Ability));
		float rand = UnityEngine.Random.value * probabilitySum;
		float currentValue = 0;
		foreach (AbilitySlot slot in allSlots)
		{
			currentValue += AbilityPool.AbilityPickProbability(slot.Ability);
			if (currentValue >= rand)
			{
				return slot;
			}
		}
		return null;
	}

	public List<AbilitySlot> GetRandomFullSlots(int num)
	{
		int n = Mathf.Min(num, GetNumberOfFullSlots());
		if(n < num)
		{
			Debug.LogWarning($"Attempting to get {num} full slots when there are only {n}!");
		}

		var slots = new List<AbilitySlot>();
		for(int i = 0; i < n; i++)
		{
			while(true)
			{
				AbilitySlot slot = GetRandomFullSlot();
				if (slots.Contains(slot) == false)
				{
					slots.Add(slot);
					break;
				}
			}
		}
		return slots;
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