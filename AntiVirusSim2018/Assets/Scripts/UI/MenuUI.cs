using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Igor.Constants.Strings;

public class MenuUI : MonoBehaviour {

	#region Lifecycle

	private void Awake() {
		Control.OnEscapePressed += Control_OnEscapePressed;
	}

	private void Update() {
		if (Input.GetAxisRaw(InputNames.MOUSE_X) != 0 || Input.GetAxisRaw(InputNames.MOUSE_Y) != 0) {
			EventSystem.current.gameObject.GetComponent<EventSystemManager>().TryDeselect();
		}
		if ((Input.GetAxisRaw(InputNames.MOVEMENT_HORIZONTAL) != 0 || Input.GetAxisRaw(InputNames.MOVEMENT_VERTICAL) != 0) && EventSystem.current.currentSelectedGameObject == null) {
			EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
		}
	}

	private void OnDestroy() {
		Control.OnEscapePressed -= Control_OnEscapePressed;
	}

	#endregion

	private void Control_OnEscapePressed() {
		MainMenuRefHolder refs = GetComponent<MainMenuRefHolder>();
		if (WindowManager.getWindowCount > 0) {
			WindowManager.CloseMostRecent();
			foreach (Button b in refs.All) {
				b.interactable = !b.interactable;
			}
			EventSystem.current.SetSelectedGameObject(refs.startGame.gameObject);
		}
	}
}
