using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Throwable))]
public class FloopIntoFhroom : MonoBehaviour
{
	public float HeightToAdd = 0f;

	public GameObject FhroomPrefab;

	private Throwable myThrowable;

	void Awake()
	{
		myThrowable = gameObject.GetComponent<Throwable>();

		if (FhroomPrefab == null) Debug.LogError($"A {gameObject.name} doesn't have a prefab set in the {nameof(FloopIntoFhroom)} component.");
	}

	void OnEnable()
	{
		myThrowable.Expired += MyThrowable_Expired;
	}

	void OnDisable()
	{
		myThrowable.Expired -= MyThrowable_Expired;
	}

	private void MyThrowable_Expired(object sender, System.EventArgs e)
	{
		Vector3 position = transform.position.NewWithChange(deltaY: HeightToAdd);
		Quaternion rotation = FhroomPrefab.transform.rotation;
		Instantiate(FhroomPrefab, position, rotation);
	}
}
