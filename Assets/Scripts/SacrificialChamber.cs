﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SacrificialChamberState
{
	Inactive,
	Dispensing,
	WaitingForSacrifice,
	Sacrificing,
	Bestowing,
	WaitingForChoice,
	CleaningUp
}

public class SacrificialChamber : MonoBehaviour 
{
	public List<AbilityPedestal> abilityPedestals;
	public GameObject LeftBarrier;
	public GameObject RightBarrier;

	public SacrificialChamberState State = SacrificialChamberState.Inactive;

	private bool activated = false;

	private PlayerAbilityController playerAbilityController;

	private AbilitySlot currentSlot;

	private void Awake()
	{
		foreach(AbilityPedestal pedestal in abilityPedestals)
		{
			pedestal.SacrificialChamber = this;
		}

		LeftBarrier.SetActive(false);
		RightBarrier.SetActive(true);

		UI.Instance.SacrificeMessage.gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag(Tags.Player) && !activated)
		{
			activated = true;
			playerAbilityController = collision.GetComponent<PlayerAbilityController>();
			PlayerEnteredChamber();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.CompareTag(Tags.Player))
		{
			PlayerExitedChamber();
		}
	}

	private void PlayerEnteredChamber()
	{
		UI.Instance.SacrificeMessage.gameObject.SetActive(true);

		State = SacrificialChamberState.Dispensing;
		LeftBarrier.SetActive(true);

		List<AbilitySlot> sacrificeOptions = playerAbilityController.GetRandomFullSlots(abilityPedestals.Count);
		for(int i = 0; i < sacrificeOptions.Count; i++)
		{
			abilityPedestals[i].SetAbilitySlot(sacrificeOptions[i]);
			abilityPedestals[i].PlayerAbilityController = playerAbilityController;
		}

		foreach(AbilityPedestal pedestal in abilityPedestals)
		{
			pedestal.PlayerEnteredChamber();
		}
	}

	private void PlayerExitedChamber()
	{
		foreach (AbilityPedestal pedestal in abilityPedestals)
		{
			pedestal.PlayerExitedChamber();
		}
	}

	public IEnumerator SacrificeSelected(AbilitySlot slot)
	{
		currentSlot = slot;
		AbilityBase ability = playerAbilityController.UnequipAbility(slot);
		playerAbilityController.AbilityPool.AddAbility(ability);

		var coroutines = new List<Coroutine>();
		foreach(AbilityPedestal pedestal in abilityPedestals)
		{
			coroutines.Add(StartCoroutine(pedestal.PlayerSelectedSacrifice(slot)));
		}

		foreach(Coroutine coroutine in coroutines)
		{
			yield return coroutine;
		}

		UI.Instance.SacrificeMessage.gameObject.SetActive(false);

		yield return Bestow();
	}

	private IEnumerator Bestow()
	{


		List<AbilityBase> giftOptions = playerAbilityController.AbilityPool.GetRandomAbilities(abilityPedestals.Count);
		for(int i = 0; i < giftOptions.Count; i++)
		{
			abilityPedestals[i].SetGiftAbility(giftOptions[i]);
		}

		var coroutines = new List<Coroutine>();
		foreach(AbilityPedestal pedestal in abilityPedestals)
		{
			coroutines.Add(StartCoroutine(pedestal.BestowGifts()));
		}

		foreach(Coroutine coroutine in coroutines)
		{
			yield return coroutine;
		}
	}

	public IEnumerator GiftSelected(AbilityBase ability)
	{
		RightBarrier.SetActive(false);

		playerAbilityController.EquipAbility(ability, currentSlot);

		var coroutines = new List<Coroutine>();
		foreach (AbilityPedestal pedestal in abilityPedestals)
		{
			coroutines.Add(StartCoroutine(pedestal.PlayerSelectedGift(ability)));
		}

		foreach(Coroutine coroutine in coroutines)
		{
			yield return coroutine;
		}

		yield return EndSequence();
	}

	private IEnumerator EndSequence()
	{
		yield return null;
		RightBarrier.SetActive(false);
	}

}
