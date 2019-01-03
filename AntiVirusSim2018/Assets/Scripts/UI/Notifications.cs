using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Notifications : MonoBehaviour {
	public GameObject notificationPrefab;
	public GameObject warningPrefab;
	public GameObject confirmationPrefab;
	public RectTransform canvas;

	private static GameObject _notificationPrefabStatic;
	private static GameObject _warningPrefabStatic;
	private static GameObject _confirmationPrefabStatic;
	private static RectTransform _canvas;

	private void Awake() {
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoad;
		_notificationPrefabStatic = notificationPrefab;
		_warningPrefabStatic = warningPrefab;
		_confirmationPrefabStatic = confirmationPrefab;
	}

	private void OnSceneLoad(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode) {
		if (_canvas == null) {
			_canvas = canvas;
		}
	}

	public static bool notificationActive { get; private set; }


	public static Window Warn<T>(string msg, T value, Action<T> confirmation, Action returnBack) {
		GameObject w = Instantiate(_warningPrefabStatic, _canvas, false);
		Button ok = w.transform.Find("Ok").GetComponent<Button>();
		Button back = w.transform.Find("Back").GetComponent<Button>();
		Text message = w.transform.Find("Warning").GetComponent<Text>();
		message.text = msg;
		EventSystem.current.SetSelectedGameObject(ok.gameObject);
		WindowManager.AddWindow(new Window(w, Window.WindowType.ACTIVATING));
		ok.onClick.AddListener(delegate { Destroy(w); confirmation.Invoke(value); });
		back.onClick.AddListener(delegate { Destroy(w); returnBack.Invoke(); });
		notificationActive = true;
		return new Window(w, Window.WindowType.ACTIVATING, returnBack);
	}

	public static Window Notify<T>(string msg) {
		GameObject w = Instantiate(_notificationPrefabStatic, _canvas, false);
		Button ok = w.transform.Find("Ok").GetComponent<Button>();
		Text message = w.transform.Find("Notification").GetComponent<Text>();
		message.text = msg;
		EventSystem.current.SetSelectedGameObject(ok.gameObject);
		WindowManager.AddWindow(new Window(w, Window.WindowType.ACTIVATING));
		ok.onClick.AddListener(delegate { Destroy(w); });
		return new Window(w, Window.WindowType.ACTIVATING);
	}

	public static Window Confirm<T>(string msg, T value, Action<T> confirmation, Action returnBack) {
		GameObject w = Instantiate(_confirmationPrefabStatic, _canvas, false);
		Button ok = w.transform.Find("Ok").GetComponent<Button>();
		Button back = w.transform.Find("Back").GetComponent<Button>();
		Text message = w.transform.Find("Confirmation").GetComponent<Text>();
		message.text = msg;
		EventSystem.current.SetSelectedGameObject(ok.gameObject);
		WindowManager.AddWindow(new Window(w, Window.WindowType.ACTIVATING));
		ok.onClick.AddListener(delegate { Destroy(w); confirmation.Invoke(value); });
		back.onClick.AddListener(delegate { Destroy(w); returnBack.Invoke(); });
		notificationActive = true;
		return new Window(w, Window.WindowType.ACTIVATING, returnBack);
	}
}
