using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AbilityPool : MonoBehaviour
{
	public Transform PoolContainer;

	public List<AbilityBase> StandardAbilities { get; private set; }

	public List<AbilityBase> GetRandomAbilities(int num)
	{
		var abilities = new List<AbilityBase>();
		for (int i = 0; i < num; i++)
		{
			abilities.Add(GetRandomAbility());
		}
		return abilities;
	}

	private void Awake()
	{
		UpdateInternalData();
	}

	public AbilityBase GetRandomAbility()
	{
		float probabilitySum = StandardAbilities.Sum(a => AbilityPickProbability(a));
		float rand = Random.value * probabilitySum;
		float currentValue = 0;
		foreach (AbilityBase ability in StandardAbilities)
		{
			currentValue += AbilityPickProbability(ability);
			if (currentValue >= rand)
			{
				return ability;
			}
		}
		return null;
	}

	public void AddAbility(AbilityBase ability)
	{
		ability.transform.SetParent(PoolContainer);
		StandardAbilities.Add(ability);
	}

	public void AbilityWasRemoved(AbilityBase ability)
	{
		StandardAbilities.Remove(ability);
	}

	public float AbilityPickProbability(AbilityBase ability)
	{
		return 1 / Mathf.Max(ability.TotalUsageTime, 1);
	}

	private void UpdateInternalData()
	{
		StandardAbilities = new List<AbilityBase>();
		foreach(Transform child in PoolContainer)
		{
			var ability = child.GetComponent<AbilityBase>();
			StandardAbilities.Add(ability);
		}
	}
}