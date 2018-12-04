using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : AbilityBase, IRecoverable
{
	public float RecoveryPercent => Mathf.Clamp01((Time.time - lastThrowTime) / TimeBetweenThrows);
	
	public Throwable ProjectilePrefab;
	public Vector2 ThrowForce;
	public float TimeBetweenThrows;

	private Thrower thrower;
	private float lastThrowTime;

	protected override void OnEquip()
	{
		base.OnEquip();

		thrower = Controller.GameObject.AddComponent<Thrower>();
		thrower.ProjectilePrefab = ProjectilePrefab.gameObject;
	}

	protected override void OnUnequip()
	{
		base.OnUnequip();

		Destroy(thrower);
	}

	public override void Activate()
	{
		base.Activate();

		if(Time.time >= lastThrowTime + TimeBetweenThrows)
		{
			var sign = Mathf.Sign(Controller.GameObject.transform.right.x);
			thrower.ThrowProjectile(new Vector2(ThrowForce.x * sign, ThrowForce.y));
			lastThrowTime = Time.time;
		}
		
	}

}
