using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class difficultySlider : MonoBehaviour {

	public Spike score;
	public float diff;
	public static float difficulty = 1;

	void Update(){
		diff = gameObject.GetComponent<Slider> ().value;
	}
	void FixedUpdate(){
		if (diff == 0) {
			difficulty = 1f; 
		}
		if (diff == 1) {
			difficulty = 1.2f; 
		}
		if (diff == 2) {
			difficulty = 1.5f; 
		}
		if (diff == 3) {
			difficulty = 1.7f; 
		}
		if (diff == 4) {
			difficulty = 2f; 
		}
	}
}
