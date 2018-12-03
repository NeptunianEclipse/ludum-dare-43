using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> 
{
	public Scene MainMenuScene;

	public Scene InitialLevelComponent;
	public Scene SacrificeChamber;
	public List<Scene> LevelScenes;
	public float LoadDistance;

	private List<Level> LoadedLevels;

	private void Awake()
	{
		
	}

	private void Start()
	{
		SceneManager.LoadScene(MainMenuScene.name, LoadSceneMode.Additive);
	}

	public void StartNewGame()
	{

	}

	public void LoadLevelScene(Scene scene)
	{
		SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
	}

	public void LevelFinishedLoading(Level level)
	{
		LoadedLevels.Add(level);
	}

	
}
