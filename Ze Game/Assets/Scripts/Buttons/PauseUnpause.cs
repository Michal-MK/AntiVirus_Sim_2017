using UnityEngine;

public class PauseUnpause : MonoBehaviour {

	public bool isPaused = false;
	public GameObject restartButton;
	public GameObject quitToMenu;


	private void Update() {

		if (Input.GetKeyDown(KeyCode.Escape) && !isPaused) {
			print("A");
			Cursor.visible = true;
			timer.run = false;
			restartButton.SetActive(true);
			quitToMenu.SetActive(true);
			Time.timeScale = 0;

			isPaused = true;
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && isPaused){
			print("B");

			restartButton.SetActive(false);
			quitToMenu.SetActive(false);
			Cursor.visible = false;
			Time.timeScale = 1;
			timer.run = true;

			isPaused = false;
		}
	}

}

