using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SwitchScene : MonoBehaviour {
	private void Awake() {
		Statics.switchScene = this;
	}

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
		Statics.camFade.anim.SetTrigger("UnDim");

	}

	public void DelayMenuWrapper() {
		StartCoroutine(DelayManu());
	}

	private IEnumerator DelayManu() {
		GameObject save = GameObject.Find("saveGame");
		GameObject quit = GameObject.Find("quitToMenu");
		GameObject rest = GameObject.Find("restartButton");

		rest.SetActive(false);
		if (save != null) {
			save.SetActive(false);
		}
		quit.transform.position = new Vector3(0,-200,10);

		Statics.camFade.PlayTransition("Trans");
		if (Statics.music.sound.volume != 0) {
			Statics.music.StartCoroutine(Statics.music.StopMusic());
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
		SceneManager.LoadScene(0);
	}

	private void OnDestroy() {
		Statics.switchScene = null;
	}
}
