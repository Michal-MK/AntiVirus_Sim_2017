using System;
using UnityEngine;

public class SettingsJoiner : MonoBehaviour {

	public void OnEnable() {
		GameObject canvas = GameObject.Find("Canvas");
		canvas.SetActive(false);
		GameSettings.script.Attach(this, canvas);
	}
}

