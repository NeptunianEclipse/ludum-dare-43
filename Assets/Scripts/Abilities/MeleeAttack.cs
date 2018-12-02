using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Melee Attack")]
public class MeleeAttack : AbilityBase, IRecoverable
{
	public override string Name => "Melee Attack";

	public float RecoveryPercent => weapon.RecoveryPercent;

	public MeleeWeaponController WeaponPrefab;

	private MeleeWeaponController weapon;

	public override void Activate()
	{
		weapon.TryAttack();
	}

	protected override void Initialize()
	{
		base.Initialize();

		weapon = Instantiate(WeaponPrefab.gameObject, controller.GameObject.transform).GetComponent<MeleeWeaponController>();
	}

}
