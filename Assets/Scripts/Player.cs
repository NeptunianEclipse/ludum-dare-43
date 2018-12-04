using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player> 
{

	private void Awake()
	{
		GetComponent<Damageable>().Damaged += UI.Instance.SetText;
	}

}
