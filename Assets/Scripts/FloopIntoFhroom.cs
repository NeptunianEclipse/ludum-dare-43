using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Throwable))]
[RequireComponent(typeof(Growable))]
public class FloopIntoFhroom : MonoBehaviour
{
	public float HeightToAdd = 0f;

	public GameObject FhroomPrefab;

	private Throwable myThrowable;
	private Growable myGrowable;
	
	void Awake()
	{
		myThrowable = gameObject.GetComponent<Throwable>();
		myGrowable = gameObject.GetComponent<Growable>();
		
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

	void Start()
	{
		myGrowable.StartGrow(myThrowable.MaxTimeAlive);
	}

	private void MyThrowable_Expired(object sender, System.EventArgs e)
	{
		Vector3 position = transform.position.NewWithChange(deltaY: HeightToAdd);
		Quaternion rotation = FhroomPrefab.transform.rotation;
		Instantiate(FhroomPrefab, position, rotation);
	}

}
