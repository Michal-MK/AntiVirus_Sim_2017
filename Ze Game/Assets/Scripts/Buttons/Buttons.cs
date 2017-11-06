using UnityEngine;

public class Buttons: MonoBehaviour {

	public void SaveGame(bool createNew) {
		Control.script.saveManager.Save(createNew);
	}

	public void LoadGame(Transform myParent) {
		Control.script.loadManager.Load(myParent.name);
	}
}
