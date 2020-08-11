using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundBase : MonoBehaviour {

	public AudioSource source;

	public void MapAlphaToVolume(SpriteRenderer sprite) {
		StartCoroutine(_MapToAlpha(sprite));
	}

	public void MapAlphaToVolume(Image image) {
		StartCoroutine(_MapToAlpha(image));
	}

	private IEnumerator _MapToAlpha(SpriteRenderer sprite) {
		yield return _MapToAlpha(sprite.color);
	}

	private IEnumerator _MapToAlpha(Image image) {
		yield return _MapToAlpha(image.color);
	}

	private IEnumerator _MapToAlpha(Color c) {
		while (c.a < 1) {
			if (c.a >= 1 - GameSettings.AudioVolume) {
				source.volume = 1 - c.a;
			}
			yield return null;
		}
	}

	private void OnApplicationFocus(bool focus) {
		if (focus == false) {
			if (source != null) {
				source.Pause();
			}
		}
		else {
			if (source != null) {
				source.UnPause();
			}
		}
	}
}
