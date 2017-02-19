using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {
	public static float time;
	public static bool run;
	public CameraMovement cam;
	GameObject Timer_text;

	void Start () {

		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent <CameraMovement>();
		Timer_text = GameObject.Find ("Timer_text");
		Timer_text.SetActive (false);
		time = 0f;
	}


	void FixedUpdate () {
		if (cam.inBossRoom == true) {
			run = false;
		}

		if (run == true) {

			Timer_text.SetActive (true);

			time = time + Time.fixedDeltaTime;
			Timer_text.GetComponent <Text> ().text = "Time:\t" + (Mathf.Round (time * 100) / 100).ToString("0.00") + " s";		
		}
		else {
			Timer_text.SetActive (false);
		}
	}
}