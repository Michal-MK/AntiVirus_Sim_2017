using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class restartOnClick : MonoBehaviour {

	public void Quit(){
		
		M_Player.doNotMove = false;
		Spike.i = 0;
		M_Player.gameProgression = 0;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	}

}
