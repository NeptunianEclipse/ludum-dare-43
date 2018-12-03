using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesUI : MonoBehaviour
{
	public GameObject AbilityUIPrefab;
	public Transform AbilityUIContainer;
	public GameObject Tooltip;

	public PlayerAbilityController playerAbilityController;

	private Dictionary<AbilitySlot, AbilityUI> abilityUIMap;

	private void Start()
	{
		if(GameManager.Instance.GameState == GameState.Levels)
		{
			ShowUI();
			RefreshUI();
		} else
		{
			HideUI();
		}
	}

	private void Update()
	{
		if(GameManager.Instance.GameState == GameState.Levels && playerAbilityController == null)
		{
			playerAbilityController = Player.Instance?.GetComponent<PlayerAbilityController>();

			if(playerAbilityController != null)
			{
				ShowUI();
				playerAbilityController.AbilitiesChanged += RefreshUI;
				RefreshUI();
			}
		}
	}

	private void OnEnable()
	{
		GameManager.Instance.GameStateChanged += OnGameStateChanged;
	}

	private void OnDisable()
	{

		GameManager.Instance.GameStateChanged -= OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState state)
	{
		if(state == GameState.Levels)
		{
			if(playerAbilityController == null)
			{
				playerAbilityController = Player.Instance?.GetComponent<PlayerAbilityController>();
			}
			
			if (playerAbilityController != null)
			{
				ShowUI();
				playerAbilityController.AbilitiesChanged += RefreshUI;
				RefreshUI();
			}
		}
		else
		{
			HideUI();
			if(playerAbilityController != null)
			{
				playerAbilityController.AbilitiesChanged -= RefreshUI;
			}
		}
	}

	public void ShowUI()
	{
		AbilityUIContainer.gameObject.SetActive(true);
	}

	public void HideUI()
	{
		AbilityUIContainer.gameObject.SetActive(false);
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
			AbilityUI abilityUI = Instantiate(AbilityUIPrefab, AbilityUIContainer).GetComponent<AbilityUI>();
			abilityUI.AbilitySlot = slot;
			abilityUIMap[slot] = abilityUI;
			abilityUI.RefreshUI();
		}
	}

}
