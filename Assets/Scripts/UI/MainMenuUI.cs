using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour 
{

	public void Play()
	{
		GameManager.Instance.StartNewGame();
	}

	public void PlaySkipStart()
	{
		GameManager.Instance.StartNewGame();
	}

}
