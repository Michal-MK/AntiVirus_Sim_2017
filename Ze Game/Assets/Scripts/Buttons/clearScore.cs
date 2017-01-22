using UnityEngine;
using System.Collections;

public class clearScore : MonoBehaviour {
	public displayScore dsp;


	public void OnPress(){
		dsp.clearScores();
		for (int i = 0; i < 54; i++) {
			PlayerPrefs.SetFloat(i.ToString(), 500f);
		}
		

	}
}
