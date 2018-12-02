using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetCollisions : MonoBehaviour 
{

	public event Action<Collider2D> TriggerEnter;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		TriggerEnter?.Invoke(collision);
	}


}
