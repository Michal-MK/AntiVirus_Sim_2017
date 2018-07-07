using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {
	public static event PauseUnpause.Pause OnPauseChange;

	public static UIScene sceneMode { get; set; }
	public MainMenuRefs mmReferences;

	public enum UIScene {
		MAIN_MENU,
		GAME,
		SAVES,
		OTHER
	}

	protected virtual void Awake() {
		M_Player.OnPlayerDeath += M_Player_OnPlayerDeath;
		Control.OnEscapePressed += Control_OnEscapePressed;
		sceneMode = UIScene.MAIN_MENU;
	}

	private void Control_OnEscapePressed() {
		switch (sceneMode) {
			case UIScene.MAIN_MENU: {
				if (WindowManager.getWindowCount > 0) {
					WindowManager.CloseMostRecent();
					foreach (Button b in mmReferences.getButtons) {
						b.interactable = !b.interactable;
					}
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
				else {
					if (OnPauseChange != null && M_Player.gameProgression != -1) {
						OnPauseChange(!PauseUnpause.isPaused);
					}
				}
				return;
			}
		}
	}

	private void Update() {
		switch (sceneMode) {
			case UIScene.MAIN_MENU: {
				if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) {
					EventSystem.current.gameObject.GetComponent<EventSystemManager>().TryDeselect();
				}
				if ((Input.GetAxisRaw("HorMovement") != 0 || Input.GetAxisRaw("VertMovement") != 0) && EventSystem.current.currentSelectedGameObject == null) {
					EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
				}
				return;
			}
		}
	}

	#region GameScene functions
	private void M_Player_OnPlayerDeath(M_Player sender) {
		Animator gameOverAnim = GameObject.Find("GameOver").GetComponent<Animator>();
		gameOverAnim.Play("GameOver");
		M_Player.OnPlayerDeath -= M_Player_OnPlayerDeath;
	}
	#endregion

	private void OnDestroy() {
		Control.OnEscapePressed -= Control_OnEscapePressed;
	}
}
