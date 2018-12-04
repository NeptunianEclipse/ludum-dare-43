using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player> 
{

	private void Awake()
	{


		var d = GetComponent<Damageable>();
		d.Damaged += UI.Instance.SetText;
		d.Destroyed += UI.Instance.SetText;
		d.Destroyed += eventnsdlkfjas;
	}

	void eventnsdlkfjas(object sender, System.EventArgs args)
	{
		UI.Instance.Gameover.gameObject.SetActive(true);
	}

}
