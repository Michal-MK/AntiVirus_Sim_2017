using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using Igor.Constants.Strings;

public class Control : MonoBehaviour {

	public bool allowTesting = false;

	public static Control script;
	public SaveManager saveManager;
	public LoadManager loadManager;
	public MapData mapData;

	public static int currAttempt = 0;
	public static int currDifficulty = 0;

	public delegate void Escape();
	public static event Escape OnEscapePressed;

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
		if (script == null) {
			script = this;
			script.loadManager = new LoadManager();
			DontDestroyOnLoad(gameObject);
			SceneManager.sceneLoaded += OnSceneFinishedLoading;
			gameObject.name = "Active Game Control";
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		currDifficulty = data.core.difficulty;
	}

	public void StartNewGame(int difficulty) {
		SaveManager.SaveNewGame(difficulty);
		CamFadeOut.registerMenuMusicVolumeFade = true;
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += TransitionToNewGame;
		SceneManager.sceneLoaded += NewGameSceneLoaded;
	}

	private void NewGameSceneLoaded(Scene arg0, LoadSceneMode arg1) {
		SceneManager.sceneLoaded -= NewGameSceneLoaded;
		Spike.spikesCollected = 0;
		Coin.coinsCollected = 0;
		M_Player.gameProgression = 0;
		Timer.ResetTimer();
		Time.timeScale = 1;
		CamFadeOut.script.anim.speed = 0.75f;
	}

	private void TransitionToNewGame() {
		CamFadeOut.OnCamFullyFaded -= TransitionToNewGame;
		Destroy(MenuMusic.script.gameObject);
		SceneManager.LoadScene(SceneNames.GAME1_SCENE);
	}

	public void Restart() {
		CamFadeOut.registerGameMusicVolumeFade = true;
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += RestartTransition;
	}

	private void RestartTransition() {
		Player_Movement.canMove = false;
		Spike.spikesCollected = 0;
		Coin.coinsCollected = 0;
		M_Player.gameProgression = 0;
		Timer.ResetTimer();
		Time.timeScale = 1;
		CamFadeOut.OnCamFullyFaded -= RestartTransition;
		SceneManager.LoadScene(SceneNames.GAME1_SCENE);
	}

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
		if (scene.name == SceneNames.GAME1_SCENE) {
			CamFadeOut.script.anim.speed = 0.5f;
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
		if (Input.GetButtonDown("Escape")) {
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
 * Control: currAttempt, currDifficulty, currProfile, OnEscapePressed Control
 * BossBehaviour: playerSpeedMultiplier, OnBossfightBegin
 * Coins: coinsCollected, OnNewTarget
 * Spike: spikesCollected, OnNewTarget
 * CameraMovement: OnZoomModeSwitch, CameraMovement
 * CamFadeOut: CamFadeOut, OnCamFullyFaded, registerGameMusicVolumeFade, registerMenuMusicVolumeFade
 * CanvasRenderer: CanvasRenderer
 * MapData: MapData
 * MusicHandler: MusicHandler
 * PauseUnpause: canPause, isPaused
 * LoadManager: OnSaveDataLoaded
 * SaveManager: saveButton, canSave, current,
 * SaveGameHelper: SaveGameHelper
 * SoundFXHandler: SoundFXHandler
 * Timer: time, isRunning, timeFlowMultiplier, OnTimerPause, Timer, 
 * Zoom: canZoom
 * SignPost: OnAvoidanceBegin
 * M_Player: gameProgression, currentBG_name, M_Player, playerState, EVENTS
 * Player_Movement: canMove
 * PlayerAttack: OnAmmoChanged, OnAmmoPickup
 * Profile: profilesFolder, currProfile, profileRepresentation, authentication, profileName, 
 * BlockScript: pressurePlateTriggered
 * Maze: mazeSpeedMultiplier
 * MazeEntrance: OnMazeEnter
 * MazeEscape: OnMazeEscape
 * PressurePlate: OnNewTarget
 * MenuMusic: MenuMusic
 * HUDElements: HudElements
 * Notifications: all the prefabs, canvas
 * UserInterface: OnPauseChange, sceneMode, 
 * WindowManager: activeWindows
 * UICallbacks: source(private)
 */
