using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class quitTM : MonoBehaviour {

	public void quitToMM(int menuIndex){
		
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		M_Player.doNotMove = false;

		#else
		M_Player.doNotMove = false;
		SceneManager.LoadScene (menuIndex);
	
		#endif

	}
}
