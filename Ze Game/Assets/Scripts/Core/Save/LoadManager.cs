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
		using (FileStream file = File.Open(fileToLoad, FileMode.Open)) {
			loadedData = (SaveFile)bf.Deserialize(file);
		}

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
		SceneManager.sceneLoaded -= SceneLoadedWithData;

	}

	private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode) {
		if (OnSaveDataLoaded != null) {
			OnSaveDataLoaded(loadedData.data);
		}
		SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
	}

	private void CamFadeOut_OnCamFullyFaded() {
		SceneManager.LoadScene(SceneNames.GAME1_SCENE);
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}
}