using UnityEngine;

public class Buttons : MonoBehaviour {

	public void StartNewGame(int difficulty) {
		Control.script.StartNewGame(difficulty);
		MenuMusic.script.StopMusicWrapper();
	}

	public void SaveGame(bool createNew) {
		Control.script.saveManager.Save(SaveManager.current.data.core.difficulty, createNew);
	}

	public void LoadGame(Transform myParent) {
		Control.script.loadManager.Load(myParent.name);
		MenuMusic.script.StopMusicWrapper();
	}

	public void Restart() {
		Control.script.Restart();
	}

	public void CreateProfile(string p_name) {
		Control.currProfile = new Profile().Create(p_name);
	}

	public void Quit() {
	#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
	#else
		Application.Quit();
	#endif
	}
}
