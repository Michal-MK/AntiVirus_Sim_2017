using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class timer : MonoBehaviour {
	public static float time;
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
		time = 0f;
	}


	void FixedUpdate () {
		if (cam.inBossRoom == true) {
			run = false;
		}

		if (run == true) {

			Timer_text.SetActive (true);
			seconds.SetActive (true);

			time = time + Time.fixedDeltaTime;
			Timer_text.GetComponent <Text> ().text = "Time: " + (Mathf.Round (time * 100) / 100);		

			if (time <= 10) {
				seconds.transform.position = Timer_text.transform.position + new Vector3(44,0,0);
			}
		
			if (time >= 10 && time <= 100) {
				seconds.transform.position = Timer_text.transform.position + new Vector3(54, 0, 0);
			}
			if (time >= 100) {
				seconds.transform.position = Timer_text.transform.position + new Vector3(172, 0, 0);
				Timer_text.GetComponent <Text> ().text = "Are you even trying ? " + (Mathf.Round (time * 100) / 100);
			}
		}
		else {
			Timer_text.SetActive (false);
			seconds.SetActive (false);
		}
	}
}