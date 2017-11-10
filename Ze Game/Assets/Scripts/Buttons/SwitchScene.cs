using UnityEngine;
using UnityEngine.SceneManagement;


public class SwitchScene : MonoBehaviour {

	private int sceneIndexHolder = 0;

	public void SwitchTo(int Index) {
		SceneManager.LoadScene(Index);
		PlayerAttack.bullets = 0;
		PlayerAttack.bombs = 0;
		Coins.coinsCollected = 0;
		Projectile.projectileSpeed = 15;
		Spike.spikesCollected = 0;
		M_Player.gameProgression = 0;
		M_Player.doNotMove = false;
		Time.timeScale = 1;
		Timer.ResetTimer();
		if (CamFadeOut.script != null) {
			CamFadeOut.script.anim.SetTrigger("UnDim");
		}
	}

	public void DelayMenu(int i) {
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

		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
		sceneIndexHolder = i;
		if (MusicHandler.script.sound.volume != 0) {
			MusicHandler.script.StartCoroutine(MusicHandler.script.StopMusic());
		}
	}

	private void CamFadeOut_OnCamFullyFaded() {
		PlayerAttack.bullets = 0;
		PlayerAttack.bombs = 0;
		Coins.coinsCollected = 0;
		Projectile.projectileSpeed = 15;
		Spike.spikesCollected = 0;
		M_Player.gameProgression = 0;
		M_Player.doNotMove = false;
		Time.timeScale = 1;
		Timer.ResetTimer();
		SceneManager.LoadScene(sceneIndexHolder);
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}
}
