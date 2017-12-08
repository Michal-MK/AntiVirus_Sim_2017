using UnityEngine;
using UnityEngine.SceneManagement;


public class SwitchScene : MonoBehaviour {

	private string sceneNameHolder;

	public void SwitchTo(string name) {
		SceneManager.LoadScene(name);
		if (CamFadeOut.script != null) {
			CamFadeOut.script.anim.SetTrigger("UnDim");
		}
	}

	public void DelayMenu(string sceneName) {
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
		sceneNameHolder = sceneName;
		if (MusicHandler.script.musicPlayer.volume != 0) {
			MusicHandler.script.FadeMusic();
		}
	}

	private void CamFadeOut_OnCamFullyFaded() {
		SceneManager.LoadScene(sceneNameHolder);
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}
}
