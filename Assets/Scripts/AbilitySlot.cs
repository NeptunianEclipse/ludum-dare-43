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

	public virtual AbilityBase SwapInAbility(AbilityBase newAbility)
	{
		newAbility.transform.SetParent(transform);
		newAbility.Mode = AbilityMode.Passive;

		if(Ability == null)
		{
			Ability = newAbility;
			return null;
		}
		else
		{
			AbilityBase oldAbility = Ability;
			Ability = newAbility;
			return oldAbility;
		}
	}

	public virtual void UpdateInternalData()
	{
		Ability = GetComponentInChildren<AbilityBase>();
		gameObject.name = $"Slot [Passive]";
	}

}