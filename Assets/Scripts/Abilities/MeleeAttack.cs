using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AbilityBase
{
	public override string Name => "Melee Attack";

	public GameObject WeaponPrefab;

	private MeleeWeaponController weaponController;

	protected override void OnEquip()
	{
		base.OnEquip();

		weaponController = Instantiate(WeaponPrefab, Controller.GameObject.transform).GetComponent<MeleeWeaponController>();
	}

	protected override void OnUnequip()
	{
		base.OnUnequip();
		Destroy(weaponController.gameObject);
	}

	public override void Activate()
	{
		weaponController.TryAttack();
	}

}
