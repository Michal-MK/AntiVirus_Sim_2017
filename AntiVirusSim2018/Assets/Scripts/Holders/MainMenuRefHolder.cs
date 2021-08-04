using UnityEngine;
using UnityEngine.UI;

public class MainMenuRefHolder : MonoBehaviour {

	#region Buttons
	public Button startGame;
	public Button loadSave;
	public Button leaderboard;
	public Button settings;
	public Button help;
	public Button quitGame;
	#endregion


	public Button[] All {
		get {
			return new Button[] { startGame, leaderboard, loadSave, quitGame, help, settings };
		}
	}

	public void ToggleButtons() {
		foreach (Button b in All) {
			b.interactable = !b.interactable;
		}
	}
}

