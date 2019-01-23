using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsJoiner : MonoBehaviour {

	public void OnEnable() {
		GameObject canvas = GameObject.Find("Canvas");
		canvas.SetActive(false);
		GameSettings.script.fromGame = true;
		GameSettings.script.Attach(this, canvas);
		EventSystem.current.GetComponent<EventSystemManager>().enabled = true;
	}
}

