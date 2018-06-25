using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseUnpause : MonoBehaviour {
	public delegate void Pause(bool isPausing);

	private static bool _canPause = true;
	private static bool _isPaused = false;

	public GameObject restartButton;
	public GameObject quitToMenu;
	public GameObject saveButton;
	public GameObject loadButton;

	private void Awake() {
		UserInterface.OnPauseChange += UserInterface_OnPauseChange;
		Timer.OnTimerPause += Timer_OnTimerPause;
		M_Player.OnPlayerDeath += Timer_OnPlayerDeath;
	}

	private void Timer_OnPlayerDeath(M_Player sender) {
		Cursor.visible = true;
		restartButton.SetActive(true);
		quitToMenu.SetActive(true);
		loadButton.SetActive(true);
		EventSystem.current.SetSelectedGameObject(loadButton);
		Time.timeScale = 0;
		Player_Movement.canMove = false;
		_isPaused = true;
		_canPause = false;
	}

	private void Timer_OnTimerPause(bool isPaused) {
		_isPaused = isPaused;
	}

	private void UserInterface_OnPauseChange(bool isPausing) {
		if (isPausing) {
			Cursor.visible = true;
			Timer.PauseTimer();
			restartButton.SetActive(true);
			quitToMenu.SetActive(true);
			saveButton.SetActive(true);
			EventSystem.current.SetSelectedGameObject(saveButton);
			Time.timeScale = 0;
			Player_Movement.canMove = false;
			_isPaused = true;

		}
		else {
			saveButton.GetComponentInChildren<Text>().text = "Save?";
			saveButton.GetComponent<Button>().interactable = true;
			saveButton.SetActive(false);
			restartButton.SetActive(false);
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
			_isPaused = false;
		}
	}

	public static bool canPause {
		get { return _canPause; }

		set { _canPause = value; }
	}

	public static bool isPaused {
		get { return _isPaused; }
	}

	private void OnDestroy() {
		UserInterface.OnPauseChange -= UserInterface_OnPauseChange;
		Timer.OnTimerPause -= Timer_OnTimerPause;
		M_Player.OnPlayerDeath -= Timer_OnPlayerDeath;
	}
}

