using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
	public void InflictDamage(float damage)
	{
		Debug.Log($"This ({gameObject.name}) took {damage.ToString("N1")} damage.");
	}
}
