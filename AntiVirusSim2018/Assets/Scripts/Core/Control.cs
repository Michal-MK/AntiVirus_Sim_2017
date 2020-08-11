using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Igor.Constants.Strings;

public class Control : MonoBehaviour {

	public bool allowTesting = false;

	public static Control Instance { get; private set; }

	public SaveManager saveManager;
	public LoadManager loadManager;
	public MapData mapData;

	public static int currAttempt = 0;
	public static int currDifficulty = 0;

	public static event EmptyEventHandler OnEscapePressed;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init() {
		if (!Directory.Exists(Application.dataPath + "/Saves")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves");
		}
		if (!Directory.Exists(Application.dataPath + Path.DirectorySeparatorChar + "Settings")) {
			Directory.CreateDirectory(Application.dataPath + Path.DirectorySeparatorChar + "Settings");
		}
	}

	private void Awake() {
		if (Instance == null) {
			Instance = this;
			Instance.loadManager = new LoadManager();
			DontDestroyOnLoad(gameObject);
			SceneManager.sceneLoaded += OnSceneFinishedLoading;
			gameObject.name = "Active Game Control";
		}
		else if (Instance != this) {
			Destroy(gameObject);
		}
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		currDifficulty = data.core.difficulty;
	}

	public void StartNewGame(int difficulty) {
		SaveManager.SaveNewGame(difficulty);
		CamFadeOut.registerMenuMusicVolumeFade = true;
		CamFadeOut.Instance.PlayTransition(CameraTransitionModes.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += TransitionToNewGame;
		SceneManager.sceneLoaded += NewGameSceneLoaded;
	}

	private void NewGameSceneLoaded(Scene _, LoadSceneMode __) {
		SceneManager.sceneLoaded -= NewGameSceneLoaded;
		Player.GameProgression = 0;
		Timer.ResetTimer();
		Time.timeScale = 1;
		CamFadeOut.Instance.Animator.speed = 0.75f;
	}

	private void TransitionToNewGame() {
		CamFadeOut.OnCamFullyFaded -= TransitionToNewGame;
		Destroy(MenuMusic.script.gameObject);
		SceneManager.LoadScene(SceneNames.GAME1_SCENE);
	}

	public void Restart() {
		CamFadeOut.registerGameMusicVolumeFade = true;
		CamFadeOut.Instance.PlayTransition(CameraTransitionModes.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += RestartTransition;
	}

	private void RestartTransition() {
		PlayerMovement.CanMove = false;
		Player.GameProgression = 0;
		Timer.ResetTimer();
		Time.timeScale = 1;
		CamFadeOut.OnCamFullyFaded -= RestartTransition;
		SceneManager.LoadScene(SceneNames.GAME1_SCENE);
	}

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode _) {
		if (scene.name == SceneNames.GAME1_SCENE) {
			CamFadeOut.Instance.Animator.speed = 0.5f;
		}
		else if (scene.name == SceneNames.MENU_SCENE) {
			if (!MenuMusic.script.isPlaying) {
				MenuMusic.script.PlayMusic();
			}
		}
		Time.timeScale = 1;
		WindowManager.ClearWindows();
	}

	private void Update() {
		if (Input.GetButtonDown(InputNames.ESCAPE)) {
			OnEscapePressed?.Invoke();
		}
	}

	public static void PressingEscape() {
		OnEscapePressed?.Invoke();
	}

	private void OnDestroy() {
		print("Destroyed instance " + gameObject.name);
		SceneManager.sceneLoaded -= OnSceneFinishedLoading;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}

/* List of Static variables
 * Control: ., currAttempt, currDifficulty, OnEscapePressed
  BossBehaviour: OnBossfightBegin, OnBossfightResult
  Coins: OnNewTarget
  Spike: OnNewTarget, $_spikesCollected
  CameraMovement: .
  CamFadeOut: ., OnCamFullyFaded, registerGameMusicVolumeFade, registerMenuMusicVolumeFade
  HUDisplay: .
  UIControlScheme: .
  GameSettings: ., AudioVolume, FXVolume, $path, $fileContents
  MapData: .
  PauseUnpause: canPause, isPaused, OnPaused
  LoadManager: OnSaveDataLoaded
  SaveManager: current, $_canSave
  SaveGameHelper: .
  Zoom: $_canZoom
  Timer: ., getTime, timeFlowMultiplier 
  SignPost: OnAvoidanceBegin, $readPosts
  Player: ., PlayerState, EVENTS{5}, $gameProgression
  PlayerMovement: canMove, playerMovementSpeedMultiplier
  PlayerAttack: OnAmmoChanged, OnAmmoPickup
  BlockScript: pressurePlateTriggered
  Maze: getMazeSpeedMultiplier
  MazeEntrance: OnMazeEnter
  MazeEscape: OnMazeEscape
  PressurePlate: OnNewTarget
  MenuMusic: .
  MusicHandler: .
  SoundFXHandler: .
  Notifications: NotificationActive, $PREFABS{3}, $canvas
  WindowManager: OnWindowOpen, OnWindowClose, $activeWindows
 */
