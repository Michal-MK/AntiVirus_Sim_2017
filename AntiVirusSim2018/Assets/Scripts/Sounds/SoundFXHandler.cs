using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
			GameSettings.Instance.OnFxVolumeChanged += UpdateFxVol;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	void Start() {
		foreach (AudioSource s in transform.GetComponentsInChildren<AudioSource>()) {
			sources.Add(s);
			s.volume = GameSettings.FXVolume;
		}
	}

	private void UpdateFxVol(float newValue) {
		foreach (AudioSource source in sources) {
			source.volume = newValue;
		}
	}

	public void PlayFX(AudioClip newClip) {
		AudioSource source = FindFreeSource();
		if (source != null) {
			source.clip = newClip;
			source.Play();
			StartCoroutine(RemoveClipAfterFinish(source));
		}
		else {
			print("Not Enough Sound players to play " + newClip.name);
		}
	}

	public IEnumerator PlayFXLoop(AudioClip loopingEffect, float loopDuration) {
		AudioSource source = FindFreeSource();
		if(source != null) {
			source.clip = loopingEffect;
			source.loop = true;
			source.Play();
			yield return new WaitForSeconds(loopDuration);
			source.loop = false;
		}
		else {
			print("Not Enough Sound players to play looping " + loopingEffect.name);
		}
	}

	private AudioSource FindFreeSource() {
		foreach (AudioSource s in sources) {
			if (s.clip == null) {
				return s;
			}
		}
		return null;
	}

	public void PlayFxChannel(int channel, AudioClip newClip) {
		sources[channel].clip = newClip;
		sources[channel].Play();
		StartCoroutine(RemoveClipAfterFinish(sources[channel]));
	}

	public IEnumerator RemoveClipAfterFinish(AudioSource s) {
		yield return new WaitUntil(() => !s.isPlaying);
		s.clip = null;
		s.loop = false;
	}

	private void OnDestroy() {
		GameSettings.Instance.OnFxVolumeChanged -= UpdateFxVol;
		script = null;
	}
}
