using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesUI : MonoBehaviour
{
	public GameObject AbilityUIPrefab;
	public GameObject Tooltip;

	public PlayerAbilityController playerAbilityController;

	private Dictionary<AbilitySlot, AbilityUI> abilityUIMap;

	private void Start()
	{
		RefreshUI();
	}

	private void OnEnable()
	{
		if (Player.Instance == null)
		{
			enabled = false;
			return;
		}

		if (playerAbilityController == null)
		{
			playerAbilityController = Player.Instance.GetComponent<PlayerAbilityController>();
		}

		playerAbilityController.AbilitiesChanged += RefreshUI;
	}

	private void OnDisable()
	{
		if(Player.Instance == null)
		{
			enabled = false;
			return;
		}

		playerAbilityController.AbilitiesChanged -= RefreshUI;
	}

	public void RefreshUI()
	{	
		if(abilityUIMap != null)
		{
			foreach (AbilityUI abilityUI in abilityUIMap.Values)
			{
				Destroy(abilityUI.gameObject);
			}
		}
		

		abilityUIMap = new Dictionary<AbilitySlot, AbilityUI>();

		foreach (AbilitySlot slot in playerAbilityController.AllAbilitySlots)
		{
			AbilityUI abilityUI = Instantiate(AbilityUIPrefab, transform).GetComponent<AbilityUI>();
			abilityUI.AbilitySlot = slot;
			abilityUIMap[slot] = abilityUI;
			abilityUI.RefreshUI();
		}
	}

}
