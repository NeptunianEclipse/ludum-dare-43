using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleController : MonoBehaviour
{
	public GameObject ProjectilePrefab;

	private GameObject projectileInstance = null;

	void Update()
	{
		if (ProjectilePrefab == null) return;
		if (PlayerInRange() && projectileInstance == null)
		{
			Vector2 playersDirection = new Vector2(-3f, 0f);

			var projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
			var projectileShakeyMovement = projectile.GetComponent<ShakeyMovement>();
			projectileShakeyMovement.myVelocity = playersDirection;
			projectileShakeyMovement.transform.rotation = Quaternion.LookRotation(playersDirection);

			projectileInstance = projectile;
		}
	}
	

	public bool PlayerInRange()
	{
		return true;
	}
}
