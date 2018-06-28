using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	private static float _time;
	private static bool _isRunning;
	private static float timeFlowMultiplier = 1;

	private Text Timer_text;

	public static event PauseUnpause.Pause OnTimerPause;

	public static Timer script;

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		M_Player.OnPlayerDeath += M_Player_OnPlayerDeath;
	}

	private void M_Player_OnPlayerDeath(M_Player sender) {
		_isRunning = false;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		_time = data.core.time;
		StartTimer(1f);
		Timer_text = GetComponent<Text>();
		Timer_text.gameObject.SetActive(true);
	}

	private void Start() {
		if (Timer_text == null) {
			Timer_text = GetComponent<Text>();
			Timer_text.gameObject.SetActive(false);
		}
	}

	private void Update() {
		if (_isRunning) {
			_time += Time.deltaTime * timeFlowMultiplier;
			Timer_text.text = "Time:\t" + getTimeFormated;
		}
	}


	public static void PauseTimer() {
		_isRunning = false;
		if (OnTimerPause != null) {
			OnTimerPause(true);
		}
	}

	public static void StartTimer(float flowMultiplier) {
		script.gameObject.SetActive(true);
		_isRunning = true;
		if (OnTimerPause != null) {
			OnTimerPause(false);
		}
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
		get {
			if (getTime != 0) {
				return string.Format("{0:00}:{1:00}.{2:00} minutes", (int)getTime / 60, getTime % 60, getTime.ToString().Remove(0, getTime.ToString().Length - 2));
			}
			else {
				return "0";
			}
		}
	}

	public static bool isRunning {
		get { return _isRunning; }
	}

	private void OnDestroy() {
		script = null;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		M_Player.OnPlayerDeath -= M_Player_OnPlayerDeath;
	}
}
