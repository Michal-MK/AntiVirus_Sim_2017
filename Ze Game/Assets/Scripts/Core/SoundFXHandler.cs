using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXHandler : MonoBehaviour {

	public AudioSource sound;

	public AudioClip ArrowSound;
	public AudioClip ArrowCollected;
	public AudioClip CoinCollected;
	public AudioClip ELShock;

	private bool lastClip = false;

	private void Awake() {
		Statics.sound = this;
	}

	public void PlayFX(AudioClip clip) {

		switch (clip.name) {
			case "FX - CollectCoin": {
				sound.volume = 0.5f;
				break;
			}
			default: {
				sound.volume = 1f;
				break;
			}
		}

		if(sound.clip != clip) {
			sound.clip = clip;
		}
		if (!lastClip) {
			sound.Play();
			if (clip == ELShock) {
				lastClip = true;
			}
		}
	}
	private void OnDestroy() {
		Statics.sound = null;
	}
}
