using UnityEngine;
using UnityEngine.UI;
using Igor.Constants.Strings;

public class Buttons : MonoBehaviour {

	public void StartNewGame(int difficulty) {
		Control.script.StartNewGame(difficulty);
		//MenuMusic.script.StopMusic();
	}

	public void SavePrompt() {
		Notifications.Confirm("Do you wish to save in this place?", true, 
			delegate {
				Control.script.saveManager.Save(SaveManager.current.data.core.difficulty,false);
				GetComponent<Button>().interactable = false;
				transform.Find("saveGameText").GetComponent<Text>().text = "Saved!";
			},
			delegate { WindowManager.CloseMostRecent(); }
		);
	}

	public void LoadGame(Transform myParent) {
		Control.script.loadManager.Load(myParent.name);
	}

	public void Restart() {
		Control.script.Restart();
	}

	public void Quit() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void ToggleWindowActive(GameObject window) {
		WindowManager.ToggleWindow(new Window(window, Window.WindowType.ACTIVATING));
	}

	public void ToggleWindowAnim(GameObject window) {
		WindowManager.ToggleWindow(new Window(window, Window.WindowType.MOVING));
	}
}
