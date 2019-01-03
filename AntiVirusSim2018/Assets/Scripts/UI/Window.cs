using System;
using System.Collections.Generic;
using UnityEngine;

public class Window {

	public enum WindowType {
		MOVING,
		ACTIVATING,
	}

	public Window(GameObject window, WindowType type, bool disableAfterMoving = false) : this(window, type, null) {
		if (type == WindowType.MOVING) {
			isFlagedForSwitchOff = disableAfterMoving;
		}
	}

	public Window(GameObject window, WindowType type, Action OnWindowClose) {
		this.window = window;
		this.type = type;
		if (type == WindowType.MOVING) {
			animator = window.GetComponent<Animator>();
		}
		closeAction = OnWindowClose;
	}

	public Action closeAction { get; }
	public GameObject window { get; }
	public WindowType type { get; }
	public Animator animator { get; }
	public bool isFlagedForSwitchOff { get; }
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
