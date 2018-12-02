using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class SploofController : MonoBehaviour
{
	private Damageable damageable;

	private void Awake()
	{
		damageable = gameObject.GetComponent<Damageable>();
	}

	private void OnEnable()
	{
		damageable.Destroyed += Damageable_Destroyed;
	}

	private void OnDisable()
	{
		damageable.Destroyed -= Damageable_Destroyed;
	}

	private void Damageable_Destroyed(object sender, System.EventArgs e)
	{
		// for now, just make them real squished.
		transform.localScale = new Vector3(transform.localScale.x, 0.2f);
	}
}
