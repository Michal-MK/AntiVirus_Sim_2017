using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour {

	public void LoadByIndex(int sceneIndex){

		#if UNITY_EDITOR
		M_Player.doNotMove = false;
		UnityEditor.EditorApplication.isPlaying = false;

		#else
		M_Player.doNotMove = false;
		SceneManager.LoadScene (sceneIndex);

		#endif
	}
}
