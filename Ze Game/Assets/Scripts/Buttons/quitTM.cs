using UnityEngine;
using UnityEngine.SceneManagement;

public class quitTM : MonoBehaviour {

	public void quitToMM(int menuIndex){
		
		SceneManager.LoadScene (0);
		Spike.spikesCollected = 0;
		M_Player.gameProgression = 0;
		M_Player.doNotMove = false;
		Time.timeScale = 1;

	}
}
