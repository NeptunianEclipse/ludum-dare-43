using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public enum GameState
{
	MainMenu,
	IntroCutscene,
	Levels,
	EndCutscene,
	Paused
}

public class GameManager : Singleton<GameManager> 
{
	public SceneReference MainMenuSceneReference; 
	public SceneReference PlayerSceneReference;

	public SceneReference InitialLevelComponentReference;
	public SceneReference SacrificeChamberReference;
	public List<SceneReference> LevelSceneReferences;
	public float LoadDistance;
	public int LevelsInbetweenChambers;

	public bool DoGameSequence;

	[Header("Editor only")]
	public List<SceneReference> LoadOnStart;

	public Scene? MainMenuScene { get; private set; }
	public Scene? PlayerScene { get; private set; }

	private GameState gameState;
	public GameState GameState {
		get { return gameState; }
		set
		{
			gameState = value;
			GameStateChanged?.Invoke(gameState);
		}
	}

	public event Action<GameState> GameStateChanged;

	private List<Level> LoadedLevels = new List<Level>();
	private Dictionary<string, Level> ScenePathLoadedLevels = new Dictionary<string, Level>(); 
	private Transform rightmostConnector;
	private int levelsUntilNextChamber;

	private void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
	}

	private void Start()
	{
		foreach(SceneReference reference in LoadOnStart)
		{
			SceneManager.LoadScene(reference.ScenePath, LoadSceneMode.Additive);
		}

		if(DoGameSequence)
		{
			GameState = GameState.MainMenu;
			SceneManager.LoadScene(MainMenuSceneReference.ScenePath, LoadSceneMode.Additive);
		}
		else
		{
			GameState = GameState.Levels;
		}
	}

	private void Update()
	{
		// We're in game, load levels ahead of us and remove them behind us
		if(GameState == GameState.Levels)
		{
			if(Player.Instance != null)
			{
				var playerPosition = Player.Instance.transform.position;
				var rightmostConnectorPosition = rightmostConnector?.position ?? Vector3.zero;
				var distance = rightmostConnectorPosition.x - playerPosition.x;
				if (distance <= LoadDistance)
				{
					SceneReference nextLevel = NextLevelSceneToLoad();
					LoadLevelScene(nextLevel);
					levelsUntilNextChamber--;
				}
			}
		}
	}

	public void StartNewGame()
	{
		if(MainMenuScene.HasValue)
		{
			SceneManager.UnloadSceneAsync(MainMenuScene.Value);
		}

		SceneManager.LoadScene(InitialLevelComponentReference.ScenePath, LoadSceneMode.Additive);
		SceneManager.LoadScene(PlayerSceneReference.ScenePath, LoadSceneMode.Additive);

		GameState = GameState.Levels;

		levelsUntilNextChamber = LevelsInbetweenChambers;
	}

	public SceneReference NextLevelSceneToLoad()
	{
		if(levelsUntilNextChamber == 0)
		{
			return SacrificeChamberReference;
		}
		else
		{
			return LevelSceneReferences[UnityEngine.Random.Range(0, LevelSceneReferences.Count - 1)];
		}
	}

	public void LoadLevelScene(SceneReference scene)
	{
		SceneManager.LoadScene(scene.ScenePath, LoadSceneMode.Additive);
	}

	public void LevelLoaded(Level level)
	{
		Level rightmostLevel = LoadedLevels.LastOrDefault();
		rightmostConnector = rightmostLevel?.RightConnector;
		Vector3 rightmostConnectorPosition = rightmostConnector?.position ?? Vector3.zero;

		Vector3 leftConnectorRelativeToNewLevelsOrigin = level.LeftConnector.position - level.transform.position;
		level.transform.position = rightmostConnectorPosition - leftConnectorRelativeToNewLevelsOrigin;

		LoadedLevels.Add(level);
		ScenePathLoadedLevels[level.gameObject.scene.path] = level;

		rightmostConnector = level.RightConnector;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if(scene.path == MainMenuSceneReference.ScenePath)
		{
			MainMenuScene = scene;
		}
		else if(scene.path == PlayerSceneReference.ScenePath)
		{
			PlayerScene = scene;
		}
		else
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			foreach (GameObject gameObject in rootGameObjects)
			{
				var level = gameObject.GetComponent<Level>();
				if (level != null && level.Editing == false)
				{
					LevelLoaded(level);
				}
			}
		}

		
	}

	private void OnSceneUnloaded(Scene scene)
	{
		if (scene.path == MainMenuSceneReference.ScenePath)
		{
			MainMenuScene = null;
		}
		else if (scene.path == PlayerSceneReference.ScenePath)
		{
			PlayerScene = null;
		}
		else
		{
			if(ScenePathLoadedLevels.ContainsKey(scene.path))
			{
				Level unloadedLevel = ScenePathLoadedLevels[scene.path];
				LoadedLevels.Remove(unloadedLevel);
				ScenePathLoadedLevels.Remove(scene.path);
			}
		}
	}

	
}
