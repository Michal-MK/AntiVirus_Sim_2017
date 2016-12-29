using UnityEngine;
using System.Collections;

public class clearScore : MonoBehaviour {
	public displayScore dsp;


	public void OnPress(){
		for (int i = 0; i < 54; i++) {
			if (PlayerPrefs.GetFloat (i.ToString ()) != 500f) {
				PlayerPrefs.SetFloat (i.ToString (), 500f);
			
			}
			
		}

	}
}
