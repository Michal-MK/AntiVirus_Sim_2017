using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProfileName : MonoBehaviour {

	private void Awake() {
		Statics.profile = this;
	}

	private void Start() {
		if (PlayerPrefs.GetString("player_name") == null || PlayerPrefs.GetString("player_name") == "" && SceneManager.GetActiveScene().buildIndex == 0) {
			Control.script.StartCoroutine(Control.script.SetName());
		}
		else if (gameObject.GetComponent<Text>() != null && gameObject.GetComponent<Text>().text != "Current profile: ") {
			DisplayProfile();
		}

	}

	public void DisplayProfile() {
		string name = PlayerPrefs.GetString("player_name");
		if (name == "" || name == null) {
		}
		else {
			gameObject.GetComponent<Text>().text += name;

		}
	}
	public void OnPress() {
		PlayerPrefs.DeleteKey("player_name");
	}

	private void OnDestroy() {
		Statics.profile = null;
	}
}
