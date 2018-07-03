using System;
using System.Collections;
using System.Collections.Generic;
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
		while (sprite.color.a < 1) {
			if (sprite.color.a >= 1 - GameSettings.audioVolume) {
				source.volume = 1 - sprite.color.a;
			}
			yield return null;
		}
	}

	private IEnumerator _MapToAlpha(Image image) {
		while (image.color.a < 1) {
			if (image.color.a >= 1 - GameSettings.audioVolume) {
				source.volume = 1 - image.color.a;
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
