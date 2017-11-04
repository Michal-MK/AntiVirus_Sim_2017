using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour {

	public void Save(bool newSaveFile) {
		BinaryFormatter formatter = new BinaryFormatter();
		int chosenDifficulty = Control.currDifficulty;
		SaveData data;
		FileStream file;

		if (newSaveFile) {
			file = File.Create(Application.dataPath + "/Saves/D" + chosenDifficulty + "/Save-D" + chosenDifficulty + ".Kappa");
			data = new SaveData();
		}
		else {
			file = File.Open(Application.dataPath + "/Saves/D" + chosenDifficulty + "/Save-D" + chosenDifficulty + ".Kappa", FileMode.Open);
			data = SaveData.current;
		}
		Statics.gameProgression.GetValues();

		#region Player data
		data.player.bombs = PlayerAttack.bombs;
		data.player.bullets = PlayerAttack.bullets;
		data.player.playerPos = Statics.gameProgression.playerPos;
		data.player.spikesCollected = Spike.spikesCollected;
		data.player.coinsCollected = Coins.coinsCollected;
		data.player.canZoom = Statics.zoom.canZoom;
		data.player.currentBGName = M_Player.currentBG_name;
		#endregion

		#region World data
		data.world.blockPos = Statics.gameProgression.boxPos;
		data.world.blockZRotation = Statics.gameProgression.ZRotationBlock;
		data.world.blockPushAttempt = Statics.pressurePlate.attempts;
		data.world.spikeActive = GameObject.Find("Collectibles").GetComponent<Wrapper>().Objects[0].activeSelf;
		data.world.spikePos = Statics.gameProgression.spikePos;
		data.world.pressurePlateTriggered = Statics.pressurePlate.alreadyTriggered;
		data.world.postMazeDoorOpen = !Statics.mazeEscape.pathOpen;
		data.world.doneAvoidance = Statics.avoidance.performed;
		data.world.bossSpawned = Statics.bossBehaviour.bossSpawned;
		#endregion

		#region Core data
		data.core.camSize = Camera.main.orthographicSize;
		data.core.difficulty = chosenDifficulty;
		data.core.time = Timer.time;
		data.core.localAttempt = Control.currAttempt;
		#endregion

		#region Hints data
		data.shownHints.currentlyDisplayedSideInfo = Canvas_Renderer.script.info_S.text;
		data.shownHints.shownAttempt = Statics.mPlayer.newGame;
		data.shownHints.shownAvoidanceInfo = Statics.avoidance.displayAvoidInfo;
		data.shownHints.shownBlockInfo = Statics.blockScript.showInfo;
		data.shownHints.shownShotInfo = Statics.playerAttack.displayShootingInfo;
		#endregion

		formatter.Serialize(file, data);
		file.Close();
		StartCoroutine(ScreenShot(chosenDifficulty));
	}

	private IEnumerator ScreenShot(int currAttempt) {
		GameObject saveButton = GameObject.Find("saveGame");
		if (saveButton != null) {
			yield return new WaitUntil(() => !saveButton.activeInHierarchy);
		}
		print("Captured");
		ScreenCapture.CaptureScreenshot(Application.dataPath + "/Saves/D" + currAttempt + "/Resources/Save-D" + currAttempt + "_" + currAttempt.ToString("000") + ".png");
	}
}
