using LudumDare43.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.GetComponent<Player>() != null)
		{
			UI.Instance.WinMesssage.gameObject.SetActive(true);
			StartCoroutine(Extensions.InvokeAfter(() => UI.Instance.WinMesssage.gameObject.SetActive(false), 15));
		}
	}

}
