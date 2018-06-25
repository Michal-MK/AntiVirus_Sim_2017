using UnityEngine;
using UnityEngine.SceneManagement;
using Igor.Constants.Strings;

public class SwitchScene : MonoBehaviour {

	private string sceneNameHolder;

	public void SwitchTo(string name) {
		if (name == SceneNames.MENU_SCENE && SceneManager.GetActiveScene().name == SceneNames.GAME1_SCENE) {
			DelayMenu();
			return;
		}
		SceneManager.LoadScene(name);
		if (CamFadeOut.script != null) {
			CamFadeOut.script.anim.SetTrigger("UnDim");
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

		rest.SetActive(false);
		if (save != null) {
			save.SetActive(false);
		}
		if(load != null) {
			load.SetActive(false);
		}
		quit.transform.position = new Vector3(0,-200,10);

		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
		sceneNameHolder = SceneNames.MENU_SCENE;
		if (MusicHandler.script.musicPlayer.volume != 0) {
			MusicHandler.script.FadeMusic();
		}
	}

	private void CamFadeOut_OnCamFullyFaded() {
		SceneManager.LoadScene(sceneNameHolder);
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}
}
