using System;

public class PauseEventArgs : EventArgs {

	/// <summary>
	/// Is the game in a paused state
	/// </summary>
	public bool IsPaused { get; }

	/// <summary>
	/// Is the game resumed
	/// </summary>
	public bool IsPlaying => !IsPaused;

	/// <summary>
	/// The scene on which the pause occurred
	/// </summary>
	public string CurrentSceneName { get; }

	public PauseEventArgs(bool isPaused, string currentSceneName) {
		IsPaused = isPaused;
		CurrentSceneName = currentSceneName;
	}
}