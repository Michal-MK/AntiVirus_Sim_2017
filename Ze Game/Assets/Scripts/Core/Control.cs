using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using Igor.Constants.Strings;

public class Control : MonoBehaviour {

	public static Control script;

	public SaveManager saveManager;
	public LoadManager loadManager;
	public MapData mapData;

	public static int currAttempt = 0;
	public static int currDifficulty = 0;
	public static Profile_Data currProfile;

	public delegate void Escape();
	public static event Escape OnEscapePressed;

	void Awake() {
		if (script == null) {
			script = this;
			loadManager = new LoadManager();
			DontDestroyOnLoad(gameObject);
		}
		else if (script != this) {
			if (script.loadManager == null) {
				print("Somehow we lost a reference to the load manager...");
				script.loadManager = new LoadManager();
			}
			Destroy(gameObject);
			LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		}

		if (!Directory.Exists(Application.dataPath + "/Saves")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D0")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D0");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D1")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D1");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D2")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D2");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D3")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D3");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D4")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D4");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D0/Resources")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D0/Resources");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D1/Resources")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D1/Resources");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D2/Resources")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D2/Resources");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D3/Resources")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D3/Resources");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D4/Resources")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D4/Resources");
		}
		if (!Directory.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "Profiles")) {
			Directory.CreateDirectory(Application.persistentDataPath + Path.DirectorySeparatorChar + "Profiles");
		}
		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		currAttempt = data.core.localAttempt;
		currDifficulty = data.core.difficulty;
	}

	public void StartNewGame(int difficulty) {
		SaveManager.SaveNewGame(difficulty);
		MenuMusic.script.StopMusic();
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += TransitionToNewGame;
		SceneManager.sceneLoaded += NewGameSceneLoaded;
	}

	private void NewGameSceneLoaded(Scene arg0, LoadSceneMode arg1) {
		SceneManager.sceneLoaded -= NewGameSceneLoaded;
		Spike.spikesCollected = 0;
		Coins.coinsCollected = 0;
		PlayerAttack.bombs = 0;
		PlayerAttack.bullets = 0;
		M_Player.gameProgression = 0;
		Projectile.projectileSpeed = 15;
		Timer.ResetTimer();
		Time.timeScale = 1;
		CamFadeOut.script.anim.speed = 0.75f;
	}

	private void TransitionToNewGame() {
		CamFadeOut.OnCamFullyFaded -= TransitionToNewGame;
		SceneManager.LoadScene(SceneNames.GAME1_SCENE);
	}

	public void Restart() {
		MusicHandler.script.FadeMusic();
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += RestartTransition;
	}

	private void RestartTransition() {
		Player_Movement.canMove = false;
		Spike.spikesCollected = 0;
		Coins.coinsCollected = 0;
		PlayerAttack.bombs = 0;
		PlayerAttack.bullets = 0;
		M_Player.gameProgression = 0;
		Projectile.projectileSpeed = 15;
		Timer.ResetTimer();
		Time.timeScale = 1;
		CamFadeOut.OnCamFullyFaded -= RestartTransition;
		SceneManager.LoadScene(SceneNames.GAME1_SCENE);
	}

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
		if (scene.name == SceneNames.GAME1_SCENE) {
			UserInterface.sceneMode = UserInterface.UIScene.GAME;
			CamFadeOut.script.anim.speed = 0.5f;
			//if (MusicHandler.script.isAnythingPlaying == false) {
				//MusicHandler.script.PlayMusic(MusicHandler.script.room1_1);
			//}
		}
		else if(scene.name == SceneNames.MENU_SCENE) {
			UserInterface.sceneMode = UserInterface.UIScene.MAIN_MENU;
			if (!MenuMusic.script.isPlaying) {
				MenuMusic.script.PlayMusic();
			}
		}
		else if(scene.name == SceneNames.SAVES_SCENE) {
			UserInterface.sceneMode = UserInterface.UIScene.SAVES;
		}
		else{
			UserInterface.sceneMode = UserInterface.UIScene.OTHER;
		}

		Time.timeScale = 1;
		WindowManager.ClearWindows();
	}

	private void Update() {
		if (Input.GetButtonDown("Escape")) {
			if(OnEscapePressed != null) {
				OnEscapePressed();
			}
		}
	}

	public static void PressingEscape() {
		if (OnEscapePressed != null) {
			OnEscapePressed();
		}
	}

	private void OnDestroy() {
		SceneManager.sceneLoaded -= OnSceneFinishedLoading;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}