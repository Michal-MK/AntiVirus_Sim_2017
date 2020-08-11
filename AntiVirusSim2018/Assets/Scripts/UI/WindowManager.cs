using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager {

	/// <summary>
	/// Stack of active windows
	/// </summary>
	private static Stack<Window> activeWindows = new Stack<Window>();

	public delegate void WindowChangedHandler(Window changed);
	/// <summary>
	/// Called when a window closes, null when closing everything
	/// </summary>
	public static event WindowChangedHandler OnWindowClose;
	/// <summary>
	/// Called when a new window is open
	/// </summary>
	public static event WindowChangedHandler OnWindowOpen;

	/// <summary>
	/// Adds a new window to the stack
	/// </summary>
	/// <param name="win">New window</param>
	public static void AddWindow(Window win) {
		activeWindows.Push(win);
		if (win.type == Window.WindowType.ACTIVATING) {
			if (!win.window.activeInHierarchy) {
				win.window.SetActive(true);
			}
		}
		else {
			win.window.SetActive(true);
			if (!win.animator.GetCurrentAnimatorStateInfo(0).IsName("Show")) {
				win.animator.SetTrigger("Show");
			}
		}
		if (OnWindowOpen != null) {
			OnWindowOpen(win);
		}
	}
	/// <summary>
	/// Closes the most recent window
	/// </summary>
	public static void CloseMostRecent() {
		if (activeWindows.Count > 0) {
			Window win = activeWindows.Pop();
			if (win != null) {
				if (win.type == Window.WindowType.ACTIVATING) {
					win.window.SetActive(false);
				}
				else {
					win.animator.SetTrigger("Hide");
					if (win.isFlagedForSwitchOff) {
						Control.Instance.StartCoroutine(DisableAfterAnimation(win.animator));
					}
				}
				if (OnWindowClose != null) {
					OnWindowClose(win);

				}
			}
			else {
				//Would  pause the game
			}
		}
		else {
			//Would pause the game
		}
	}
	/// <summary>
	/// Closes the most recent windows
	/// </summary>
	/// <param name="count">How many.</param>
	public static void CloseMostRecent(int count) {
		if (activeWindows.Count >= count) {
			for (int i = 0; i < count; i++) {
				Window win = activeWindows.Pop();
				if (win != null) {
					if (win.type == Window.WindowType.ACTIVATING) {
						win.window.SetActive(false);
					}
					else {
						win.animator.SetTrigger("Hide");
						if (win.isFlagedForSwitchOff) {
							Control.Instance.StartCoroutine(DisableAfterAnimation(win.animator));
						}
					}
					if (OnWindowClose != null) {
						OnWindowClose(win);
					}
				}
				else {
					//Would pause the game
				}
			}
		}
		else {
			Debug.Log("Invalid count " + count + " is bigger than all active windows " + activeWindows.Count);
			//Would pause the game
		}
	}

	private static IEnumerator DisableAfterAnimation(Animator win) {
		win.SetTrigger("Hide");
		yield return new WaitForSecondsRealtime(win.GetCurrentAnimatorClipInfo(0)[0].clip.length - 0.05f);

		if (win.GetCurrentAnimatorStateInfo(0).IsName("Hide")) {
			win.gameObject.SetActive(false);
		}
	}
	/// <summary>
	/// Enables / Disables a window.
	/// </summary>
	/// <param name="win"></param>
	public static void ToggleWindow(Window win) {
		if (win.type == Window.WindowType.ACTIVATING) {
			win.window.SetActive(!win.window.activeInHierarchy);
			if (win.window.activeInHierarchy) {
				activeWindows.Push(win);
				if (OnWindowOpen != null) {
					OnWindowOpen(win);
				}
			}
			else {
				activeWindows.Pop();
				if (OnWindowClose != null) {
					OnWindowClose(win);
				}
			}
		}
		else {
			AnimatorStateInfo s = win.animator.GetCurrentAnimatorStateInfo(0);
			if (s.IsName("Show")) {
				win.animator.SetTrigger("Hide");
			}
			else if (s.IsName("Hide")) {
				win.animator.SetTrigger("Show");
			}
		}
	}

	/// <summary>
	/// Clears the screen of any open windows.
	/// </summary>
	public static void CloseAllActive() {
		foreach (Window win in activeWindows) {
			if (win.type == Window.WindowType.ACTIVATING) {
				win.window.SetActive(false);
			}
			else {
				Debug.Log(win.window.name);
			}
		}
		activeWindows = new Stack<Window>();
		if (OnWindowClose != null) {
			OnWindowClose(null);
		}
	}

	/// <summary>
	/// Loops though all active windows and closes matching one.
	/// </summary>
	/// <param name="window"></param>
	public static void CloseWindow(GameObject window) {
		foreach (Window w in activeWindows) {
			if (w.window == window) {

				if (w.type == Window.WindowType.ACTIVATING) {
					w.window.SetActive(false);
				}
				else {
					w.animator.SetTrigger("Hide");
				}
				activeWindows.Remove(w);
				break;
			}
		}
		Debug.LogWarning("No window " + window + " found.");
	}

	/// <summary>
	/// Removes the content of activeWindows list
	/// </summary>
	public static void ClearWindows() {
		CloseAllActive();
		activeWindows.Clear();
	}


	public static Window[] getWindowArray {
		get { return activeWindows.ToArray(); }
	}

	public static int getWindowCount {
		get {
			return activeWindows.Count;
		}
	}
}

