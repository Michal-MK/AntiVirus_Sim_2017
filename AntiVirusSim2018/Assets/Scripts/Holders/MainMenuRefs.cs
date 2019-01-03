using UnityEngine;
using UnityEngine.UI;

public class MainMenuRefs : MonoBehaviour {

	#region Buttons
	public Button startGame;
	public Button loadSave;
	public Button leaderboard;
	public Button settings;
	public Button help;
	public Button quitGame;
	#endregion


	public Button[] getAllButtons {
		get {
			return new Button[] { startGame, leaderboard, loadSave, quitGame, help, settings };
		}
	}

	public void ToggleButtons() {
		foreach (Button b in getAllButtons) {
			b.interactable = !b.interactable;
		}
	}
}

