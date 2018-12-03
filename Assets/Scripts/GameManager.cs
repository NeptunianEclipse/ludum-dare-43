using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public Scene MainMenuScene;

	public List<Scene> LevelComponents;

	private List<Scene> LoadedLevelComponents;

	private void Awake()
	{
		
	}

	private void Start()
	{
		SceneManager.LoadScene(MainMenuScene.name);
	}

	public void StartNewGame()
	{

	}

	//public void 
}
