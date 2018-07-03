using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundFXHandler : SoundBase {

	private List<AudioSource> sources = new List<AudioSource>();

	public AudioClip ArrowSound;
	public AudioClip ArrowCollected;
	public AudioClip CoinCollected;
	public AudioClip ELShock;

	public static SoundFXHandler script;

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	void Start() {
		foreach (AudioSource s in transform.GetComponentsInChildren<AudioSource>()) {
			sources.Add(s);
			s.volume = GameSettings.fxVolume;
		}
		GameSettings.script.OnFxVolumeChanged += UpdateFxVol;
	}

	private void UpdateFxVol(float newValue) {
		foreach (AudioSource source in sources) {
			source.volume = newValue;
		}
	}

	public void PlayFX(AudioClip newClip) {
		bool added = false;
		foreach (AudioSource s in sources) {
			if (s.clip == null) {
				added = true;
				s.clip = newClip;
				s.Play();
				StartCoroutine(RemoveClipAfterFinish(s));
				return;
			}
		}
		if (!added) {
			print("Not Enough Sound players to play " + newClip.name);
		}
	}

	public void PlayFxChannel(int channel, AudioClip newClip) {
		sources[channel].clip = newClip;
		sources[channel].Play();
		StartCoroutine(RemoveClipAfterFinish(sources[channel]));
	}

	public IEnumerator RemoveClipAfterFinish(AudioSource s) {
		yield return new WaitUntil(() => !s.isPlaying);
		s.clip = null;
	}

	private void OnDestroy() {
		script = null;
	}
}
