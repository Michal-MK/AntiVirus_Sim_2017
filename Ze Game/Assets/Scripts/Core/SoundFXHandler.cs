using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXHandler : MonoBehaviour {
	public static SoundFXHandler sound;

	public AudioSource source;

	public AudioClip ArrowSound;
	public AudioClip ArrowCollected;
	public AudioClip CoinCollected;
	public AudioClip ELShock;

	private bool lastClip = false;

	private void Awake() {
		if (sound == null) {
			sound = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (sound != this) {
			Destroy(gameObject);
		}

	}

	public void PlayFX(AudioClip clip) {

		switch (clip.name) {
			case "FX - CollectCoin": {
				source.volume = 0.5f;
				break;
			}
			default: {
				source.volume = 1f;
				break;
			}
		}

		if(source.clip != clip) {
			source.clip = clip;
		}
		if (!lastClip) {
			source.Play();
			if (clip == ELShock) {
				lastClip = true;
			}
		}
	}
}
