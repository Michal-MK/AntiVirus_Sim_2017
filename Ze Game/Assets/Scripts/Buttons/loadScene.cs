using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour {

	public void LoadByIndex(int sceneIndex){
		M_Player.doNotMove = false;
		SceneManager.LoadScene (sceneIndex);
	}
}
