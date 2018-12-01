﻿using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleController : MonoBehaviour
{
	public GameObject projectilePrefab;

	private GameObject projectileInstance = null;

	void Start()
	{

	}

	void Update()
	{
		if (projectilePrefab == null) return;
		if (PlayerInRange() && projectileInstance == null)
		{
			Vector2 playersDirection = new Vector2(-3f, 0f);

			var projectile = Instantiate<GameObject>(projectilePrefab, this.transform.position, Quaternion.identity);
			var projectileShakeyMovement = projectile.GetComponent<ShakeyMovement>();
			projectileShakeyMovement.myVelocity = playersDirection;

			projectileInstance = projectile;
		}
	}



	public bool PlayerInRange()
	{
		return true;
	}
}