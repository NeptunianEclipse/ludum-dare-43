using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityController
{
	GameObject GameObject { get; }
	event Action Activate;
}

public abstract class AbilityBase : ScriptableObject
{
	public abstract KeyCode ActivateKey { get; }

	protected IAbilityController controller;

	public IAbilityController Controller {
		get
		{
			return controller;
		}
		set
		{
			controller = value;
			Initialize();
		}
	}

	public abstract void OnActivate();

	protected virtual void Initialize()
	{
	}

}

