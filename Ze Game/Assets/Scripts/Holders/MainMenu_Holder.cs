using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_Holder : MonoBehaviour {
	public Toggle startGame;
	public Button loadSave;
	public Button leaderboard;
	public Button help;
	public Button quitGame;

	public Button[] getButtons {
		get {
			return new Button[] { leaderboard, loadSave, quitGame, help };
		}
	}
}

