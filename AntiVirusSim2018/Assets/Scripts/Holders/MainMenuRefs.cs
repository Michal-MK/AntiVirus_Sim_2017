using UnityEngine;
using UnityEngine.UI;

public class MainMenuRefs : MonoBehaviour {

	#region Buttons
	public Toggle startGame;
	public Button loadSave;
	public Button leaderboard;
	public Button settings;
	public Button help;
	public Button quitGame;
	#endregion

	public Button[] getButtons {
		get {
			return new Button[] { leaderboard, loadSave, quitGame, help, settings };
		}
	}
}

