using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour {

	public delegate void SaveState(SaveData data);
	public static event SaveState OnSaveDataLoaded;

	public void Load(string fileToLoad) {

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(fileToLoad, FileMode.Open);

		SaveData data = (SaveData)bf.Deserialize(file);
		file.Close();

		StartCoroutine(FilesLoaded(data));
	}

	private IEnumerator FilesLoaded(SaveData data) {
		Statics.camFade.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES);
		yield return new WaitUntil(() => Statics.camFade.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f);
		Time.timeScale = 0;
		AsyncOperation loading = SceneManager.LoadSceneAsync(1);
		Statics.camFade.anim.speed = 0;
		loading.allowSceneActivation = true;
		yield return new WaitUntil(() => loading.isDone);
		if (OnSaveDataLoaded != null) {
			OnSaveDataLoaded(data);
		}
		OnSceneFinishedLoading(data);
		Statics.camFade.anim.speed = 1;
	}

	private void OnSceneFinishedLoading(SaveData loadedData) {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		GameObject block = GameObject.Find("Block");
		Wrapper wrp = GameObject.Find("Collectibles").GetComponent<Wrapper>();

		M_Player.gameProgression = loadedData.player.spikesCollected;

		player.transform.position = loadedData.player.playerPos;

		Vector3 spikePos = loadedData.world.spikePos;

		wrp.Objects[0].gameObject.SetActive(loadedData.world.spikeActive);
		if (wrp.Objects[0].activeInHierarchy == true) {
			wrp.Objects[0].transform.position = spikePos;
		}

		Coins.coinsCollected = loadedData.player.coinsCollected;
		Spike.spikesCollected = loadedData.player.spikesCollected;
		//Statics.canvasRenderer.Counters("Update");

		PlayerAttack.bullets = loadedData.player.bullets;
		PlayerAttack.bombs = loadedData.player.bombs;

		Statics.playerAttack.displayShootingInfo = loadedData.shownHints.shownShotInfo;

		Statics.blockScript.showInfo = loadedData.shownHints.shownBlockInfo;
		block.transform.position = loadedData.world.blockPos;
		block.transform.rotation = Quaternion.AngleAxis(loadedData.world.blockZRotation, Vector3.back);
		if (loadedData.world.blockPushAttempt == 3) {
			Statics.pressurePlate.CreateBarrier();
		}
		else {
			Statics.pressurePlate.attempts = loadedData.world.blockPushAttempt;
		}

		if (!loadedData.shownHints.shownShotInfo) {
			Statics.playerAttack.StartCoroutine(Statics.playerAttack.UpdateStats());
		}
		if (loadedData.player.coinsCollected == 5) {
			Statics.coins.ChatchUpToAttempt(loadedData.player.coinsCollected - 2);
			GameObject.Find("Coin").SetActive(false);
			Statics.coins.CoinBehavior();
		}
		else if (loadedData.player.coinsCollected <= 4) {
			Statics.coins.ChatchUpToAttempt(loadedData.player.coinsCollected - 2);
			Statics.coins.CoinBehavior();
			Statics.guide.Recalculate(GameObject.Find("Coin"), true);
		}

		PlayerPrefs.SetInt("difficulty", loadedData.core.difficulty);

		Statics.avoidance.displayAvoidInfo = loadedData.shownHints.shownAvoidanceInfo;
		Statics.avoidance.performed = loadedData.world.doneAvoidance;
		if (loadedData.world.doneAvoidance) {
			GameObject.Find("SignPost Avoidance").SetActive(false);
			Statics.music.PlayMusic(Statics.music.room1);
		}

		Timer.time = loadedData.core.time;

		Camera.main.orthographicSize = loadedData.core.camSize;
		Statics.zoom.canZoom = loadedData.player.canZoom;

		Statics.cameraMovement.inBossRoom = loadedData.world.bossSpawned;
		if (loadedData.world.bossSpawned) {
			RectTransform bg = GameObject.Find("Background_room_Boss_1").GetComponent<RectTransform>();
			Camera.main.transform.position = bg.position + new Vector3(0, 0, -10);
			Statics.cameraMovement.psA.transform.position = bg.position + new Vector3(0, bg.sizeDelta.y / 2, 0);
			ParticleSystem.ShapeModule shape = Statics.cameraMovement.psA.shape;
			shape.radius = 108 * 2;
			Statics.cameraMovement.psB.gameObject.SetActive(false);
			M_Player.gameProgression = 10;
			Statics.bossEntrance.SpawnBossOnLoad();
			print(Camera.main.transform.position);
			Statics.zoom.canZoom = false;
		}

		Statics.canvasRenderer.InfoRenderer(null, loadedData.shownHints.currentlyDisplayedSideInfo);
		Statics.pressurePlate.alreadyTriggered = loadedData.world.pressurePlateTriggered;

		GameObject.Find("Blocker3").SetActive(loadedData.world.postMazeDoorOpen);

		Statics.mPlayer.newGame = false;
		switch (loadedData.player.currentBGName) {
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
		M_Player.doNotMove = false;
		Timer.run = true;
		Time.timeScale = 1;
	}
}