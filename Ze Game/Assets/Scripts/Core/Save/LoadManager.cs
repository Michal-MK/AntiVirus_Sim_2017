using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using Igor.Constants.Strings;
using System.Collections;

public class LoadManager {

	public delegate void SaveState(SaveData data);
	public static event SaveState OnSaveDataLoaded;

	private SaveData save;

	public void Load(string fileToLoad) {

		BinaryFormatter bf = new BinaryFormatter();
		SaveFile saveFile;
		using (FileStream file = File.Open(fileToLoad, FileMode.Open)) {
			saveFile = (SaveFile)bf.Deserialize(file);
			save = saveFile.data;
		}

		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		SaveManager.current = saveFile;
	}

	public void Load(SaveData saveToLoad) {
		save = saveToLoad;
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;

		BinaryFormatter bf = new BinaryFormatter();
		using (FileStream file = File.Open(saveToLoad.core.fileLocation, FileMode.Open)) {
			SaveManager.current = (SaveFile)bf.Deserialize(file);
		}
	}

	private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode) {
		Control.script.StartCoroutine(DelayLoad());
		SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
	}

	private IEnumerator DelayLoad() {
		yield return new WaitForEndOfFrame();
		if (OnSaveDataLoaded != null) {
			OnSaveDataLoaded(save);
		}
	}

	private void CamFadeOut_OnCamFullyFaded() {
		SceneManager.LoadScene(SceneNames.GAME1_SCENE);
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}
}