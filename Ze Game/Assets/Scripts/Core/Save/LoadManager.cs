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
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES);
		yield return new WaitUntil(() => CamFadeOut.script.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f);
		Time.timeScale = 0;
		AsyncOperation loading = SceneManager.LoadSceneAsync(1);
		CamFadeOut.script.anim.speed = 0;
		loading.allowSceneActivation = true;
		yield return new WaitUntil(() => loading.isDone);
		if (OnSaveDataLoaded != null) {
			OnSaveDataLoaded(data);
		}
		OnSceneFinishedLoading(data);
		CamFadeOut.script.anim.speed = 1;
	}

	private void OnSceneFinishedLoading(SaveData loadedData) {

		Wrapper wrp = GameObject.Find("Collectibles").GetComponent<Wrapper>();

		Vector3 spikePos = loadedData.world.spikePos;

		wrp.Objects[0].gameObject.SetActive(loadedData.world.spikeActive);
		if (wrp.Objects[0].activeInHierarchy == true) {
			wrp.Objects[0].transform.position = spikePos;
		}

		Coins.coinsCollected = loadedData.player.coinsCollected;
		Spike.spikesCollected = loadedData.player.spikesCollected;
		//Canvas_Renderer.script.Counters("Update");

		PlayerAttack.bullets = loadedData.player.bullets;
		PlayerAttack.bombs = loadedData.player.bombs;

		PlayerPrefs.SetInt("difficulty", loadedData.core.difficulty);

		if (loadedData.world.doneAvoidance) {
			GameObject.Find("SignPost Avoidance").SetActive(false);
			MusicHandler.script.PlayMusic(MusicHandler.script.room1);
		}

		Timer.time = loadedData.core.time;

		Camera.main.orthographicSize = loadedData.core.camSize;
		Zoom.canZoom = loadedData.player.canZoom;

		Canvas_Renderer.script.InfoRenderer(null, loadedData.shownHints.currentlyDisplayedSideInfo);

		GameObject.Find("Blocker3").SetActive(loadedData.world.postMazeDoorOpen);

		switch (loadedData.player.currentBGName) {
			case "Background_Start": {
				MusicHandler.script.PlayMusic(MusicHandler.script.room1);
				break;
			}
			case "Background_room_2b": {
				MusicHandler.script.PlayMusic(MusicHandler.script.room1);
				break;
			}
			case "Background_room_Boss_1": {
				MusicHandler.script.PlayMusic(MusicHandler.script.boss);
				break;
			}
			case "MazeBG": {
				MusicHandler.script.PlayMusic(MusicHandler.script.maze);
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