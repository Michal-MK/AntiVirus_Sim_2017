using System.Collections;
using UnityEngine;

public class MenuMusic : MonoBehaviour {
	public AudioSource source;
	public float transitionSpeedMult = 1;

	public static MenuMusic script;
	public bool isPlaying = false;

	private void Awake() {
		if (script == null) {
			script = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	private void Start() {
		GameSettings.script.OnMusicVolumeChanged += UpdateMusicVol;
	}


	private void UpdateMusicVol(float newValue) {
		source.volume = newValue;
	}

	public void PlayMusic() {
		isPlaying = true;
		GetComponent<AudioListener>().enabled = true;
		StartCoroutine(_PlayMusic());
	}

	private IEnumerator _PlayMusic() {
		source.Play();
		for (float f = 0; f <= GameSettings.audioVolume; f += Time.deltaTime * transitionSpeedMult) {
			source.volume = f;
			yield return null;
		}
	}

	public void StopMusic() {
		StartCoroutine(_StopMusic());
		GetComponent<AudioListener>().enabled = false;
	}

	private IEnumerator _StopMusic() {
		for (float f = GameSettings.audioVolume; f >= 0; f -= Time.deltaTime * transitionSpeedMult) {
			source.volume = f;
			yield return null;
		}
		source.Stop();
		isPlaying = false;
	}

	private void OnApplicationFocus(bool focus) {
		if (focus == false) {
			source.Pause();
		}
		else {
			source.UnPause();
		}
	}


	private void OnDestroy() {
		GameSettings.script.OnMusicVolumeChanged -= UpdateMusicVol;
	}
}