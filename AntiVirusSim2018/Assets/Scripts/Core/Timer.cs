using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public float TimeFlowMultiplier { get; private set; } = 1;

	public static Timer Instance { get; private set; }

	public bool IsRunning { get; set; }

	private Text timerText;

	public float ElapsedTime { get; private set; }

	public string ElapsedStr {
		get {
			float current = ElapsedTime;
			if (current != 0) {
				float divided = current / 60;
				int modulo = (int)divided % 60;

				return string.Format("{0:00}:{1:00}.{2:000} {3}", modulo, (int)current % 60, (int)(current * 1000 % 1000), modulo == 0 ? "seconds" : "minute(s)");
			}
			else {
				return "0";
			}
		}
	}

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		}
		else if (Instance != this) {
			Destroy(gameObject);
		}

		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		Player.OnPlayerDeath += M_Player_OnPlayerDeath;
		PauseUnpause.OnPaused += OnPaused;
	}

	private void OnPaused(object sender, PauseEventArgs e) {
		IsRunning = e.IsPlaying;
		Time.timeScale = e.IsPaused ? 0 : 1;
	}

	private void M_Player_OnPlayerDeath(object sender, PlayerDeathEventArgs e) {
		IsRunning = false;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		ElapsedTime = data.core.time;
		StartTimer(1f);
		timerText = GetComponent<Text>();
		timerText.gameObject.SetActive(true);
	}

	private void Start() {
		if (timerText == null) {
			timerText = GetComponent<Text>();
			timerText.gameObject.SetActive(false);
		}
	}

	private void Update() {
		if (IsRunning) {
			ElapsedTime += Time.deltaTime * TimeFlowMultiplier;
			timerText.text = "Time:\t" + ElapsedStr;
		}
	}

	public static void StartTimer(float flowMultiplier) {
		Instance.gameObject.SetActive(true);
		Instance.IsRunning = true;
		Instance.TimeFlowMultiplier = flowMultiplier;
	}

	public static void ResetTimer() {
		Instance.ElapsedTime = 0;
		Instance.IsRunning = false;
		Instance.TimeFlowMultiplier = 1;
	}

	private void OnDestroy() {
		Instance = null;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		Player.OnPlayerDeath -= M_Player_OnPlayerDeath;
		PauseUnpause.OnPaused -= OnPaused;
	}
}
