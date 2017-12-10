using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using Igor.Constants.Strings;

public class LoadManager {

	public delegate void SaveState(SaveData data);
	public static event SaveState OnSaveDataLoaded;
	private SaveFile loadedData;

	private SaveData save;

	public void Load(string fileToLoad) {

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(fileToLoad, FileMode.Open);

		loadedData = (SaveFile)bf.Deserialize(file);
		file.Close();

		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		SaveManager.current = loadedData;
	}

	public void Load(SaveData saveToLoad) {
		save = saveToLoad;
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
		SceneManager.sceneLoaded += SceneLoadedWithData;
	}

	private void SceneLoadedWithData(Scene scene, LoadSceneMode mode) {
		if (OnSaveDataLoaded != null) {
			OnSaveDataLoaded(save);
		}
		OnSceneFinishedLoading(save);
		SceneManager.sceneLoaded -= SceneLoadedWithData;

	}

	private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode) {
		if (OnSaveDataLoaded != null) {
			OnSaveDataLoaded(loadedData.data);
		}
		OnSceneFinishedLoading(loadedData.data);
		SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
	}

	private void CamFadeOut_OnCamFullyFaded() {
		SceneManager.LoadScene(SceneNames.GAME1_SCENE);
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}

	private void OnSceneFinishedLoading(SaveData loadedData) {
		if (loadedData.world.doneAvoidance) {
			GameObject.Find("SignPost Avoidance").SetActive(false);
			//MusicHandler.script.PlayMusic(MusicHandler.script.room1_1);
		}
		Camera.main.orthographicSize = loadedData.core.camSize;
		Canvas_Renderer.script.InfoRenderer(null, loadedData.shownHints.currentlyDisplayedSideInfo);
		GameObject.Find("Blocker3").SetActive(loadedData.world.postMazeDoorOpen);

		//switch (loadedData.player.currentBGName) {
		//	case "Background_Start": {
		//		MusicHandler.script.PlayMusic(MusicHandler.script.room1_1);
		//		break;
		//	}
		//	case "Background_room_2b": {
		//		MusicHandler.script.PlayMusic(MusicHandler.script.room1_1);
		//		break;
		//	}
		//	case "Background_room_Boss_1": {
		//		MusicHandler.script.PlayMusic(MusicHandler.script.room_1_boss);
		//		break;
		//	}
		//	case "MazeBG": {
		//		MusicHandler.script.PlayMusic(MusicHandler.script.room_maze);
		//		break;
		//	}
		//	default: {
		//		break;
		//	}
		//}
		Player_Movement.canMove = true;
		Time.timeScale = 1;
	}
}