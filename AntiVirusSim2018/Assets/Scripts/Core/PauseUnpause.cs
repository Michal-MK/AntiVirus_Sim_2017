using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseUnpause : MonoBehaviour {
	public delegate void Pause(bool isPausing);

	public GameObject restartButton;
	public GameObject settingsButton;
	public GameObject quitToMenu;
	public GameObject saveButton;
	public GameObject loadButton;


	public static bool canPause { get; set; } = true;
	public static bool isPaused { get; private set; } = false;


	private void Awake() {
		UserInterface.OnPauseChange += UserInterface_OnPauseChange;
		Timer.OnTimerPause += Timer_OnTimerPause;
		M_Player.OnPlayerDeath += Timer_OnPlayerDeath;
	}

	private void Timer_OnPlayerDeath(M_Player sender) {
		Cursor.visible = true;
		restartButton.SetActive(true);
		settingsButton.SetActive(true);
		quitToMenu.SetActive(true);
		loadButton.SetActive(true);
		EventSystem.current.SetSelectedGameObject(loadButton);
		Time.timeScale = 0;
		Player_Movement.canMove = false;
		isPaused = true;
		canPause = false;
	}

	private void Timer_OnTimerPause(bool isPaused) {
		PauseUnpause.isPaused = isPaused;
	}

	private void UserInterface_OnPauseChange(bool isPausing) {
		if (isPausing) {
			Cursor.visible = true;
			Timer.PauseTimer();
			restartButton.SetActive(true);
			settingsButton.SetActive(true);
			quitToMenu.SetActive(true);
			saveButton.SetActive(true);
			EventSystem.current.SetSelectedGameObject(saveButton);
			Time.timeScale = 0;
			Player_Movement.canMove = false;
			isPaused = true;

		}
		else {
			saveButton.GetComponentInChildren<Text>().text = "Save?";
			saveButton.GetComponent<Button>().interactable = true;
			saveButton.SetActive(false);
			restartButton.SetActive(false);
			settingsButton.SetActive(false);
			quitToMenu.SetActive(false);
			Cursor.visible = false;
			Time.timeScale = 1;
			Player_Movement.canMove = true;
			if (M_Player.playerState == M_Player.PlayerState.NORMAL) {
				if (Coin.coinsCollected > 0) {
					Timer.StartTimer(1f);
				}
			}
			else {
				Timer.StartTimer(2f);
			}
			isPaused = false;
		}
	}

	private void OnDestroy() {
		UserInterface.OnPauseChange -= UserInterface_OnPauseChange;
		Timer.OnTimerPause -= Timer_OnTimerPause;
		M_Player.OnPlayerDeath -= Timer_OnPlayerDeath;
	}
}

