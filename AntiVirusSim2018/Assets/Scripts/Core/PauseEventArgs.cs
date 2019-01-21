using System;

public class PauseEventArgs : EventArgs {

	/// <summary>
	/// Is the game in a paused state
	/// </summary>
	public bool isPaused { get; }

	/// <summary>
	/// Is the game resumed
	/// </summary>
	public bool isPlaying => !isPaused;

	/// <summary>
	/// The scene on which the pause occured
	/// </summary>
	public string currentSceneName { get; }

	public PauseEventArgs(bool isPaused, string currentSceneName) {
		this.isPaused = isPaused;
		this.currentSceneName = currentSceneName;
	}
}