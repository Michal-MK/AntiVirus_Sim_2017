using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour {

	public void LoadByIndex(int sceneIndex){

		#if UNITY_EDITOR
		M_Player.doNotMove = false;
		SceneManager.LoadScene (1);

		#else
		M_Player.doNotMove = false;
		SceneManager.LoadScene (sceneIndex);

		#endif
	}
}
