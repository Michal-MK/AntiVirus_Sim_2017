using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour {
	public static float time_er;
	public static bool run;
	public CameraMovement cam;
	GameObject seconds;
	GameObject Timer_text;

	void Start () {

		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent <CameraMovement>();
		Timer_text = GameObject.Find ("Timer_text");
		Timer_text.SetActive (false);
		seconds = GameObject.Find ("seconds");
		seconds.SetActive (false);

		print (Timer_text.activeInHierarchy);
		time_er = 0f;
	}


	void FixedUpdate () {
		if (cam.inBossRoom == true) {
			run = false;
		}

		if (run == true) {

			Timer_text.SetActive (true);
			seconds.SetActive (true);

			time_er = time_er + Time.fixedDeltaTime;
			Timer_text.GetComponent <Text> ().text = "Time: " + (Mathf.Round (time_er * 100) / 100);		

			if (time_er >= 0 && time_er <= 9.95) {
				seconds.gameObject.transform.position = new Vector3 (Timer_text.transform.position.x + 30, Timer_text.transform.position.y, 0);
			}
		
			if (time_er >= 10 && time_er <= 10.02) {
				seconds.gameObject.transform.position = new Vector3 (Timer_text.transform.position.x + 40, Timer_text.transform.position.y, 0);
			}
			if (time_er >= 60 && time_er <= 60.02) {
				run = false;
				seconds.SetActive (false);
				Timer_text.GetComponent <Text> ().text = "Are you even trying ?";
			}
		}
		else {
			Timer_text.SetActive (false);
			seconds.SetActive (false);
		}
	}
}