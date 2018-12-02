using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesUI : MonoBehaviour
{
	public GameObject AbilityUIPrefab;

	public PlayerAbilityController playerAbilityController;

	private Dictionary<AbilitySlot, AbilityUI> abilityUIMap;

	private void Awake()
	{
		if(playerAbilityController == null)
		{
			playerAbilityController = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerAbilityController>();
		}
	}

	private void Start()
	{
		RefreshUI();
	}

	private void OnEnable()
	{
		playerAbilityController.AbilitiesChanged += RefreshUI;
	}

	private void OnDisable()
	{
		playerAbilityController.AbilitiesChanged -= RefreshUI;
	}

	public void RefreshUI()
	{
		abilityUIMap = new Dictionary<AbilitySlot, AbilityUI>();
		foreach(AbilityUI abilityUI in abilityUIMap.Values)
		{
			Destroy(abilityUI.gameObject);
		}

		foreach (AbilitySlot slot in playerAbilityController.Abilities)
		{
			AbilityUI abilityUI = Instantiate(AbilityUIPrefab, transform).GetComponent<AbilityUI>();
			abilityUI.AbilitySlot = slot;
			abilityUIMap[slot] = abilityUI;
			abilityUI.RefreshUI();
		}
	}

}
