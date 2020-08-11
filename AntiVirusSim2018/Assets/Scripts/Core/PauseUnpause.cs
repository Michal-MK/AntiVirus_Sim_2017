using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUnpause : MonoBehaviour {

	public static event EventHandler<PauseEventArgs> OnPaused;

	public static bool canPause { get; set; } = true;
	public static bool isPaused { get; private set; } = false;


	private void Awake() {
		Control.OnEscapePressed += OnEscapePressed;
		Player.OnPlayerDeath += OnPlayerDeath;
	}

	private void OnEscapePressed() {
		if (canPause) {
			isPaused ^= true;
			OnPaused?.Invoke(null, new PauseEventArgs(isPaused, SceneManager.GetActiveScene().name));
		}
	}
	
	private void OnPlayerDeath(object sender, PlayerDeathEventArgs e) {
		isPaused = true;
		canPause = false;
		OnPaused?.Invoke(null, new PauseEventArgs(isPaused, SceneManager.GetActiveScene().name));
	}

	private void OnDestroy() {
		Control.OnEscapePressed += OnEscapePressed;
		Player.OnPlayerDeath -= OnPlayerDeath;
	}
}

