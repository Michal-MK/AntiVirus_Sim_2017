using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour {

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

	}
}
