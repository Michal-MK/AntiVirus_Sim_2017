using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class restartOnClick : MonoBehaviour {

	public void Restart(){
		
		M_Player.doNotMove = false;
		Spike.spikesCollected = 0;
		Coins.coinsCollected = 0;
		M_Player.gameProgression = 0;
		Projectile.projectileSpeed = 15;
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	}

}
