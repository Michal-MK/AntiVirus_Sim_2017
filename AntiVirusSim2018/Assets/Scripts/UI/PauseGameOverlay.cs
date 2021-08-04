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
		PlayerMovement.CanMove = false;
		Timer.Instance.IsRunning = false;
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
		PlayerMovement.CanMove = true;
		Time.timeScale = 1;

		if (Player.PlayerState == PlayerState.NORMAL) {
			Timer.StartTimer(1f);
		}
		else {
			Timer.StartTimer(2f);
		}
	}
}
