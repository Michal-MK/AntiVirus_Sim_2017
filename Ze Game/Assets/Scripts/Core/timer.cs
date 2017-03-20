using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour {
	public static float time;
	public static bool run;
	public bool attacking = false;
	public CameraMovement cam;
	Text Timer_text;

	private void Awake() {
		Statics.timerScript = this;
	}
	void Start() {

		Timer_text = GameObject.Find("Timer_text").GetComponent<Text>();
		Timer_text.gameObject.SetActive(false);
		if (!run) {
			time = 0f;
		}
	}


	void Update() {

		if (run == true && Coins.coinsCollected != 0) {

			Timer_text.gameObject.SetActive(true);

			if (!attacking) {
				time = time + Time.deltaTime;
			}
			else {
				time = time + (Time.deltaTime * 2);
			}

			Timer_text.text = "Time:\t" + (Mathf.Round(time * 100) / 100).ToString("0.00") + " s";
		}
	}
	private void OnDestroy() {
		Statics.timerScript = null;
	}
}