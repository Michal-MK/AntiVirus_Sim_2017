using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Wrapper : MonoBehaviour {
	public Toggle save;
	public Button[] buttons = new Button[4];

	private void Awake() {
		Statics.wrapper = this;
	}
	public void StartNewGame(int difficulty) {
		Control.script.isNewGame = true;
		Control.script.StartCoroutine(Control.script.StartNewGame(difficulty));
	}


	public void SaveGame(bool createNew) {
		Control.script.Save(createNew);

	}
	public void LoadGame(Transform Parrent) {
		Control.script.Load(Parrent.name);
	}

	public void DisableButtons() {
		if (buttons.Length != 0/* && SceneManager.GetActiveScene().buildIndex == 1*/) {
			foreach (Button item in buttons) {
				if (item != null) {
					item.interactable = !item.interactable;
				}
			}
		}
		else {
			foreach (Button item in GameObject.Find("Canvas").GetComponentsInChildren<Button>()) {
				item.interactable = false;
			}
		}
	}
	public void DeactivateButtons() {
		print("READY");
		DeactivateButtonsCoroutine();
	}

	private void DeactivateButtonsCoroutine() {
		foreach (Button item in GameObject.Find("Canvas").GetComponentsInChildren<Button>()) {
			print(item.name);
			item.gameObject.SetActive(!item.gameObject.activeInHierarchy);
		}
		foreach (Toggle item in GameObject.Find("Canvas").GetComponentsInChildren<Toggle>()) {
			print(item.name);
			item.gameObject.SetActive(!item.gameObject.activeInHierarchy);
		}
		Control.script.Restart();
	}

	public void EnagleSaving(bool enable) {
		save.interactable = enable;
	}

	private void OnDestroy() {
		Statics.wrapper = null;
	}
}
