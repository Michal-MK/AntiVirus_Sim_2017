using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : SoundBase {

	public static MenuMusic script;
	public bool isPlaying { get { return source.isPlaying; } }

	private void Awake() {
		if (script == null) {
			script = this;
			DontDestroyOnLoad(gameObject);
			GameSettings.script.OnMusicVolumeChanged += UpdateMusicVol;
			SceneManager.sceneLoaded += OnMenuLeave;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	private void OnMenuLeave(Scene newScene, LoadSceneMode mode) {
		if(newScene.name == Igor.Constants.Strings.SceneNames.GAME1_SCENE) {
			Destroy(gameObject);
		}
	}

	private void Start() {
	}

	private void UpdateMusicVol(float newValue) {
		source.volume = newValue;
	}

	public void PlayMusic() {
		GetComponent<AudioListener>().enabled = true;
		StartCoroutine(_PlayMusic());
	}

	private IEnumerator _PlayMusic() {
		source.Play();
		for (float f = 0; f <= GameSettings.audioVolume; f += Time.deltaTime) {
			source.volume = f;
			yield return null;
		}
	}

	public void StopMusic() {
		StartCoroutine(_StopMusic());
		GetComponent<AudioListener>().enabled = false;
	}

	private IEnumerator _StopMusic() {
		for (float f = GameSettings.audioVolume; f >= 0; f -= Time.deltaTime) {
			source.volume = f;
			yield return null;
		}
		source.Stop();
	}

	private void OnDestroy() {
		if (script == this) {
			script = null;
			GameSettings.script.OnMusicVolumeChanged -= UpdateMusicVol;
		}
	}
}