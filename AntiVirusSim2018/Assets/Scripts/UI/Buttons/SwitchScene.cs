using UnityEngine;
using UnityEngine.SceneManagement;
using Igor.Constants.Strings;
using System;

public class SwitchScene : MonoBehaviour {

	private string sceneNameHolder;

	public void SwitchTo(string name) {
		if (name == SceneNames.MENU_SCENE && SceneManager.GetActiveScene().name == SceneNames.GAME1_SCENE) {
			DelayMenu();
			return;
		}
		SceneManager.LoadScene(name);
		if (CamFadeOut.Instance != null) {
			CamFadeOut.Instance.Animator.SetTrigger("UnDim");
		}
	}

	public void SwitchToWithMusicStop(string name) {
		SceneManager.LoadScene(name);
		MenuMusic.script.StopMusic();
	}

	private void DelayMenu() {
		GameObject save = GameObject.Find("saveGame");
		GameObject quit = GameObject.Find("quitToMenu");
		GameObject rest = GameObject.Find("restartButton");
		GameObject load = GameObject.Find("loadGame");
		GameObject settings = GameObject.Find("settingsButton");

		rest.SetActive(false);
		settings.SetActive(false);
		if (save != null) {
			save.SetActive(false);
		}
		if (load != null) {
			load.SetActive(false);
		}

		quit.transform.position = new Vector3(0, -200, 10);
		CamFadeOut.registerGameMusicVolumeFade = true;
		CamFadeOut.Instance.PlayTransition(CameraTransitionModes.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
		sceneNameHolder = SceneNames.MENU_SCENE;
	}

	private void CamFadeOut_OnCamFullyFaded() {
		SceneManager.LoadScene(sceneNameHolder);
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}
}
