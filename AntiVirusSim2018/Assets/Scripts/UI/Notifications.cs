using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles displaying <see cref="Notifications"/> on current <see cref="Scene"/>s canvas.
/// </summary>
public class Notifications : MonoBehaviour {
	public GameObject notificationPrefab;
	public GameObject warningPrefab;
	public GameObject confirmationPrefab;
	public RectTransform canvas;

	private static GameObject sNnotificationPrefab;
	private static GameObject sWarningPrefab;
	private static GameObject sConfirmationPrefab;
	private static RectTransform sCanvas;

	private void Awake() {
		SceneManager.sceneLoaded += OnSceneLoad;
		sNnotificationPrefab = notificationPrefab;
		sWarningPrefab = warningPrefab;
		sConfirmationPrefab = confirmationPrefab;
	}

	private void OnSceneLoad(Scene scene, LoadSceneMode mode) {
		if (sCanvas == null) {
			sCanvas = canvas;
		}
	}

	public static bool NotificationActive { get; private set; }


	public static Window Warn<T>(string msg, T value, Action<T> confirmation, Action returnBack) {
		GameObject w = Instantiate(sWarningPrefab, sCanvas, false);
		Button ok = w.transform.Find("Ok").GetComponent<Button>();
		Button back = w.transform.Find("Back").GetComponent<Button>();
		Text message = w.transform.Find("Warning").GetComponent<Text>();
		message.text = msg;
		EventSystem.current.SetSelectedGameObject(ok.gameObject);
		WindowManager.AddWindow(new Window(w, Window.WindowType.ACTIVATING));
		ok.onClick.AddListener(delegate { Destroy(w); confirmation.Invoke(value); });
		back.onClick.AddListener(delegate { Destroy(w); returnBack.Invoke(); });
		NotificationActive = true;
		return new Window(w, Window.WindowType.ACTIVATING, returnBack);
	}

	public static Window Notify<T>(string msg) {
		GameObject w = Instantiate(sNnotificationPrefab, sCanvas, false);
		Button ok = w.transform.Find("Ok").GetComponent<Button>();
		Text message = w.transform.Find("Notification").GetComponent<Text>();
		message.text = msg;
		EventSystem.current.SetSelectedGameObject(ok.gameObject);
		WindowManager.AddWindow(new Window(w, Window.WindowType.ACTIVATING));
		ok.onClick.AddListener(delegate { Destroy(w); });
		return new Window(w, Window.WindowType.ACTIVATING);
	}

	public static Window Confirm(string msg, bool value, Action<bool> confirmation, Action returnBack) {
		GameObject w = Instantiate(sConfirmationPrefab, sCanvas, false);
		Button ok = w.transform.Find("Ok").GetComponent<Button>();
		Button back = w.transform.Find("Back").GetComponent<Button>();
		Text message = w.transform.Find("Confirmation").GetComponent<Text>();
		message.text = msg;
		EventSystem.current.SetSelectedGameObject(ok.gameObject);
		WindowManager.AddWindow(new Window(w, Window.WindowType.ACTIVATING));
		ok.onClick.AddListener(delegate { Destroy(w); confirmation.Invoke(value); });
		back.onClick.AddListener(delegate { Destroy(w); returnBack.Invoke(); });
		NotificationActive = true;
		return new Window(w, Window.WindowType.ACTIVATING, returnBack);
	}
}
