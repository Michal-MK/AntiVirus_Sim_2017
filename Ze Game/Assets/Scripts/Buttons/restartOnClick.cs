using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class restartOnClick : MonoBehaviour {

	public void Quit(){

		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		M_Player.doNotMove = false;

		#else

		M_Player.doNotMove = false;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		#endif
	}

}
