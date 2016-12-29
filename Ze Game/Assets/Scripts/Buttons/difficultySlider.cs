using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class difficultySlider : MonoBehaviour {

	public Spike score;
	public static float diff;
	public static float difficulty = 1f;

	void Update(){
		diff = gameObject.GetComponent<Slider> ().value;
	}
	void FixedUpdate(){
		if (diff == 0) {
			difficulty = 2f; 
		}
		if (diff == 1) {
			difficulty = 2.5f; 
		}
		if (diff == 2) {
			difficulty = 3f; 
		}
		if (diff == 3) {
			difficulty = 3.5f; 
		}
		if (diff == 4) {
			difficulty = 4f; 
		}

		PlayerPrefs.SetInt ("difficulty", (int)diff);
	}
}
