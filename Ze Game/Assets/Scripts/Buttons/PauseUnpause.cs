using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUnpause : MonoBehaviour {

	public bool toggle = false;
	public GameObject restartButton;
	public GameObject quitToMenu;
	public GameObject continueButton;
	public Vector3 oldpos;

	private void Start() {
		oldpos = continueButton.transform.position;
		continueButton.transform.position = new Vector3( -1000, -1000, -100);
	}

	private void Update() {

		if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex == 1) {
			toggle = !toggle;

			if (toggle) {

				M_Player.doNotMove = true;
				Cursor.visible = true;
				timer.run = false;
				restartButton.SetActive(true);
				quitToMenu.SetActive(true);
				continueButton.transform.position = restartButton.transform.position + new Vector3(0, 50, 0);
				Time.timeScale = 0;

			}
		}
	}

	public void OnButtonPress(){
		M_Player.doNotMove = false;
		Cursor.visible = false;
		timer.run = true;
		restartButton.SetActive(false);
		quitToMenu.SetActive(false);
		continueButton.transform.position = new Vector3(-1000, -1000, -100);
		Time.timeScale = 1;
	}
}

