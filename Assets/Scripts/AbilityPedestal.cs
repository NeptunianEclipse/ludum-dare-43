using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityPedestalState
{
	Inactive,
	Selected
}

[RequireComponent(typeof(Collider2D))]
public class AbilityPedestal : MonoBehaviour 
{
	public AbilityBase Ability { get; private set; }
	public SpriteRenderer IconRenderer;
	public Transform InactivePosition;
	public Transform SelectedPosition;

	public AbilityPedestalState State;

	private PlayerAbilityController playerAbilityController;

	

	public void SetAbility(AbilityBase ability)
	{
		Ability = ability;
		IconRenderer.sprite = Ability.Sprite;
	}

	private void Awake()
	{
		
	}

	private void Start()
	{
		
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision)
		{
			State = AbilityPedestalState.Selected;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject == playerAbilityController.gameObject)
		{
			playerAbilityController = null;
			State = AbilityPedestalState.Inactive;
		}
		
	}

	private void Update()
	{
		Vector2 targetPostion;
		if(State == AbilityPedestalState.Inactive)
		{
			targetPostion = InactivePosition.position;
		}
		else
		{
			targetPostion = SelectedPosition.position;
		}

		
	}


}
