using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {
	
	/// <summary>
	/// Stack of active windows, wince windows tend to "Stack up" may be switched for a list  in the future to allow for removing from the middle.
	/// </summary>
	private static Stack<Window> activeWindows = new Stack<Window>();

	private static bool isShown = false;

	public delegate void WindowChangedHandler(Window changed);
	public static event WindowChangedHandler OnWindowClose;

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
	}

	[Obsolete("Use the more specific version of \"AddWindow\" whenever possible.")]
	public static void AddWindow(GameObject win) {
		Window.WindowType t;
		switch (win.name) {
			default: {
				t = Window.WindowType.ACTIVATING;
				break;
			}
		}
		Window w = new Window(win, t);
		AddWindow(w);
	}

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
			print("Invalid count " + count + " is bigger than all active windows " + activeWindows.Count);
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

	public static void ToggleWindow(GameObject window) {
		Animator anim;
		try {
			anim = window.GetComponent<Animator>();
			if (isShown) {
				anim.SetTrigger("Hide");
				isShown = false;
			}
			else {
				anim.SetTrigger("Show");
				isShown = true;
			}
		}
		catch (NullReferenceException e) {
			print(e.Source + " No Animator Present");
		}
	}

	public static void ToggleWindow(Window win) {
		if (win.type == Window.WindowType.ACTIVATING) {
			win.window.SetActive(!win.window.activeInHierarchy);
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

	public void ToggleWindowWrapper(GameObject g) {
		ToggleWindow(g);
	}

	public static void CloseAllActive() {
		foreach (Window win in activeWindows) {
			if (win.type == Window.WindowType.ACTIVATING) {
				win.window.SetActive(false);
			}
			else {
				print(win.window.name);
			}
		}
		activeWindows = new Stack<Window>();
	}

	public static void CloseWindow(GameObject window) {
		foreach (Window w in activeWindows) {
			if (w.window == window) {

				if (w.type == Window.WindowType.ACTIVATING) {
					w.window.SetActive(false);
				}
				else {
					w.animator.SetTrigger("Hide");
				}
				activeWindows.Pop();
				break;
			}
		}
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
