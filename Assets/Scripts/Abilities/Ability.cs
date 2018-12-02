using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityController
{
	GameObject GameObject { get; }
}

public abstract class AbilityBase : MonoBehaviour
{
	public IAbilityController Controller { get; protected set; }

	public abstract string Name { get; }
	public Sprite Sprite;

	public float TotalUsageTime { get; protected set; } = 0;
	public bool IsEquipped { get; private set; } = false;
	public bool Activated { get; private set; } = false;

	protected void Update()
	{
		if(IsEquipped)
		{
			TotalUsageTime += Time.deltaTime;
			Tick();
		}
	}

	protected void FixedUpdate()
	{
		if(IsEquipped)
		{
			FixedTick();
		}
	}

	public virtual void Activate() {
		Activated = true;
	}
	public virtual void Release() {
		Activated = false;
	}

	public void Equip(IAbilityController newController)
	{
		Controller = newController;
		IsEquipped = true;
		OnEquip();
	}

	public void Unequip()
	{
		IsEquipped = false;
		OnUnequip();
	}


	protected virtual void OnEquip() {
		
	}

	protected virtual void OnUnequip() {
		
	}

	protected virtual void Tick()
	{

	}

	protected virtual void FixedTick()
	{

	}

}

