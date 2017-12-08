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

	public IEnumerator PlayMusic() {
		source.Play();
		for (float f = 0; f <= 1; f += Time.deltaTime * transitionSpeedMult) {
			source.volume = f * 0.3f;
			yield return null;
		}
	}

	public void StopMusicWrapper() {
		StartCoroutine(StopMusic());
	}

	private IEnumerator StopMusic() {
		for (float f = 1; f >= 0; f -= Time.deltaTime * transitionSpeedMult) {
			source.volume = f * 0.3f;
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
}