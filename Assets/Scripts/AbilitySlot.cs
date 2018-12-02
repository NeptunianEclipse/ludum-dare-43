using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySlot : MonoBehaviour
{
	public AbilityBase Ability { get; private set; }

	public void AbilityWasRemoved()
	{
		Ability = null;
	}

	public AbilityBase SwapInAbility(AbilityBase newAbility)
	{
		if(Ability == null)
		{
			Ability = newAbility;
			Ability.transform.SetParent(transform);
			return null;
		}
		else
		{
			AbilityBase oldAbility = Ability;
			Ability = newAbility;
			Ability.transform.SetParent(transform);
			return oldAbility;
		}
	}

	public virtual void UpdateInternalData()
	{
		Ability = GetComponentInChildren<AbilityBase>();
		gameObject.name = $"Slot [Passive]";
	}

}