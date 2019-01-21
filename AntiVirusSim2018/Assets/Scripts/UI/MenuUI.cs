using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {

	private void Awake() {
		Control.OnEscapePressed += Control_OnEscapePressed;
	}

	private void Control_OnEscapePressed() {
		MainMenuRefs refs = GetComponent<MainMenuRefs>();
		if (WindowManager.getWindowCount > 0) {
			WindowManager.CloseMostRecent();
			foreach (Button b in refs.getAllButtons) {
				b.interactable = !b.interactable;
			}
			EventSystem.current.SetSelectedGameObject(refs.startGame.gameObject);
		}
	}

	private void Update() {
		if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) {
			EventSystem.current.gameObject.GetComponent<EventSystemManager>().TryDeselect();
		}
		if ((Input.GetAxisRaw("HorMovement") != 0 || Input.GetAxisRaw("VertMovement") != 0) && EventSystem.current.currentSelectedGameObject == null) {
			EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
		}
	}

	private void OnDestroy() {
		Control.OnEscapePressed -= Control_OnEscapePressed;
	}
}
