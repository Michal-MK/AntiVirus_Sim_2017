using UnityEngine;

public class GameUI : MonoBehaviour {

	public GameObject pauseGameOverlay;

	private void Awake() {
		Player.OnPlayerDeath += M_Player_OnPlayerDeath;
		PauseUnpause.OnPaused += OnGamePaused;
	}

	private void OnGamePaused(object sender, PauseEventArgs e) {
		if (WindowManager.getWindowCount > 0) {
			WindowManager.CloseMostRecent();
		}
		else {
			pauseGameOverlay.SetActive(e.IsPaused);
		}
	}

	private void M_Player_OnPlayerDeath(object sender, PlayerDeathEventArgs e) {
		Animator gameOverAnim = GameObject.Find("GameOver").GetComponent<Animator>();
		gameOverAnim.Play("GameOver");
		Cursor.visible = true;
		Player.OnPlayerDeath -= M_Player_OnPlayerDeath;
	}


	private void OnDestroy() {
		PauseUnpause.OnPaused -= OnGamePaused;
	}
}
