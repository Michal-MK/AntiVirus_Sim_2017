using UnityEngine;
using UnityEngine.UI;
using System.IO;
using static Igor.Constants.Strings.Settings;
using System;

public class GameSettings : MonoBehaviour {

	public static GameSettings Instance { get; private set; }

	public static float AudioVolume { get; private set; }
	public static float FXVolume { get; private set; }

	public event SoundVoulmeChanged OnMusicVolumeChanged;
	public event SoundVoulmeChanged OnFxVolumeChanged;

	private static string path;

	private Slider musicSlid;
	private Slider fxSlid;
	private Button applyChanges;
	private Button backButton;

	private static string[] fileContents;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init() {
		path = Application.dataPath + Path.DirectorySeparatorChar + "Settings" + Path.DirectorySeparatorChar + "config.cfg";
		if (!File.Exists(path)) {
			using (StreamWriter sr = File.CreateText(path)) {
				sr.WriteLine("# This is a configuration file for AVSim game.");
				sr.WriteLine("# This is a comment and will be ignored by the parser");
				sr.WriteLine("# Everything in here was generated automatically and can be changed from the in-game interface.\n");
				sr.WriteLine("# AudioSettings");
				sr.WriteLine();
				sr.WriteLine("music_audio=1");
				sr.WriteLine("effect_audio=1");
			}
		}
		LoadSettings();
	}

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		}
		else if (Instance != this) {
			DestroyImmediate(gameObject);
		}
	}

	private void AudioValChange(float value) {
		AudioVolume = value;
		OnMusicVolumeChanged?.Invoke(value);
	}

	private void FXValChange(float value) {
		FXVolume = value;
		OnFxVolumeChanged?.Invoke(value);
	}

	private static void LoadSettings() {
		fileContents = File.ReadAllLines(path);
		for (int i = 0; i < fileContents.Length; i++) {
			if (fileContents[i].StartsWith("#") || string.IsNullOrWhiteSpace(fileContents[i])) {
				continue;
			}
			string[] split = fileContents[i].Split('=');

			switch (split[0]) {
				case MUSIC_VOL: {
					AudioVolume = float.Parse(split[1]);
					break;
				}
				case FX_VOL: {
					FXVolume = float.Parse(split[1]);
					break;
				}
			}
		}
		print("Settings parsed successfully");
	}

	private void SaveConfig() {
		for (int i = 0; i < fileContents.Length; i++) {
			if (fileContents[i].StartsWith("#") || string.IsNullOrWhiteSpace(fileContents[i])) {
				continue;
			}
			string[] split = fileContents[i].Split('=');

			switch (split[0]) {
				case MUSIC_VOL: {
					fileContents[i] = split[0] + "=" + AudioVolume;
					break;
				}
				case FX_VOL: {
					fileContents[i] = split[0] + "=" + FXVolume;
					break;
				}
			}
		}
		File.WriteAllLines(path, fileContents);
	}

	public void Attach(SettingsJoiner settingsJoiner, GameObject canvas, Action onDetach) {

		musicSlid = settingsJoiner.transform.Find("Panel/Controls/Music_Slid").GetComponent<Slider>();
		fxSlid = settingsJoiner.transform.Find("Panel/Controls/FX_Slid").GetComponent<Slider>();
		applyChanges = settingsJoiner.transform.Find("Panel/Controls/Apply_Changes").GetComponent<Button>();
		backButton = settingsJoiner.transform.Find("Panel/Controls/Return").GetComponent<Button>();
		musicSlid.value = AudioVolume;
		fxSlid.value = FXVolume;
		musicSlid.onValueChanged.AddListener(AudioValChange);
		fxSlid.onValueChanged.AddListener(FXValChange);
		backButton.onClick.AddListener(() => { WindowManager.CloseMostRecent(); canvas.SetActive(true); RemoveEvents(); onDetach?.Invoke(); });
		applyChanges.onClick.AddListener(() => { SaveConfig(); WindowManager.CloseMostRecent(); canvas.SetActive(true); RemoveEvents(); });

		void RemoveEvents() {
			musicSlid.onValueChanged.RemoveAllListeners();
			fxSlid.onValueChanged.RemoveAllListeners();
			backButton.onClick.RemoveAllListeners();
			applyChanges.onClick.RemoveAllListeners();
		}
	}
}