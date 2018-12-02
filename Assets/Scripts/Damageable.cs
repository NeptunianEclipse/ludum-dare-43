using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
	public float MaxHealth = 100f;

	public FloatingText DamageTextPrefab;
	public Transform DamageTextCanvas;

	/// <summary>
	/// Raised whenever this is dealt non-lethal damage.
	/// </summary>
	public event System.EventHandler Damaged;

	/// <summary>
	/// Raised when this is dealt lethal damage.
	/// </summary>
	public event System.EventHandler Destroyed;

	private float remainingHealth;

	private bool destroyed = false;

	private void Awake()
	{
		remainingHealth = MaxHealth;
	}

	private void OnEnable()
	{
		Damaged += LogDamageTaken;
	}

	private void OnDisable()
	{
		Damaged -= LogDamageTaken;
	}

	/// <summary>
	/// Call this to damage the object.
	/// </summary>
	/// <param name="damage">The amount of damage to deal to this object.</param>
	public void InflictDamage(float damage)
	{
		SpawnDamageText(damage);

		if (!destroyed)
		{
			remainingHealth -= damage;
			if (remainingHealth < 0.5f)
			{
				destroyed = true;
				OnDestroyed(this, null);
			}
			else
			{
				OnDamaged(this, null);
			}
		}
	}

	public void OnDamaged(object sender, System.EventArgs args)
	{
		Damaged?.Invoke(sender, args);
	}

	public void OnDestroyed(object sender, System.EventArgs args)
	{
		Destroyed?.Invoke(sender, args);
	}

	private void SpawnDamageText(float amount)
	{
		var floatingText = Instantiate(DamageTextPrefab, transform.position, Quaternion.identity, DamageTextCanvas).GetComponent<FloatingText>();
		floatingText.Text = amount.ToString();
	}


	private System.EventHandler LogDamageTaken => (sender, e) => Debug.Log($"This ({gameObject.name}) took damage.");

}
