using UnityEngine;
using System.Collections;

public class clearScore : MonoBehaviour {
	public displayScore dsp;

	public void OnPress() {

		foreach (GameObject delResults in dsp.results) {
			if (delResults) {
				delResults.SetActive(false);
			}
		}

		for (int i = 0; i < 54; i++) {
			PlayerPrefs.SetFloat(i.ToString(), 500f);
		}
		dsp.Display();

	}
}
