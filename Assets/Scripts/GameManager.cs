using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public Scene MainMenuScene;

	private void Awake()
	{
		
	}

	private void Start()
	{
		SceneManager.LoadScene(MainMenuScene.name);
	}
}
