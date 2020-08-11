using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour {

	public Button saveButton;

	public PressurePlate pPlate;
	public Avoidance avoidance;
	public BlockScript block;
	public Transform collectibles;

	private static bool _canSave = true;

	public static SaveFile current;

	#region SaveModifiers
	private Vector3 automaticSave_PreBoss1;
	#endregion

	private void Awake() {
		automaticSave_PreBoss1 = new Vector3(302, -130);
	}

	private void Start() {
		Control.Instance.saveManager = this;
	}


	public static void SaveNewGame(int difficulty) {
		BinaryFormatter formatter = new BinaryFormatter();
		string folderName = DateTime.Now.ToLongTimeString().Replace(':', '-');
		folderName = folderName.Remove(folderName.Length - 3, 3);
		folderName = folderName + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
		DirectoryInfo newSaveDir = Directory.CreateDirectory(Application.dataPath + Path.DirectorySeparatorChar + "Saves" + Path.DirectorySeparatorChar + "D" + difficulty + " " + folderName);

		string filePath = newSaveDir.FullName + Path.DirectorySeparatorChar.ToString() + "data.Kappa"; //This was the moment I realized that file extensions are irrelevant ;) What a time that was.
		string imgPath = newSaveDir.FullName + Path.DirectorySeparatorChar.ToString() + "00.png";

		using (FileStream file = File.Create(filePath)) {

			SaveFile newSaveFile = new SaveFile {
				saveHistory = new SaveHistory(),
				data = new SaveData()
			};

			newSaveFile.data.core.time = 0;
			newSaveFile.data.core.difficulty = difficulty;
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

	public void Save(bool isAutomatic) {
		BinaryFormatter formatter = new BinaryFormatter();
		SaveFile newSave = current;
		if (Control.Instance.allowTesting) {
			print("No saving!");
			return;
		}
		if (current.data.core.time != 0) {
			newSave.saveHistory.previousSaves.Add(DeepCopy(current.data));
		}

		string filePath = newSave.data.core.fileLocation;
		string imgFilePath = newSave.data.core.fileLocation.Remove(filePath.Length-10,10) + "0" + (newSave.saveHistory.previousSaves.Count + 1).ToString() + ".png";

		using (FileStream file = File.Open(filePath, FileMode.Open)) {
			SaveGameHelper.script.GetValues();

			#region Player data
			newSave.data.player.bombs = Player.Instance.pAttack.Bombs;
			newSave.data.player.bullets = Player.Instance.pAttack.Bullets;
			newSave.data.player.playerPos = isAutomatic && Player.Instance.GetCurrentBackground() == MapData.Instance.GetRoom(4).Background ? automaticSave_PreBoss1 : SaveGameHelper.script.playerPos;
			newSave.data.player.spikesCollected = SaveGameHelper.script.spike.SpikesCollected;
			newSave.data.player.coinsCollected = SaveGameHelper.script.coin.CoinsCollected;
			newSave.data.player.canZoom = Zoom.CanZoom;
			newSave.data.player.currentBGName = Player.Instance.GetCurrentBackground().name;
			newSave.data.player.gameProgression = Player.GameProgression;
			#endregion

			#region World data
			newSave.data.world.blockPos = SaveGameHelper.script.boxPos;
			newSave.data.world.blockZRotation = SaveGameHelper.script.ZRotationBlock;
			newSave.data.world.blockPushAttempt = pPlate.attempts;
			newSave.data.world.spikeActive = collectibles.Find("Spike").gameObject.activeSelf;
			newSave.data.world.spikePos = SaveGameHelper.script.spikePos;
			newSave.data.world.pressurePlateTriggered = pPlate.alreadyTriggered;
			newSave.data.world.doneAvoidance = avoidance.AvoidanceFinished;
			newSave.data.world.boss1Killed = MapData.Instance.BossOneKilled;
			newSave.data.world.postMazeDoorOpen = CameraMovement.Instance.IsInMaze == false && SaveGameHelper.script.spike.SpikesCollected >= 4 ? true : false;
			newSave.data.world.doorsOpen = new System.Collections.Generic.List<string>();
			for (int i = 0; i < MapData.Instance.AllDoors.Length; i++) {
				if (MapData.Instance.AllDoors[i].IsOpen) {
					newSave.data.world.doorsOpen.Add(MapData.Instance.AllDoors[i].FromRoomID + "," + MapData.Instance.AllDoors[i].ToRoomID);
				}
			}
			#endregion

			#region Hints data
			newSave.data.shownHints.currentlyDisplayedSideInfo = HUDisplay.script.slideInText.text;
			newSave.data.shownHints.shownBlockInfo = block.FirstApproachHint;
			newSave.data.shownHints.shootingIntro = Player.Instance.pAttack.attackModeIntro;
			#endregion

			#region Core data
			newSave.data.core.time = Timer.getTime;
			newSave.data.core.difficulty = Control.currDifficulty;
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

	public static T DeepCopy<T>(T other) {
		using (MemoryStream ms = new MemoryStream()) {
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(ms, other);
			ms.Position = 0;
			return (T)formatter.Deserialize(ms);
		}
	}

	public bool canSave {
		get { return _canSave; }
		set {
			_canSave = value;
			if (value) {
				saveButton.interactable = true;
			}
			else {
				saveButton.interactable = false;
			}
		}
	}
}
