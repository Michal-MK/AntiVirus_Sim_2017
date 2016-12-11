using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour {
	public static float time_er;
	Text text;
	public static bool run;

	void Start () {
		time_er = 0f;
		text = this.GetComponent<Text> (); 

	}


	void FixedUpdate () {

		if (run == true) {
			time_er = time_er + Time.deltaTime;
			text.text = "Time: " + (Mathf.Round(time_er * 100) / 100) + " s";
		}
	}
}
