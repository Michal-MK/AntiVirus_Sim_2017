using UnityEngine;
using UnityEngine.UI;

public class PauseUnpause : MonoBehaviour {

	public bool isPaused = false;
	public GameObject restartButton;
	public GameObject quitToMenu;
	public GameObject saveButton;


	private void Awake() {
		Statics.pauseUnpause = this;
	}

	private void Update() {
		if (!Statics.canvasRenderer.isRunning) {
			if (Input.GetKeyDown(KeyCode.Escape) && !isPaused) {

				Cursor.visible = true;
				timer.run = false;
				restartButton.SetActive(true);
				quitToMenu.SetActive(true);
				saveButton.SetActive(true);

				Time.timeScale = 0;

				isPaused = true;
			}
			else if (Input.GetKeyDown(KeyCode.Escape) && isPaused) {
				saveButton.GetComponentInChildren<Text>().text = "Save?";
				saveButton.GetComponent<Toggle>().isOn = false;
				saveButton.SetActive(false);
				restartButton.SetActive(false);
				quitToMenu.SetActive(false);
				Cursor.visible = false;
				Time.timeScale = 1;
				timer.run = true;

				isPaused = false;
			}
		}
	}
	private void OnDestroy() {
		Statics.pauseUnpause = null;
	}
}

