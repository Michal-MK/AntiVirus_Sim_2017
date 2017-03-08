using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {
	public static float time;
	public static bool run;
	public bool attacking = false;
	public CameraMovement cam;
	Text Timer_text;

	void Start() {

		Timer_text = GameObject.Find("Timer_text").GetComponent<Text>();
		Timer_text.gameObject.SetActive(false);
		time = 0f;
	}


	void FixedUpdate() {

		if (run == true) {

			Timer_text.gameObject.SetActive(true);

			if (attacking == false) {
				time = time + Time.fixedDeltaTime;
			}
			else {
				time = time + (Time.fixedDeltaTime * 2);
			}

			Timer_text.text = "Time:\t" + (Mathf.Round(time * 100) / 100).ToString("0.00") + " s";
		}
	}
}