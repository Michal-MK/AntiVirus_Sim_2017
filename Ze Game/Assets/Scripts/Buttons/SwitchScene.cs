using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SwitchScene : MonoBehaviour {

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

	public void DelayMenuWrapper(int i) {
		StartCoroutine(DelayMenu(i));
	}

	private IEnumerator DelayMenu(int i) {
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
		if (MusicHandler.script.sound.volume != 0) {
			MusicHandler.script.StartCoroutine(MusicHandler.script.StopMusic());
		}
		yield return new WaitForSecondsRealtime(2);

		PlayerAttack.bullets = 0;
		PlayerAttack.bombs = 0;
		Coins.coinsCollected = 0;
		Projectile.projectileSpeed = 15;
		Spike.spikesCollected = 0;
		M_Player.gameProgression = 0;
		M_Player.doNotMove = false;
		Time.timeScale = 1;
		Timer.ResetTimer();
		SceneManager.LoadScene(i);
	}
}
