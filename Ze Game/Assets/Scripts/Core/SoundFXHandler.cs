using UnityEngine;

public class SoundFXHandler : MonoBehaviour {

	public AudioSource sound;

	public AudioClip ArrowSound;
	public AudioClip ArrowCollected;
	public AudioClip CoinCollected;
	public AudioClip ELShock;

	private bool lastClip = false;

	public static SoundFXHandler script;

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	public void PlayFX(AudioClip clip) {

		switch (clip.name) {
			case "FX - CollectCoin": {
				sound.volume = 1f;
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
		script = null;
	}
}
