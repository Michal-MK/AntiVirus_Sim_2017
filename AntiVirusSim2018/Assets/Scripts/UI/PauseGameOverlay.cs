using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseGameOverlay : MonoBehaviour {
	public GameObject restartButton;
	public GameObject settingsButton;
	public GameObject quitToMenu;
	public GameObject saveButton;
	public GameObject loadButton;

	public void OnEnable() {
		restartButton.SetActive(true);
		settingsButton.SetActive(true);
		quitToMenu.SetActive(true);
		saveButton.SetActive(true);
		EventSystem.current.SetSelectedGameObject(saveButton);
		Cursor.visible = true;
		Player_Movement.canMove = false;
		Timer.script.isRunning = false;
		Time.timeScale = 0;
	}

	public void OnDisable() {
		saveButton.GetComponentInChildren<Text>().text = "Save?";
		saveButton.GetComponent<Button>().interactable = true;
		saveButton.SetActive(false);
		restartButton.SetActive(false);
		settingsButton.SetActive(false);
		quitToMenu.SetActive(false);
		Cursor.visible = false;
		Player_Movement.canMove = true;
		Time.timeScale = 1;

		if (M_Player.playerState == M_Player.PlayerState.NORMAL) {
			if (Coin.coinsCollected > 0) {
				Timer.StartTimer(1f);
			}
		}
		else {
			Timer.StartTimer(2f);
		}
	}
}
