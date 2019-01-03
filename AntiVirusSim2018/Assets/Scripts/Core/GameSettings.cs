using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using static Igor.Constants.Strings.Settings;

public class GameSettings : MonoBehaviour {

	public static GameSettings script;

	public delegate void SoundVoulmeChanged(float newValue);
	public static float audioVolume { get; private set; }
	public static float fxVolume { get; private set; }

	public event SoundVoulmeChanged OnMusicVolumeChanged;
	public event SoundVoulmeChanged OnFxVolumeChanged;

	private static string path;

	private Slider musicSlid;
	private Slider fxSlid;
	private Button applyChanges;
	private Button backButton;

	private static string[] fileContents;

	public bool fromGame { get; set; }

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
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			DestroyImmediate(gameObject);
			return;
		}
	}

	private void SilderValChange(float value) {
		switch (EventSystem.current.currentSelectedGameObject.name) {
			case "Music_Slid": {
				audioVolume = value;
				if (OnMusicVolumeChanged != null) {
					OnMusicVolumeChanged(value);
				}
				break;
			}
			case "FX_Slid": {
				fxVolume = value;
				if (OnFxVolumeChanged != null) {
					OnFxVolumeChanged(value);
				}
				break;
			}
		}
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
					audioVolume = float.Parse(split[1]);
					break;
				}
				case FX_VOL: {
					fxVolume = float.Parse(split[1]);
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
					fileContents[i] = split[0] + "=" + audioVolume;
					break;
				}
				case FX_VOL: {
					fileContents[i] = split[0] + "=" + fxVolume;
					break;
				}
			}
		}
		File.WriteAllLines(path, fileContents);
	}

	public void Attach(SettingsJoiner settingsJoiner, GameObject canvas) {

		musicSlid = settingsJoiner.transform.Find("Panel/Controls/Music_Slid").GetComponent<Slider>();
		fxSlid = settingsJoiner.transform.Find("Panel/Controls/FX_Slid").GetComponent<Slider>();
		applyChanges = settingsJoiner.transform.Find("Panel/Controls/Apply_Changes").GetComponent<Button>();
		backButton = settingsJoiner.transform.Find("Panel/Controls/Return").GetComponent<Button>();
		musicSlid.value = audioVolume;
		fxSlid.value = fxVolume;
		musicSlid.onValueChanged.AddListener(SilderValChange);
		fxSlid.onValueChanged.AddListener(SilderValChange);
		applyChanges.onClick.AddListener(SaveConfig);

		if (fromGame) {
			backButton.gameObject.SetActive(false);
			applyChanges.onClick.AddListener(delegate { SaveConfig();  WindowManager.CloseMostRecent(); canvas.SetActive(true); });
			fromGame = false;
		}
		else {
			backButton.onClick.AddListener(delegate { WindowManager.CloseMostRecent(); canvas.SetActive(true); });
			applyChanges.onClick.AddListener(delegate { SaveConfig(); WindowManager.CloseMostRecent(); canvas.SetActive(true); GameObject.Find("MenuGraphics").SetActive(true); });
		}
	}
}
