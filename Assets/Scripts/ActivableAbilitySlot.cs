using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableAbilitySlot : AbilitySlot
{
	public KeyCode ActivateKey;

	public override void UpdateInternalData()
	{
		base.UpdateInternalData();
		gameObject.name = $"Slot [{ActivateKey.ToString()}]";
	}

	public override AbilityBase SwapInAbility(AbilityBase newAbility)
	{
		AbilityBase oldAbility = base.SwapInAbility(newAbility);
		return oldAbility;
	}
}