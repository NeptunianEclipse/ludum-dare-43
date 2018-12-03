using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FollowPlayer : Follow 
{
	private void Awake()
	{
		FollowObject = Player.Instance?.transform;
	}

	protected override void Update()
	{
		base.Update();
		FollowObject = Player.Instance?.transform;
	}
}
