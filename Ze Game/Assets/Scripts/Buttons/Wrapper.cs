using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Wrapper : MonoBehaviour {
	public Wrapper canvas;
	public GameObject dim;
	public InputField field;
	public Toggle save;
	public Toggle start;
	public Button[] buttons = new Button[4];
	public GameObject[] Objects = new GameObject[4];

	private void Awake() {
		Statics.wrapper = this;
	}

	private void Start() {
		if (field != null) {
			field.onValidateInput += delegate (string text, int index, char ch) { return Validate(ch); };
			dim = GameObject.Find("Dim");
			dim.GetComponent<Image>().color = new Color32(0, 0, 0, 150);
			canvas = GameObject.Find("Canvas").GetComponent<Wrapper>();
			canvas.DisableButtons();
		}
	}

	public void StartNewGame(int difficulty) {
		Control.script.isNewGame = true;
		Control.script.StartCoroutine(Control.script.StartNewGame(difficulty));
	}

	public void SetPCName(InputField name) {
		PlayerPrefs.SetString("player_name", name.text);
	}

	public char Validate(char ch) {
		if (ch == '$' || ch == '~' || ch == '@' || ch == '_' || ch == '#') {
			ch = '\0';
		}
		return ch;
	}

	public void UpdateName() {
		Statics.profile.DisplayProfile();
		if (dim != null) {
			dim.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
			if (PlayerPrefs.GetString("player_name") == null) {
				print("AAAA");
				canvas.DisableButtons();
			}
		}
	}

	public void SaveGame(bool createNew) {
		Control.script.Save(createNew);
	}

	public void LoadGame(Transform Parrent) {
		Control.script.Load(Parrent.name);
	}

	public void SelectButton(int i = 0) {
		print("Deselect");
		EventSystem e = EventSystem.current;
		if (buttons[i].gameObject != null) {
			e.SetSelectedGameObject(buttons[i].gameObject);	
		}
		else {
			e.SetSelectedGameObject(buttons[1].gameObject);
		}
	}
	public void SelectGameObject(int i = 0) {
		EventSystem e = EventSystem.current;
		if (Objects[i] != null && Objects[i].activeInHierarchy) {
			e.SetSelectedGameObject(Objects[i].gameObject);
		}
		else if (start != null) {
			e.SetSelectedGameObject(start.gameObject);
		}
	}

	public void DisableButtons() {
		if (SceneManager.GetActiveScene().buildIndex == 0 && gameObject.name == "startGame") {
			foreach (Button b in buttons) {
				b.interactable = !b.interactable;

			}
		}
	}

	public void DeactivateButtons() {
		print("READY");
		DeactivateButtonsCoroutine();
	}

	public void DeactivateObjects() {
		foreach (var item in Objects) {
			item.SetActive(!item.activeInHierarchy);
		}
	}


	private void DeactivateButtonsCoroutine() {
		foreach (Button item in GameObject.Find("Canvas").GetComponentsInChildren<Button>()) {
			//print(item.name);
			item.gameObject.SetActive(!item.gameObject.activeInHierarchy);
		}
		foreach (Toggle item in GameObject.Find("Canvas").GetComponentsInChildren<Toggle>()) {
			print(item.name);
			item.gameObject.SetActive(!item.gameObject.activeInHierarchy);
		}
		Control.script.Restart();
	}

	public void SetSaving(bool state) {
		save.interactable = state;
	}

	private void Update() {
		if (SceneManager.GetActiveScene().buildIndex != 1) {
			EventSystem e = EventSystem.current;
			if (e.currentSelectedGameObject == null) {
				if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f) {
					if (!e.alreadySelecting) {
						e.SetSelectedGameObject(buttons[0].gameObject);
					}
				}
			}
		}

		if (SceneManager.GetActiveScene().buildIndex != 1 && dim != null && field != null && canvas != null) {
			if (Input.GetButtonDown("Submit")) {
				GameObject startG = GameObject.Find("startGame");
				PlayerPrefs.SetString("player_name", field.text);
				canvas.DisableButtons();
				dim.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
				UpdateName();
				gameObject.transform.parent.gameObject.SetActive(false);
				EventSystem.current.SetSelectedGameObject(startG);
			}
		}
	}

	private void OnDestroy() {
		Statics.wrapper = null;
	}
}
