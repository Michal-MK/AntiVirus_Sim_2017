using UnityEngine;
using UnityEngine.UI;

public class FXSliderPreview : MonoBehaviour {
	private AudioSource source;
	private void OnEnable() {
		source = GetComponent<AudioSource>();
		GetComponent<Slider>().onValueChanged.AddListener(OnValChanged);
	}

	private void OnValChanged(float val) {
		source.volume = val;
		if (!source.isPlaying) {
			source.Play();
		}
	}

	private void OnDisable() {
		GetComponent<Slider>().onValueChanged.RemoveAllListeners();
	}
}


