using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	private static float _time;
	private static bool _isRunning;
	private static float timeFlowMultiplier = 1;

	private Text Timer_text;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		_time = data.core.time;
		StartTimer(1f);
	}

	void Start() {
		Timer_text = GameObject.Find("Timer_text").GetComponent<Text>();
		Timer_text.gameObject.SetActive(false);
	}

	void Update() {

		if (isRunning) {
			Timer_text.gameObject.SetActive(true);

			_time += Time.deltaTime * timeFlowMultiplier;

			Timer_text.text = "Time:\t" + getTimeFormated;
		}
	}


	public static void PauseTimer() {
		_isRunning = false;
	}

	public static void StartTimer(float flowMultiplier) {
		_isRunning = true;
		timeFlowMultiplier = flowMultiplier;
	}

	public static void ResetTimer() {
		_time = 0;
		_isRunning = false;
		timeFlowMultiplier = 1;
	}

	public static float getTime {
		get { return _time; }
	}

	public static string getTimeFormated {
		get { return string.Format("{0:00}:{1:00}.{2:00} minutes", (int)getTime / 60, getTime % 60, getTime.ToString().Remove(0, getTime.ToString().Length - 2)); }
	}

	public static bool isRunning {
		get { return _isRunning; }
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}