using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour {

	public Button saveButton;
	private static Button saveButton_static;
	public PressurePlate pPlate;
	public Avoidance avoidance;
	public BlockScript block;
	public Transform collectibles;

	private static bool _canSave = true;

	public static SaveFile current;

	private bool bossSpawned = false;

	private void Awake() {
		BossBehaviour.OnBossfightBegin += BossBehaviour_OnBossfightBegin;
		saveButton_static = saveButton;
	}

	private void Start() {
		Control.script.saveManager = this;
	}

	private void BossBehaviour_OnBossfightBegin(BossBehaviour sender) {
		bossSpawned = true;
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
		SaveFile newSave = current;
		FileStream file;
		string filePath = Application.dataPath + "/Saves/D" + difficulty + "/Save-D" + difficulty + ".Kappa";
		string imgFilePath = Application.dataPath + "/Saves/D" + difficulty + "/Resources/Save-D" + difficulty + "_" + (newSave.saveHistory.saveHistory.Count - 1).ToString("000") + ".png";
		file = File.Open(filePath,FileMode.Open);

		if (current.data.core.time != 0) {
			newSave.saveHistory.saveHistory.Add(DeepCopy(current.data));
		}
		GameProgression.script.GetValues();

		#region Player data
		newSave.data.player.bombs = PlayerAttack.bombs;
		newSave.data.player.bullets = PlayerAttack.bullets;
		newSave.data.player.playerPos = GameProgression.script.playerPos;
		newSave.data.player.spikesCollected = Spike.spikesCollected;
		newSave.data.player.coinsCollected = Coins.coinsCollected;
		newSave.data.player.canZoom = Zoom.canZoom;
		newSave.data.player.currentBGName = M_Player.currentBG_name;
		#endregion

		#region World data
		newSave.data.world.blockPos = GameProgression.script.boxPos;
		newSave.data.world.blockZRotation = GameProgression.script.ZRotationBlock;
		newSave.data.world.blockPushAttempt = pPlate.attempts;
		newSave.data.world.spikeActive = collectibles.Find("Spike").gameObject.activeSelf;
		newSave.data.world.spikePos = GameProgression.script.spikePos;
		newSave.data.world.pressurePlateTriggered = pPlate.alreadyTriggered;
		newSave.data.world.doneAvoidance = avoidance.performed;
		newSave.data.world.bossSpawned = bossSpawned;

		if (Maze.inMaze == false && Spike.spikesCollected >= 4) {
			newSave.data.world.postMazeDoorOpen = true;
		}
		else {
			newSave.data.world.postMazeDoorOpen = false;
		}
		#endregion

		#region Core data
		newSave.data.core.camSize = Camera.main.orthographicSize;
		newSave.data.core.difficulty = difficulty;
		newSave.data.core.time = Timer.getTime;
		newSave.data.core.localAttempt = Control.currAttempt;
		#endregion

		#region Hints data
		newSave.data.shownHints.currentlyDisplayedSideInfo = Canvas_Renderer.script.info_S.text;
		newSave.data.shownHints.shownAttempt = M_Player.player.newGame;
		newSave.data.shownHints.shownAvoidanceInfo = avoidance.displayAvoidInfo;
		newSave.data.shownHints.shownBlockInfo = block.showInfo;
		newSave.data.shownHints.displayShootInfo = M_Player.player.pAttack.displayShootingInfo;
		#endregion

		#region Core data
		newSave.data.core.time = Timer.getTime;
		newSave.data.core.difficulty = Control.currDifficulty;
		newSave.data.core.camSize = Camera.main.orthographicSize;
		newSave.data.core.fileLocation = filePath;
		newSave.data.core.imgFileLocation = imgFilePath;
		#endregion

		formatter.Serialize(file, newSave);
		file.Close();
		StartCoroutine(ScreenShot(imgFilePath));
	}

	private IEnumerator ScreenShot(int difficulty, int currAttempt) {
		if (saveButton != null) {
			yield return new WaitUntil(() => !saveButton.gameObject.activeInHierarchy);
		}
		ScreenCapture.CaptureScreenshot(Application.dataPath + "/Saves/D" + difficulty + "/Resources/Save-D" + difficulty + "_" + currAttempt.ToString("000") + ".png");
	}

	private IEnumerator ScreenShot(string filePath) {
		if (saveButton != null) {
			yield return new WaitUntil(() => !saveButton.gameObject.activeInHierarchy);
		}
		ScreenCapture.CaptureScreenshot(filePath);
	}

	private void OnDestroy() {
		BossBehaviour.OnBossfightBegin -= BossBehaviour_OnBossfightBegin;
	}

	public static T DeepCopy<T>(T other) {
		using (MemoryStream ms = new MemoryStream()) {
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(ms, other);
			ms.Position = 0;
			return (T)formatter.Deserialize(ms);
		}
	}


	public static bool canSave {
		get { return _canSave; }
		set {
			_canSave = value;
			if (value) {
				saveButton_static.interactable = true;
			}
			else {
				saveButton_static.interactable = false;
			}
		}
	}
}
