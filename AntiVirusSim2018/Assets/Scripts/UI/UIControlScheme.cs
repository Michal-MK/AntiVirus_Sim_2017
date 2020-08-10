using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIControlScheme : MonoBehaviour {

	private GameObject selectedGO;
	private GameObject initialSceneFocus;

	public bool Debug;

	public static UIControlScheme Instance { get; private set; }
	public ControlScheme LastControlScheme { get; set; } = ControlScheme.Mouse;
	public bool IsMouseScheme() => LastControlScheme == ControlScheme.Mouse;


	private void Awake() {
		if (Instance == null) {
			Instance = this;
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		}
		else {
			Destroy(this);
		}
	}

	private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadMode) {
		selectedGO = initialSceneFocus = EventSystem.current.firstSelectedGameObject;

		if (Debug)
			print("Scene loaded: " + initialSceneFocus.name + " and control: " + LastControlScheme);

		if (LastControlScheme == ControlScheme.Mouse) {
			EventSystem.current.firstSelectedGameObject = null;
			EventSystem.current.SetSelectedGameObject(null);
			selectedGO = null;
		}
	}


	private void Update() {
		bool mouseMoved = Input.GetAxis(Igor.Constants.Strings.InputNames.MOUSE_X) != 0 ||
						  Input.GetAxis(Igor.Constants.Strings.InputNames.MOUSE_Y) != 0;

		bool keyboardOrController = Input.GetAxis(Igor.Constants.Strings.InputNames.MOVEMENT_HORIZONTAL) != 0 ||
									Input.GetAxis(Igor.Constants.Strings.InputNames.MOVEMENT_VERTICAL) != 0;

		if (selectedGO != EventSystem.current.currentSelectedGameObject) {
			selectedGO = EventSystem.current.currentSelectedGameObject;
			if (selectedGO != null) {
				print(selectedGO.name);
			}
		}

		if (mouseMoved) {
			EventSystem.current.SetSelectedGameObject(null);
			selectedGO = null;
			LastControlScheme = ControlScheme.Mouse;
		}

		if (keyboardOrController) {
			if (selectedGO == null) {
				selectedGO = initialSceneFocus;
				EventSystem.current.SetSelectedGameObject(selectedGO);
				if (Debug)
					print("Keyboard/Controller: Selected ->" + selectedGO.name);
			}
			LastControlScheme = ControlScheme.Keyboard | ControlScheme.Controller;
		}
	}
}
