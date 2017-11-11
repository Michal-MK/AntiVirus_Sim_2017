using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour {

	private static Toggle saveToggle;

	public PressurePlate pPlate;
	public Avoidance avoidance;
	public BlockScript block;
	public BossBehaviour boss;
	public Transform collectibles;

	private static bool _canSave = true;

	public static SaveFile current;

	private void Awake() {
		BossBehaviour.OnBossfightBegin += BossBehaviour_OnBossfightBegin;
	}

	private void Start() {
		saveToggle = GameObject.Find("saveGame").GetComponent<Toggle>();
	}

	private void BossBehaviour_OnBossfightBegin(BossBehaviour sender) {
		boss = sender;
	}

	public static void SaveNewGame(int difficulty) {
		FileStream file;
		BinaryFormatter formatter = new BinaryFormatter();

		file = File.Create(Application.dataPath + "/Saves/D" + difficulty + "/Save-D" + difficulty + ".Kappa");
		string imgPath = Application.dataPath + Path.DirectorySeparatorChar + "Saves" + Path.DirectorySeparatorChar + "D" + difficulty + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + "Save-D" + difficulty + ".png";
		SaveFile newSaveFile = new SaveFile {
			saveHistory = new SaveHistory(),
			data = new SaveData()
		};


		newSaveFile.data.core.time = 0;
		newSaveFile.data.core.difficulty = difficulty;
		newSaveFile.data.core.localAttempt = 0;

		try {
			File.Copy(Application.streamingAssetsPath + Path.DirectorySeparatorChar + "NewGame.png", imgPath);
		}
		catch (IOException) {
			if (File.Exists(imgPath)) {
				print("File Exists already");
			}
		}

		formatter.Serialize(file, newSaveFile);
		file.Close();
		file.Dispose();

		current = newSaveFile;
	}

	public void Save(int difficulty, bool newSaveFile = false) {
		BinaryFormatter formatter = new BinaryFormatter();
		SaveFile saveFile;
		FileStream file;

		file = File.Open(Application.dataPath + "/Saves/D" + difficulty + "/Save-D" + difficulty + ".Kappa", FileMode.Open);
		saveFile = current;

		GameProgression.script.GetValues();

		#region Player data
		saveFile.data.player.bombs = PlayerAttack.bombs;
		saveFile.data.player.bullets = PlayerAttack.bullets;
		saveFile.data.player.playerPos = GameProgression.script.playerPos;
		saveFile.data.player.spikesCollected = Spike.spikesCollected;
		saveFile.data.player.coinsCollected = Coins.coinsCollected;
		saveFile.data.player.canZoom = Zoom.canZoom;
		saveFile.data.player.currentBGName = M_Player.currentBG_name;
		#endregion

		#region World data
		saveFile.data.world.blockPos = GameProgression.script.boxPos;
		saveFile.data.world.blockZRotation = GameProgression.script.ZRotationBlock;
		saveFile.data.world.blockPushAttempt = pPlate.attempts;
		saveFile.data.world.spikeActive = collectibles.Find("Spike").gameObject.activeSelf;
		saveFile.data.world.spikePos = GameProgression.script.spikePos;
		saveFile.data.world.pressurePlateTriggered = pPlate.alreadyTriggered;
		saveFile.data.world.doneAvoidance = avoidance.performed;
		if (boss != null) {
			saveFile.data.world.bossSpawned = true;
		}
		else {
			saveFile.data.world.bossSpawned = false;
		}
		if (Maze.inMaze == false && Spike.spikesCollected >= 4) {
			saveFile.data.world.postMazeDoorOpen = true;
		}
		else {
			saveFile.data.world.postMazeDoorOpen = false;
		}
		#endregion

		#region Core data
		saveFile.data.core.camSize = Camera.main.orthographicSize;
		saveFile.data.core.difficulty = difficulty;
		saveFile.data.core.time = Timer.getTime;
		saveFile.data.core.localAttempt = Control.currAttempt;
		#endregion

		#region Hints data
		saveFile.data.shownHints.currentlyDisplayedSideInfo = Canvas_Renderer.script.info_S.text;
		saveFile.data.shownHints.shownAttempt = M_Player.player.newGame;
		saveFile.data.shownHints.shownAvoidanceInfo = avoidance.displayAvoidInfo;
		saveFile.data.shownHints.shownBlockInfo = block.showInfo;
		saveFile.data.shownHints.shownShotInfo = M_Player.player.pAttack.displayShootingInfo;
		#endregion

		#region Core data
		saveFile.data.core.time = Timer.getTime;
		saveFile.data.core.difficulty = Control.currDifficulty;
		saveFile.data.core.camSize = Camera.main.orthographicSize;
		#endregion

		formatter.Serialize(file, saveFile);
		file.Close();
		StartCoroutine(ScreenShot(difficulty));
	}

	private IEnumerator ScreenShot(int currAttempt) {
		GameObject saveButton = GameObject.Find("saveGame");
		if (saveButton != null) {
			yield return new WaitUntil(() => !saveButton.activeInHierarchy);
		}
		//print("Captured");
		ScreenCapture.CaptureScreenshot(Application.dataPath + "/Saves/D" + currAttempt + "/Resources/Save-D" + currAttempt + "_" + currAttempt.ToString("000") + ".png");
	}

	private void OnDestroy() {
		BossBehaviour.OnBossfightBegin -= BossBehaviour_OnBossfightBegin;
	}


	public static bool canSave {
		get { return _canSave; }
		set {
			_canSave = value;
			if (value) {
				saveToggle.interactable = true;
			}
			else {
				saveToggle.interactable = false;
			}
		}
	}
}
