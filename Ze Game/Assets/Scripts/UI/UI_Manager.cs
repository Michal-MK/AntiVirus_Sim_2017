using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager {
	
	/// <summary>
	/// Stack of active windows, since windows tend to "Stack up" may be switched for a list in the future to allow for removing from the middle.
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
	/// <param name="win"></param>
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
		if(OnWindowOpen != null) {
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
						Control.script.StartCoroutine(DisableAfterAnimation(win.animator));
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
							Control.script.StartCoroutine(DisableAfterAnimation(win.animator));
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
			if (win.window.activeInHierarchy && OnWindowOpen != null) {
				OnWindowOpen(win);
			}
			else if(OnWindowClose != null) {
				OnWindowClose(win);
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

	public static int getWindowCount {
		get {
			return activeWindows.Count;
		}
	}
}

public class Window {

	public enum WindowType {
		MOVING,
		ACTIVATING,
	}

	public Window(GameObject window, WindowType type, bool disableAfterMoving = false) {
		_window = window;
		_type = type;
		if (type == WindowType.MOVING) {
			_anim = window.GetComponent<Animator>();
			_disableAfterMoving = disableAfterMoving;
		}
	}

	private GameObject _window;
	private WindowType _type;
	private Animator _anim;
	private bool _disableAfterMoving;

	public GameObject window {
		get { return _window; }
	}

	public WindowType type {
		get { return _type; }
	}

	public Animator animator {
		get { return _anim; }
	}

	public bool isFlagedForSwitchOff {
		get { return _disableAfterMoving; }
	}
}
public static class WindowExtensions {
	public static void Remove(this Stack<Window> stack, Window element) {
		Window[] wins = stack.ToArray();
		for (int i = 0; i < wins.Length; i++) {
			if (wins[i] == element) {
				wins[i] = null;
				break;
			}
		}
		stack.Clear();
		for (int i = 0; i < wins.Length; i++) {
			if (wins[i] != null) {
				stack.Push(wins[i]);
			}
		}
	}
}


