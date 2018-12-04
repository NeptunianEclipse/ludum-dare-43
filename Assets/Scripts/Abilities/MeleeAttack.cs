using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AbilityBase, IRecoverable
{
	public float RecoveryPercent => weaponController.RecoveryPercent;

	public GameObject WeaponPrefab;

	public override bool IsPassive => false;

	private MeleeWeaponController weaponController;

	protected override void OnEquip()
	{
		base.OnEquip();

		if (WeaponPrefab != null)
		{
			weaponController = Instantiate(WeaponPrefab, Controller.GameObject.transform).GetComponent<MeleeWeaponController>();
		}
		else
		{
			Debug.LogError($"A {Controller.GameObject.name} was given a {nameof(MeleeAttack)} ability but the ability doesn't have a {nameof(WeaponPrefab)} set.");
		}
	}

	protected override void OnUnequip()
	{
		base.OnUnequip();

		Destroy(weaponController.gameObject);
	}

	public override void Activate()
	{
		base.Activate();

		weaponController.TryAttack();
	}

}
