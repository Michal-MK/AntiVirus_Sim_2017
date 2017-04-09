using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Control : MonoBehaviour {

	public static Control script;
	public int chosenDifficulty;
	private int attempt;

	public GameObject spikeInScene;
	public GameObject authentication;
	public bool isNewGame = true;
	public bool isRestarting = false;

	private SaveData loadedData;


	void Awake() {
		//PlayerPrefs.DeleteKey("player_name");
		if (script == null) {
			script = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (script != this) {
			Destroy(gameObject);
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
		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}

	private void Start() {
		//StartCoroutine(SetName());
	}

	public IEnumerator SetName() {
		if (PlayerPrefs.GetString("player_name") == null || PlayerPrefs.GetString("player_name") == "") {
			yield return new WaitForSeconds(1f);
			GameObject auth = Instantiate(authentication);
			auth.transform.SetParent(GameObject.Find("Canvas").transform);
			auth.transform.localPosition = Vector3.zero;
			print(auth.transform.position);
		}
		//else {
		//	yield return new WaitForEndOfFrame();
		//	if (Statics.profile != null) {
		//		Statics.profile.DisplayProfile();
		//	}
		//}

	}
	public void Save(bool newsaveFile, bool saveOnceInBoss = true) {
		print("Saving");

		if (newsaveFile) {
			attempt++;
			PlayerPrefs.SetInt("A", attempt);
		}

		if (saveOnceInBoss == false && loadedData != null) {
			if (loadedData.bossSpawned) {
				print("Exists");
				return;
			}
		}

		print("Condition met");
		chosenDifficulty = PlayerPrefs.GetInt("difficulty");
		attempt = PlayerPrefs.GetInt("A");

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream file = File.Create(Application.dataPath + "/Saves/D" + chosenDifficulty + "/Save-D" + chosenDifficulty + "_" + attempt.ToString("000") + ".Kappa");
		SaveData data = new SaveData();
		Statics.gameProgression.GetValues();

		data.coinsCollected = Coins.coinsCollected;
		data.spikesCollected = Spike.spikesCollected;
		data.bombs = PlayerAttack.bombs;
		data.bullets = PlayerAttack.bullets;
		print(data.bombs + " " + data.bullets);
		data.playerPositionX = Statics.gameProgression.currentPositionPlayerX;
		data.playerPositionY = Statics.gameProgression.currentPositionPlayerY;
		data.playerPositionZ = Statics.gameProgression.currentPositionPlayerZ;
		data.blockPosX = Statics.gameProgression.currentPositionBoxX;
		data.blockPosY = Statics.gameProgression.currentPositionBoxY;
		data.blockPosZ = Statics.gameProgression.currentPositionBoxZ;
		data.spikePosX = Statics.gameProgression.currentPositionSpikeX;
		data.spikePosY = Statics.gameProgression.currentPositionSpikeY;
		data.spikePosZ = Statics.gameProgression.currentPositionSpikeZ;
		data.blockZRotation = Statics.gameProgression.ZRotationBlock;
		data.difficulty = chosenDifficulty;
		data.currentBGName = M_Player.currentBG_name;
		data.currentlyDisplayedSideInfo = Statics.canvasRenderer.info_S.text;
		data.time = Mathf.Round(timer.time * 1000) / 1000;
		data.shownAttempt = Statics.mPlayer.newGame;
		data.shownShotInfo = Statics.playerAttack.displayShootingInfo;
		data.shownAvoidanceInfo = Statics.avoidance.displayAvoidInfo;
		data.doneAvoidance = Statics.avoidance.preformed;
		data.shownBlockInfo = Statics.blockScript.showInfo;
		data.blockPushAttempt = Statics.pressurePlate.attempts;
		data.camSize = Camera.main.orthographicSize;
		data.canZoom = Statics.zoom.canZoom;
		data.bossSpawned = Statics.cameraMovement.inBossRoom;
		data.pressurePlateTriggered = Statics.pressurePlate.alreadyTriggered;
		data.isNewGame = isNewGame;
		data.isRestarting = isRestarting;
		data.spikeActive = GameObject.Find("Collectibles").GetComponent<Wrapper>().Objects[0].activeSelf;
		data.postMazeDoorOpen = !Statics.mazeEscape.pathOpen;


		formatter.Serialize(file, data);
		file.Close();
		StartCoroutine(ScreenShot(attempt));


	}


	private IEnumerator ScreenShot(int currAttempt) {
		GameObject saveButton = GameObject.Find("saveGame");
		if (saveButton != null) {
			yield return new WaitUntil(() => !saveButton.activeInHierarchy);
		}
		print("Captured");
		Application.CaptureScreenshot(Application.dataPath + "/Saves/D" + chosenDifficulty + "/Resources/Save-D" + chosenDifficulty + "_" + currAttempt.ToString("000") + ".png");
	}

	public void Load(string fileToLoad) {

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(fileToLoad, FileMode.Open);


		SaveData data = (SaveData)bf.Deserialize(file);
		file.Close();

		script.chosenDifficulty = data.difficulty;
		loadedData = data;
		StartCoroutine(FilesLoaded());
	}

	public IEnumerator StartNewGame(int difficulty) {
		PlayerPrefs.SetInt("difficulty", difficulty);
		Statics.camFade.PlayTransition("Trans");
		AsyncOperation loading = SceneManager.LoadSceneAsync(1);
		loading.allowSceneActivation = true;
		yield return new WaitForSeconds(1.5f);
		Statics.camFade.anim.speed = 0;
		yield return new WaitUntil(() => loading.isDone);
		print("DoneLoading");
		Statics.camFade.anim.speed = 1;
	}



	public void Restart() {
		M_Player.doNotMove = false;
		Spike.spikesCollected = 0;
		Coins.coinsCollected = 0;
		PlayerAttack.bombs = 0;
		PlayerAttack.bullets = 0;
		M_Player.gameProgression = 0;
		Projectile.projectileSpeed = 15;
		Time.timeScale = 1;
		script.isRestarting = true;
		print(script.isRestarting);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Statics.camFade.anim.SetTrigger("UnDim");
	}

	private IEnumerator FilesLoaded() {
		Statics.camFade.PlayTransition("Trans");
		AsyncOperation loading = SceneManager.LoadSceneAsync(1);
		isNewGame = false;
		loading.allowSceneActivation = true;
		yield return new WaitForSeconds(1.5f);
		Statics.camFade.anim.speed = 0;
		yield return new WaitUntil(() => loading.isDone);
		Statics.camFade.anim.speed = 1;

	}


	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
		if(scene.buildIndex == 0 && PlayerPrefs.GetString("player_name") == null) {
			SetName();
		}

		if (scene.buildIndex == 1 && isNewGame) {
			Statics.mPlayer.newGame = true;
		}
		else if (isRestarting) {
			isNewGame = false;
		}

		else if (scene.buildIndex == 1 && !isNewGame) {
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			GameObject block = GameObject.Find("Block");
			Wrapper wrp = GameObject.Find("Collectibles").GetComponent<Wrapper>();

			M_Player.gameProgression = loadedData.spikesCollected;

			Vector3 playerPos = new Vector3(loadedData.playerPositionX, loadedData.playerPositionY, loadedData.playerPositionZ);
			player.transform.position = playerPos;

			Vector3 spikePos = new Vector3(loadedData.spikePosX, loadedData.spikePosY, loadedData.spikePosZ);

			wrp.Objects[0].gameObject.SetActive(loadedData.spikeActive);
			if (wrp.Objects[0].activeInHierarchy == true) {
				wrp.Objects[0].transform.position = spikePos;
			}

			Coins.coinsCollected = loadedData.coinsCollected;
			Spike.spikesCollected = loadedData.spikesCollected;
			Statics.canvasRenderer.Counters("Update");


			PlayerAttack.bullets = loadedData.bullets;
			PlayerAttack.bombs = loadedData.bombs;


			Statics.playerAttack.displayShootingInfo = loadedData.shownShotInfo;

			Statics.blockScript.showInfo = loadedData.shownBlockInfo;
			block.transform.position = new Vector3(loadedData.blockPosX, loadedData.blockPosY, loadedData.blockPosZ);
			block.transform.rotation = Quaternion.AngleAxis(loadedData.blockZRotation, Vector3.back);
			if (loadedData.blockPushAttempt == 3) {
				Statics.pressurePlate.CreateBarrier();
			}
			else {
				Statics.pressurePlate.attempts = loadedData.blockPushAttempt;
			}

			if (!loadedData.shownShotInfo) {
				Statics.playerAttack.StartCoroutine(Statics.playerAttack.UpdateStats());
			}
			if (loadedData.coinsCollected == 5) {
				Statics.coins.ChatchUpToAttempt(loadedData.coinsCollected - 2);
				GameObject.Find("Coin").SetActive(false);
				Statics.coins.CoinBehavior();
			}
			else if (loadedData.coinsCollected <= 4) {
				Statics.coins.ChatchUpToAttempt(loadedData.coinsCollected - 2);
				Statics.coins.CoinBehavior();
				Statics.guide.Recalculate(GameObject.Find("Coin"), true);
			}


			PlayerPrefs.SetInt("difficulty", loadedData.difficulty);



			Statics.avoidance.displayAvoidInfo = loadedData.shownAvoidanceInfo;
			Statics.avoidance.preformed = loadedData.doneAvoidance;
			if (loadedData.doneAvoidance) {
				GameObject.Find("SignPost Avoidance").SetActive(false);
				Statics.music.PlayMusic(Statics.music.room1);
			}


			timer.time = loadedData.time;
			timer.run = true;

			Camera.main.orthographicSize = loadedData.camSize;
			Statics.zoom.canZoom = loadedData.canZoom;

			Statics.cameraMovement.inBossRoom = loadedData.bossSpawned;
			if (loadedData.bossSpawned) {
				RectTransform bg = GameObject.Find("Background_room_Boss_1").GetComponent<RectTransform>();
				Camera.main.transform.position = bg.position;
				Statics.cameraMovement.psA.transform.position = bg.position + new Vector3(0, bg.sizeDelta.y / 2, 0);
				ParticleSystem.ShapeModule shape = Statics.cameraMovement.psA.shape;
				shape.radius = 108 * 2;
				Statics.cameraMovement.psB.gameObject.SetActive(false);
				M_Player.gameProgression = 10;
				Statics.bossEntrance.SpawnBossOnLoad();

			}

			Statics.canvasRenderer.infoRenderer(null, loadedData.currentlyDisplayedSideInfo);
			Statics.pressurePlate.alreadyTriggered = loadedData.pressurePlateTriggered;

			GameObject.Find("Blocker3").SetActive(loadedData.postMazeDoorOpen);

			isNewGame = loadedData.isNewGame;
			isRestarting = loadedData.isRestarting;



			Statics.mPlayer.newGame = false;
			switch (loadedData.currentBGName) {
				case "Background_Start": {
					Statics.music.PlayMusic(Statics.music.room1);
					break;
				}
				case "Background_room_2b": {
					Statics.music.PlayMusic(Statics.music.room1);
					break;
				}
				case "Background_room_Boss_1": {
					Statics.music.PlayMusic(Statics.music.boss);
					break;
				}
				case "MazeBG": {
					Statics.music.PlayMusic(Statics.music.maze);
					break;
				}
				default: {
					break;
				}
			}
		}
	}

	private void OnDestroy() {
		SceneManager.sceneLoaded -= OnSceneFinishedLoading;
	}
}

//Data to be saved
[Serializable]
public class SaveData {


	public int coinsCollected;
	public int spikesCollected;
	public int bullets;
	public int bombs;

	public float playerPositionX, playerPositionY, playerPositionZ;
	public float blockPosX, blockPosY, blockPosZ;
	public float spikePosX, spikePosY, spikePosZ;
	public float blockZRotation;
	public bool spikeActive;

	public int difficulty;
	public float time;
	public bool canZoom;

	public string currentBGName;
	public string currentlyDisplayedSideInfo;

	public bool shownShotInfo;
	public bool shownAttempt;
	public bool shownBlockInfo;
	public int blockPushAttempt;
	public bool pressurePlateTriggered;
	public bool postMazeDoorOpen;

	public float camSize;

	public bool isNewGame;
	public bool isRestarting;


	public bool shownAvoidanceInfo;
	public bool doneAvoidance;
	public bool bossSpawned;

}

//Static reference to other classes
public class Statics : MonoBehaviour {

	public static BossBehaviour bossBehaviour;
	public static BossEntrance bossEntrance;
	public static BossHealth bossHealth;


	public static SwitchScene switchScene;


	public static Coins coins;
	public static Guide guide;
	public static Spike spike;


	public static CameraMovement cameraMovement;
	public static Canvas_Renderer canvasRenderer;
	public static GameProgression gameProgression;
	public static PauseUnpause pauseUnpause;
	public static SaveGame saveGame;
	public static timer timerScript;
	public static Wrapper wrapper;
	public static Zoom zoom;


	public static EnemySpawner enemySpawner;
	public static Projectile projectile;
	public static TurretAttack turretAttack;


	public static Maze mazeScript;
	public static MazeEntrance mazeEntrance;
	public static MazeEscape mazeEscape;


	public static M_Player mPlayer;
	public static PlayerAttack playerAttack;
	public static SpikeBullet spikeBullet;
	public static PlayerParticles playerParticles;


	public static Avoidance avoidance;
	public static BlockScript blockScript;
	public static PressurePlate pressurePlate;


	public static CamFadeOut camFade;
	public static MusicHandler music;
	public static SoundFXHandler sound;

	public static DisplaySaveFiles displaySaves;
	public static ProfileName profile;

}

