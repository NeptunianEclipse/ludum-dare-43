using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : Singleton<Tooltip>
{


	public Text Text;
	public GameObject Body;

	public string DisplayString {
		get { return Text.text; }
		set { Text.text = value; }
	}

	public bool Visible {
		get { return Body.activeSelf; }
		set { Body.SetActive(value); }
	}
}
