using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SwitchScene : MonoBehaviour {
	private void Awake() {
		Statics.switchScene = this;
	}

	public void SwitchTo(int Index){
		
		SceneManager.LoadScene (Index);
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
	private void OnDestroy() {
		Statics.switchScene = null;
	}
}
