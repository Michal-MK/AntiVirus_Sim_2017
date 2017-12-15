using UnityEngine;
using UnityEngine.UI;
using Igor.Constants.Strings;

public class Buttons : MonoBehaviour {

	public void StartNewGame(int difficulty) {
		Control.script.StartNewGame(difficulty);
		MenuMusic.script.StopMusic();
	}

	public void SaveGame(bool createNew) {
		Control.script.saveManager.Save(SaveManager.current.data.core.difficulty, createNew);
	}

	public void LoadGame(Transform myParent) {
		Control.script.loadManager.Load(myParent.name);
		MenuMusic.script.StopMusic();
	}

	public void Restart() {
		Control.script.Restart();
	}

	public void CreateProfile(string p_name) {
		Control.currProfile = Profile.Create(p_name);
	}

	public void Quit() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void ToggleWindowActive(GameObject window) {
		if (Profile.getCurrentProfile == null) {
			Notifications.Confirm("You need to create a profile first", true,
				delegate { Profile.RequestProfiles(); },
				delegate { FindObjectOfType<UserInterface>().ToggleMenuButtons(); }
			);
			return;
		}
		WindowManager.ToggleWindow(new Window(window, Window.WindowType.ACTIVATING));
		WindowManager.OnWindowClose += WindowManager_OnWindowClose;
	}

	private void WindowManager_OnWindowClose(Window changed) {
		if (changed.window.name == ObjNames.MENU_CHOOSE_DIFFICULTY) {
			FindObjectOfType<UserInterface>().ToggleMenuButtons();
		}
		WindowManager.OnWindowClose -= WindowManager_OnWindowClose;
	}

	public void ToggleWindowAnim(GameObject window) {
		WindowManager.ToggleWindow(new Window(window, Window.WindowType.MOVING));
	}
}
