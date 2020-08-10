using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WindowBehaviour : MonoBehaviour {

	private Button self;
	private Window windowRef;
	private GameScene_Holder gameHolder;

	public bool findSelectableInWindow;
	public bool isSpecialityWindow;
	public bool destroyWindow;

	private void Start() {
		self = GetComponent<Button>();
		gameHolder = FindObjectOfType<GameScene_Holder>();
	}

	public void ToggleWindow(GameObject window) {
		if (window.activeInHierarchy) {
			WindowManager.CloseMostRecent();
		}
		else {
			Window w = new Window(window, Window.WindowType.ACTIVATING);
			w.window.SetActive(true);
			windowRef = w;
			WindowManager.AddWindow(w);
			if (!UIControlScheme.Instance.IsMouseScheme() && findSelectableInWindow) {
				EventSystem.current.SetSelectedGameObject(w.window.GetComponentInChildren<Selectable>().gameObject);
			}
			WindowManager.OnWindowClose += WindowManager_OnWindowClose;
		}
	}


	private GameObject spcialityGameObject;
	public void SetupSpecialityWindow(int speciality) {
		switch ((SpecialityWindow)speciality) {
			case SpecialityWindow.SETTINGS_PANNEL: {
				spcialityGameObject = GameObject.Find("Canvas");
				break;
			}
			default: {
				throw new Exception("Invalid Input " + speciality);
			}
		}
	}

	public void ToggleWindowSpecial(GameObject window) {
		Window w;
		switch (specialityWindow) {
			case SpecialityWindow.SETTINGS_PANNEL: {
				w = new Window(window, Window.WindowType.ACTIVATING, delegate { spcialityGameObject.SetActive(true); });
				break;
			}
			default: {
				throw new Exception();
			}
		}
		w.window.SetActive(true);
		windowRef = w;
		WindowManager.AddWindow(w);
		if (!UIControlScheme.Instance.IsMouseScheme() && findSelectableInWindow) {
			EventSystem.current.SetSelectedGameObject(w.window.GetComponentInChildren<Selectable>().gameObject);
		}
		WindowManager.OnWindowClose += WindowManager_OnWindowClose;
	}

	private void WindowManager_OnWindowClose(Window changed) {
		if (changed == windowRef) {
			if (!UIControlScheme.Instance.IsMouseScheme() && findSelectableInWindow) {
				EventSystem.current.SetSelectedGameObject(self.gameObject);
			}
			self.OnSelect(null);
			changed.closeAction?.Invoke();
			WindowManager.OnWindowClose -= WindowManager_OnWindowClose;
		}
	}

	private bool savePromptActive = false;
	public void SavePrompt() {
		if (savePromptActive) {
			Destroy(windowRef.window);
			savePromptActive = false;
			return;
		}

		savePromptActive = true;
		windowRef = Notifications.Confirm("Do you wish to save in this place?", true,
			(b) => { 
				Control.script.saveManager.Save(false);
				self.interactable = false;
				transform.Find("_saveGameText").GetComponent<Text>().text = "Saved!";
			},
			() => {
				WindowManager.CloseMostRecent();
				EventSystem.current.SetSelectedGameObject(self.gameObject);
				self.OnSelect(null);
				savePromptActive = false;
				gameHolder.SavePromptToggle();
			});
		if (!UIControlScheme.Instance.IsMouseScheme() && findSelectableInWindow) {
			EventSystem.current.SetSelectedGameObject(windowRef.window.GetComponentInChildren<Selectable>().gameObject);
		}
		gameHolder.SavePromptToggle();
	}


	[Serializable]
	public enum SpecialityWindow {
		SETTINGS_PANNEL,
	}

	public SpecialityWindow specialityWindow;
}
