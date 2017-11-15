using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

	public static UIScene sceneMode;

	public enum UIScene {
		MAIN_MENU,
		GAME,
		SAVES,
		OTHER
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void ListenForEscape() {
		Control.OnEscapePressed += Control_OnEscapePressed;
		sceneMode = UIScene.MAIN_MENU;
	}

	private static void Control_OnEscapePressed() {
		switch (sceneMode) {
			case UIScene.MAIN_MENU: {
				if (WindowManager.getWindowCount > 0) {
					WindowManager.CloseMostRecent();
				}
				return;
			}

			case UIScene.GAME: {
				if (WindowManager.getWindowCount > 0) {
					WindowManager.CloseMostRecent();
					try {
						EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
					}
					catch {
						print("No Buttons are selectable.");
					}
				}
				return;
			}

			case UIScene.SAVES: {

				return;
			}

			case UIScene.OTHER: {

				return;
			}

			default: {

				return;
			}
		}
	}

	private void Update() {
		switch (sceneMode) {
			case UIScene.MAIN_MENU: {
				if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) {
					EventSystem.current.SetSelectedGameObject(null);
				}
				break;
			}
			case UIScene.GAME: {
				break;
			}
			case UIScene.SAVES: {
				break;
			}
			case UIScene.OTHER: {
				break;
			}
			default: {
				break;
			}
		}
	}
}
