using UnityEngine;
using UnityEngine.UI;

public class GameSceneRefHolder : MonoBehaviour {

	public Button save;
	public Button restart;
	public Button quitToMenu;
	public Button load;
	public Button settings;


	public Button[] getButtonsNonSpecial {
		get { return new Button[] { restart, settings, quitToMenu }; }
	}

	public Button[] getButtonsSave {
		get { return new Button[] { save, restart, settings, quitToMenu }; }
	}

	public Button[] getButtonsLoad {
		get { return new Button[] { load, restart, settings, quitToMenu }; }
	}

	public void SavePromptToggle() {
		foreach (Button button in getButtonsNonSpecial) {
			button.interactable = !button.interactable;
		}
	}
}
