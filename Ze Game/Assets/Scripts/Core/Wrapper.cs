using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrapper : MonoBehaviour {
	private void Awake() {
		Statics.wrapper	= this;
	}

	public void SaveGame() {
		Control.script.Save(true);
	}
	public void LoadGame(Transform Parrent) {
		Control.script.Load(Parrent.name);
	}
	private void OnDestroy() {
		Statics.wrapper = null;
	}
}
