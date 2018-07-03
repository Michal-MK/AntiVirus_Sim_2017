using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsJoiner : MonoBehaviour {

	public void OnEnable() {
		GameObject canvas = GameObject.Find("Canvas");
		canvas.SetActive(false);
		GameSettings.script.fromGame = true;
		WindowManager.AddWindow(new Window(gameObject, Window.WindowType.ACTIVATING));
		GameSettings.script.Attach(this, canvas);
		EventSystem.current.GetComponent<EventSystemManager>().enabled = true;
	}
}

