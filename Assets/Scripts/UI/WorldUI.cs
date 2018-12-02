using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class WorldUI : Singleton<WorldUI>
{
	public float MinDamage;
	public float MaxDamage;
	public Gradient DamageTextGradient;
	public GameObject DamageTextPrefab;

	private Canvas canvas;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
	}

	public void SpawnDamageText(float amount, Vector2 position)
	{
		var floatingText = Instantiate(DamageTextPrefab, position, Quaternion.identity, transform).GetComponent<FloatingTextUI>();
		floatingText.Text = amount.ToString();
		floatingText.TextObject.color = DamageTextGradient.Evaluate(Mathf.Clamp01(Mathf.InverseLerp(MinDamage, MaxDamage, amount)));

		Destroy(floatingText.gameObject, 10);
	}
}
