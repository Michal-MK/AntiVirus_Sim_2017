using UnityEngine;
using System.Collections;

public class quitGame : MonoBehaviour {
	

public M_Player player;

	public void Quit(){

		#if UNITY_EDITOR
		M_Player.doNotMove = false;
		UnityEditor.EditorApplication.isPlaying = false;

		#else

			M_Player.doNotMove = false;
			Application.Quit();

		#endif
	}
}