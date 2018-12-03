using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidDamageOrDestroy : MonoBehaviour
{
	public bool AlwaysDestroy;

	public float HeightToDamageIfBelow = -10f;

	public float DamageAmount = 10f;
	public float DamagePeriod = 1f;

	private Damageable maybeDamageable;

	void Awake()
	{
		maybeDamageable = GetComponent<Damageable>();

		StartCoroutine(VoidDamageOrDestory());
	}

	private IEnumerator<WaitForSeconds> VoidDamageOrDestory()
	{
		while (true)
		{
			if (transform.position.y < HeightToDamageIfBelow)
			{
				if (AlwaysDestroy || maybeDamageable == null)
				{
					Destroy(gameObject);
					yield break;
				}
				else
				{
					maybeDamageable.InflictDamage(DamageAmount);

					if (maybeDamageable.CurrentHealth < -DamageAmount)
					{
						Destroy(gameObject);
						yield break;
					}
				}
			}

			yield return new WaitForSeconds(DamagePeriod);
		}
	}
}