using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXHandler : MonoBehaviour {
	public static SoundFXHandler script;

	public AudioSource sound;

	public AudioClip ArrowSound;
	public AudioClip ArrowCollected;
	public AudioClip CoinCollected;
	public AudioClip ELShock;

	private void Awake() {
		script = this;
	}

	public void PlayFX(AudioClip clip) {
		print(clip.name);
		switch (clip.name) {
			case "FX - CollectCoin": {
				sound.volume = 0.1f;
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
		sound.Play();
	}
}
