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
		float initial = sprite.color.a;
		float end = initial > 0.5f ? 0 : 1;
		float startVol = source.volume;
		float endVol = end == 1 ? 0 : GameSettings.AudioVolume;
		while (source.volume != endVol) {
			source.volume = ValueMapping.MapFloat(sprite.color.a, initial, end, startVol, endVol);
			yield return null;
		}
	}

	private IEnumerator _MapToAlpha(Image image) {
		float initial = image.color.a;
		float end = initial > 0.5f ? 0 : 1;
		float startVol = source.volume;
		float endVol = end == 1 ? 0 : GameSettings.AudioVolume;
		while (source.volume != endVol) {
			source.volume = ValueMapping.MapFloat(image.color.a, initial, end, startVol, endVol);
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
