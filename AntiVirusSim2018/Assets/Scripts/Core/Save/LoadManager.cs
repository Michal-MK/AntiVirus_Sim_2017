using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using Igor.Constants.Strings;
using System.Collections;

public class LoadManager {

	public static event SaveLoadedEventHandler OnSaveDataLoaded;

	private SaveData save;

	public void Load(string fileToLoad) {

		BinaryFormatter bf = new BinaryFormatter();
		SaveFile saveFile;
		using (FileStream file = File.Open(fileToLoad, FileMode.Open)) {
			saveFile = (SaveFile)bf.Deserialize(file);
			save = saveFile.data;
		}
		//CamFadeOut.registerMenuMusicVolumeFade = true;
		CamFadeOut.Instance.PlayTransition(CameraTransitionModes.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		SaveManager.current = saveFile;
	}

	public void Load(SaveData saveToLoad) {
		save = saveToLoad;
		CamFadeOut.Instance.PlayTransition(CameraTransitionModes.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;

		BinaryFormatter bf = new BinaryFormatter();
		using (FileStream file = File.Open(saveToLoad.core.fileLocation, FileMode.Open)) {
			SaveManager.current = (SaveFile)bf.Deserialize(file);
		}
	}

	private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode) {
		Control.Instance.StartCoroutine(DelayLoad());
		SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
	}

	private IEnumerator DelayLoad() {
		yield return new WaitForEndOfFrame(); //TODO why is this in a coroutine?
		OnSaveDataLoaded?.Invoke(save);
	}

	private void CamFadeOut_OnCamFullyFaded() {
		SceneManager.LoadScene(SceneNames.GAME1_SCENE);
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}
}