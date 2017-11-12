using System.Collections;
using UnityEngine;

public class MenuMusic : MonoBehaviour {
	public AudioSource source;

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

	public IEnumerator PlayMuic() {
		source.volume = 0f;
		source.Play();
		for (float f = 0; f <= 0.3f; f += CamFadeOut.CAM_FULLY_FADED_NORMAL * Time.unscaledDeltaTime * 0.075f) {
			if (f <= 0.2f) {
				source.volume = f;
				yield return null;
			}
			else {
				source.volume = 0.2f;
				isPlaying = true;
				break;
			}
		}
	}

	public void StopMusicWrapper() {
		StartCoroutine(StopMusic());
	}

	public IEnumerator StopMusic() {
		for (float f = 0.2f; f >= -1; f -= CamFadeOut.CAM_FULLY_FADED_NORMAL * Time.unscaledDeltaTime * 0.075f) {
			if (f > 0) {
				source.volume = f;
				yield return null;
			}
			else {
				source.volume = 0;
				source.Stop();
				break;
			}
		}
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

