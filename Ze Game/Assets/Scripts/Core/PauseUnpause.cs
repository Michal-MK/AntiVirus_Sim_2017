using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseUnpause : MonoBehaviour {

	public static bool isPaused = false;
	public GameObject restartButton;
	public GameObject quitToMenu;
	public GameObject saveButton;
	public GameObject loadButton;

	private void Update() {
		if (!Canvas_Renderer.script.isRunning && !M_Player.player.gameOver) {
			if (Input.GetButtonDown("Escape") && !isPaused) {

				Cursor.visible = true;
				Timer.run = false;
				restartButton.SetActive(true);
				quitToMenu.SetActive(true);
				saveButton.SetActive(true);

				Time.timeScale = 0;
				EventSystem e = EventSystem.current;
				e.SetSelectedGameObject(saveButton);
				isPaused = true;
			}
			else if (Input.GetButtonDown("Escape") && isPaused) {
				saveButton.GetComponentInChildren<Text>().text = "Save?";
				saveButton.GetComponent<Toggle>().interactable = true;
				saveButton.GetComponent<Toggle>().isOn = false;
				saveButton.SetActive(false);
				restartButton.SetActive(false);
				quitToMenu.SetActive(false);
				Cursor.visible = false;
				Time.timeScale = 1;
				Timer.run = true;

				isPaused = false;
			}
		}
	}
}

