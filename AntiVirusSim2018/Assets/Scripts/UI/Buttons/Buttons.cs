using UnityEngine;

public class Buttons : MonoBehaviour {

	public void StartNewGame(int difficulty) {
		Control.Instance.StartNewGame(difficulty);
	}

	public void LoadGame(Transform myParent) {
		Control.Instance.loadManager.Load(myParent.name);
	}

	public void Restart() {
		Control.Instance.Restart();
	}

	public void Quit() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
