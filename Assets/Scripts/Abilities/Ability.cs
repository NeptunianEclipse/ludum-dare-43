﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityController
{
	GameObject GameObject { get; }
	event Action Activate;
	event Action Tick;
}

public abstract class AbilityBase : ScriptableObject
{
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
