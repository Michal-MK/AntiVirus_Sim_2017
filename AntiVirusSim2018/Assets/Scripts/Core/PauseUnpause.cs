using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUnpause : MonoBehaviour {

	public static event EventHandler<PauseEventArgs> OnPaused;

	public static bool CanPause { get; set; } = true;
	public static bool IsPaused { get; private set; } = false;


	private void Awake() {
		Control.OnEscapePressed += OnEscapePressed;
		Player.OnPlayerDeath += OnPlayerDeath;
	}

	private void OnEscapePressed() {
		if (CanPause) {
			IsPaused ^= true;
			OnPaused?.Invoke(null, new PauseEventArgs(IsPaused, SceneManager.GetActiveScene().name));
		}
	}
	
	private void OnPlayerDeath(object sender, PlayerDeathEventArgs e) {
		IsPaused = true;
		CanPause = false;
		OnPaused?.Invoke(null, new PauseEventArgs(IsPaused, SceneManager.GetActiveScene().name));
	}

	private void OnDestroy() {
		Control.OnEscapePressed += OnEscapePressed;
		Player.OnPlayerDeath -= OnPlayerDeath;
	}
}

