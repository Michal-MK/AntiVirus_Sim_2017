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

	public static SaveData current;

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
		current = new SaveData();

		current.core.time = 0;
		current.core.difficulty = difficulty;
		current.core.localAttempt = 0;

		try {
			File.Copy(Application.streamingAssetsPath + Path.DirectorySeparatorChar + "NewGame.png", imgPath);
		}
		catch(IOException) {
			if (File.Exists(imgPath)) {
				print("File Exists already");
			}
		}
		
		formatter.Serialize(file, current);
		file.Close();
		file.Dispose();
	}

	public void Save(int difficulty, bool newSaveFile = false) {
		BinaryFormatter formatter = new BinaryFormatter();
		SaveData data;
		FileStream file;

		if (newSaveFile) {
			file = File.Create(Application.dataPath + "/Saves/D" + difficulty + "/Save-D" + difficulty + ".Kappa");
			data = new SaveData();
		}
		else {
			file = File.Open(Application.dataPath + "/Saves/D" + difficulty + "/Save-D" + difficulty + ".Kappa", FileMode.Open);
			data = current;
		}
		GameProgression.script.GetValues();

		#region Player data
		data.player.bombs = PlayerAttack.bombs;
		data.player.bullets = PlayerAttack.bullets;
		data.player.playerPos = GameProgression.script.playerPos;
		data.player.spikesCollected = Spike.spikesCollected;
		data.player.coinsCollected = Coins.coinsCollected;
		data.player.canZoom = Zoom.canZoom;
		data.player.currentBGName = M_Player.currentBG_name;
		#endregion

		#region World data
		data.world.blockPos = GameProgression.script.boxPos;
		data.world.blockZRotation = GameProgression.script.ZRotationBlock;
		data.world.blockPushAttempt = pPlate.attempts;
		data.world.spikeActive = collectibles.Find("Spike").gameObject.activeSelf;
		data.world.spikePos = GameProgression.script.spikePos;
		data.world.pressurePlateTriggered = pPlate.alreadyTriggered;
		data.world.doneAvoidance = avoidance.performed;
		if (boss != null) {
			data.world.bossSpawned = true;
		}
		else {
			data.world.bossSpawned = false;
		}
		if(Maze.inMaze == false && Spike.spikesCollected >= 4) {
			data.world.postMazeDoorOpen = true;
		}
		else {
			data.world.postMazeDoorOpen = false;
		}
		#endregion

		#region Core data
		data.core.camSize = Camera.main.orthographicSize;
		data.core.difficulty = difficulty;
		data.core.time = Timer.getTime;
		data.core.localAttempt = Control.currAttempt;
		#endregion

		#region Hints data
		data.shownHints.currentlyDisplayedSideInfo = Canvas_Renderer.script.info_S.text;
		data.shownHints.shownAttempt = M_Player.player.newGame;
		data.shownHints.shownAvoidanceInfo = avoidance.displayAvoidInfo;
		data.shownHints.shownBlockInfo = block.showInfo;
		data.shownHints.shownShotInfo = M_Player.player.pAttack.displayShootingInfo;
		#endregion

		#region Core data
		data.core.time = Timer.getTime;
		data.core.difficulty = Control.currDifficulty;
		data.core.camSize = Camera.main.orthographicSize;
		#endregion

		formatter.Serialize(file, data);
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
