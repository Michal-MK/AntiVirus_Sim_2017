using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsJoiner : MonoBehaviour {

	[SerializeField]
	private Button settingsButton;

	public void OnEnable() {
		GameObject canvas = GameObject.Find("Canvas");
		canvas.SetActive(false);
		GameSettings.Instance.Attach(this, canvas, HandleSettings);
		EventSystem.current.GetComponent<EventSystemManager>().enabled = true;
	}

	private void HandleSettings() {
		//This hack is necessary to prevent the button from being stuck in highlighted mode after disabling it while pressed /./
		settingsButton.gameObject.SetActive(false);
		settingsButton.gameObject.SetActive(true);
	}
}

