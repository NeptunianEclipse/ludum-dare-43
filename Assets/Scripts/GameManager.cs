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

[System.Serializable]
public class Difficulty
{
	public List<SceneReference> Levels;
}

public class GameManager : Singleton<GameManager> 
{
	public SceneReference MainMenuSceneReference; 
	public SceneReference PlayerSceneReference;

	public SceneReference InitialLevelComponentReference;
	public SceneReference FinalLevel;
	public SceneReference SacrificeChamberReference;
	public List<SceneReference> LevelSceneReferences;

	public List<Difficulty> LevelsByDifficulty;

	public float LoadDistance;
	public int LevelsInbetweenChambers;
	public int ChambersPerDifficulty;

	public bool DoGameSequence;
	public bool DebugSpawnPlayer;

	public int Difficulty = 0;

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
	private int chambersHadThisDifficulty;

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
			if(DebugSpawnPlayer)
			{
				SceneManager.LoadScene(PlayerSceneReference.ScenePath, LoadSceneMode.Additive);
			}
		}
	}

	private void Update()
	{
		// We're in game, load levels ahead of us and remove them behind us
		if(GameState == GameState.Levels && !done)
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
		UI.Instance.StoryDialogue.SetActive(true);
		if(MainMenuScene.HasValue)
		{
			SceneManager.UnloadSceneAsync(MainMenuScene.Value);
		}

		SceneManager.LoadScene(InitialLevelComponentReference.ScenePath, LoadSceneMode.Additive);
		SceneManager.LoadScene(PlayerSceneReference.ScenePath, LoadSceneMode.Additive);

		GameState = GameState.Levels;

		levelsUntilNextChamber = LevelsInbetweenChambers;
	}

	bool done;

	public SceneReference NextLevelSceneToLoad()
	{
		if(levelsUntilNextChamber == 0)
		{
			levelsUntilNextChamber = LevelsInbetweenChambers;
			chambersHadThisDifficulty++;
			if(chambersHadThisDifficulty >= ChambersPerDifficulty)
			{
				chambersHadThisDifficulty = 0;
				Difficulty++;
				if(Difficulty >= LevelsByDifficulty.Count)
				{
					done = true;
					return FinalLevel;
				}
			}
			return SacrificeChamberReference;
		}
		else
		{
			return LevelsByDifficulty[Difficulty].Levels[UnityEngine.Random.Range(0, LevelsByDifficulty[Difficulty].Levels.Count)];
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
