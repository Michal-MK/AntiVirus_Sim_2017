using System;
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


	#region SaveModifiers
	private Vector3 automaticSave_PreBoss1;
	#endregion

	private void Awake() {
		BossBehaviour.OnBossfightBegin += BossBehaviour_OnBossfightBegin;
		saveButton_static = saveButton;

		automaticSave_PreBoss1 = new Vector3(302, -130);
	}

	private void Start() {
		Control.script.saveManager = this;
	}

	private void BossBehaviour_OnBossfightBegin(BossBehaviour sender) {
		bossSpawned = true;
	}

	public static void SaveNewGame(int difficulty) {
		BinaryFormatter formatter = new BinaryFormatter();
		string folderName = DateTime.Now.ToLongTimeString().Replace(':', '-');
		folderName = folderName.Remove(folderName.Length - 3, 3);
		folderName = folderName + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
		DirectoryInfo newSaveDir = Directory.CreateDirectory(Application.dataPath + Path.DirectorySeparatorChar + "Saves" + Path.DirectorySeparatorChar + "D" + difficulty + " " + folderName);

		string filePath = newSaveDir.FullName + Path.DirectorySeparatorChar.ToString() + "data.Kappa";
		string imgPath = newSaveDir.FullName + Path.DirectorySeparatorChar.ToString() + "00.png";

		using (FileStream file = File.Create(filePath)) {

			SaveFile newSaveFile = new SaveFile {
				saveHistory = new SaveHistory(),
				data = new SaveData()
			};

			newSaveFile.data.core.time = 0;
			newSaveFile.data.core.difficulty = difficulty;
			newSaveFile.data.core.localAttempt = 0;
			newSaveFile.data.core.fileLocation = filePath;
			newSaveFile.data.core.imgFileLocation = imgPath;
			newSaveFile.data.world.spikePos = new SaveData_Helper.SVector3(-100, -60, 0);
			newSaveFile.data.world.blockPos = new SaveData_Helper.SVector3(-80, 0, 0);
			newSaveFile.data.player.playerPos = new SaveData_Helper.SVector3(0, 0, 0);
			newSaveFile.data.core.camSize = 15f;


			File.Copy(Application.streamingAssetsPath + Path.DirectorySeparatorChar + "NewGame.png", imgPath);

			formatter.Serialize(file, newSaveFile);
			current = newSaveFile;
		}
	}

	public void Save(int difficulty, bool isAutomatic) {
		BinaryFormatter formatter = new BinaryFormatter();
		SaveFile newSave = current;

		if (current.data.core.time != 0) {
			newSave.saveHistory.previousSaves.Add(DeepCopy(current.data));
		}

		string filePath = newSave.data.core.fileLocation;
		string imgFilePath = newSave.data.core.fileLocation.Remove(filePath.Length-10,10) + "0" + (newSave.saveHistory.previousSaves.Count + 1).ToString() + ".png";

		using (FileStream file = File.Open(filePath, FileMode.Open)) {
			SaveGameHelper.script.GetValues();

			#region Player data
			newSave.data.player.bombs = M_Player.player.pAttack.bombs;
			newSave.data.player.bullets = M_Player.player.pAttack.bullets;
			newSave.data.player.playerPos = isAutomatic && M_Player.player.GetCurrentBackground() == MapData.script.GetBackground(4) ? automaticSave_PreBoss1 : SaveGameHelper.script.playerPos;
			newSave.data.player.spikesCollected = Spike.spikesCollected;
			newSave.data.player.coinsCollected = Coin.coinsCollected;
			newSave.data.player.canZoom = Zoom.canZoom;
			newSave.data.player.currentBGName = M_Player.player.GetCurrentBackground().name;
			newSave.data.player.gameProgression = M_Player.gameProgression;
			#endregion

			#region World data
			newSave.data.world.blockPos = SaveGameHelper.script.boxPos;
			newSave.data.world.blockZRotation = SaveGameHelper.script.ZRotationBlock;
			newSave.data.world.blockPushAttempt = pPlate.attempts;
			newSave.data.world.spikeActive = collectibles.Find("Spike").gameObject.activeSelf;
			newSave.data.world.spikePos = SaveGameHelper.script.spikePos;
			newSave.data.world.pressurePlateTriggered = pPlate.alreadyTriggered;
			newSave.data.world.doneAvoidance = avoidance.performed;
			newSave.data.world.boss1Killed = MapData.script.isBoss1Killed;
			newSave.data.world.postMazeDoorOpen = CameraMovement.script.inMaze == false && Spike.spikesCollected >= 4 ? true : false;
			newSave.data.world.doorsOpen = new System.Collections.Generic.List<string>();
			for (int i = 0; i < MapData.script.allDoors.Length; i++) {
				if (MapData.script.allDoors[i].isDoorOpen) {
					newSave.data.world.doorsOpen.Add(MapData.script.allDoors[i].getRoomIndicies.From + "," + MapData.script.allDoors[i].getRoomIndicies.To);
				}
			}
			#endregion

			#region Hints data
			newSave.data.shownHints.currentlyDisplayedSideInfo = Canvas_Renderer.script.info_S.text;
			newSave.data.shownHints.shownAttempt = M_Player.player.newGame;
			newSave.data.shownHints.shownAvoidanceInfo = avoidance.displayAvoidInfo;
			newSave.data.shownHints.shownBlockInfo = block.showInfo;
			newSave.data.shownHints.displayShootInfo = M_Player.player.pAttack.displayShootingInfo;
			newSave.data.shownHints.shownDirectionsAfterSpikePickup = Spike.spikesCollected >= 1 ? true : false;
			#endregion

			#region Core data
			newSave.data.core.time = Timer.getTime;
			newSave.data.core.difficulty = Control.currDifficulty;
			newSave.data.core.localAttempt = Control.currAttempt;
			newSave.data.core.camSize = Camera.main.orthographicSize;
			newSave.data.core.fileLocation = filePath;
			newSave.data.core.imgFileLocation = imgFilePath;
			#endregion

			formatter.Serialize(file, newSave);
			StartCoroutine(ScreenShot(imgFilePath));
		}
	}

	private IEnumerator ScreenShot(string imgFilePath) {
		if (saveButton != null) {
			yield return new WaitUntil(() => !saveButton.gameObject.activeInHierarchy);
		}
		ScreenCapture.CaptureScreenshot(imgFilePath);
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
