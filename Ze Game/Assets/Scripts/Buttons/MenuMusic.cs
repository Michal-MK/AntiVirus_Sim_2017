using System.Collections;
using UnityEngine;

public class MenuMusic : MonoBehaviour {
	public AudioSource source;

	public static MenuMusic script;

	private void Awake() {
		if (script == null) {
			script = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	void Start() {
		source.Play();
	}

	public void StopMusicWrapper() {
		StartCoroutine(StopMusic());
	}

	public IEnumerator StopMusic() {
		for (float f = 0.2f; f >= -1; f -= CamFadeOut.CAM_FULLY_FADED_NORMAL * Time.unscaledDeltaTime * 0.1f) {
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

