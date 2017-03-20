using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour {

	public AudioSource sound;

	public AudioClip room1;
	public AudioClip room2;
	public AudioClip avoidance;
	public AudioClip maze;
	public AudioClip boss;
	public AudioClip gameOver;


	private bool lastClip = false;

	private void Awake() {
		Statics.music = this;
	}

	public void PlayMusic(AudioClip clip) {
		sound.volume = 1;
		sound.clip = clip;
		sound.Play();
	}

	public void MusicTransition(AudioClip newClip) {
		if (newClip != null) {
			StartCoroutine(Transition(newClip));
		}
		else {
			if (!lastClip) {
				StartCoroutine(StopMusic());
				lastClip = true;
			}
		}
	}
	#region MusicTransition Code
	private IEnumerator Transition(AudioClip clip) {
		print(clip);
		if (sound.clip == null) {
			sound.clip = clip;
			StartCoroutine(StartMusic());
			StopCoroutine(Transition(clip));
		}

		else {
			for (float f = 1; f >= -1; f -= Time.unscaledDeltaTime * 0.5f) {

				if (f >= 0) {
					sound.volume = f;
					yield return null;
				}
				else {
					sound.volume = 0;
					sound.Stop();
					if (clip != null) {
						sound.clip = clip;
						StartCoroutine(StartMusic());
					}
					break;
				}
			}
		}
	}

	private IEnumerator StartMusic() {
		sound.volume = 0;
		sound.Play();

		for (float f = 0; f <= 2; f += Time.unscaledDeltaTime * 0.5f) {
			if (f <= 1) {
				sound.volume = f;
				yield return null;
			}
			else {
				sound.volume = 1;
				StopAllCoroutines();
				break;
			}
		}
	}

	public IEnumerator StopMusic() {
		for (float f = 1; f >= -1; f -= Time.unscaledDeltaTime * 0.5f) {
			if (f > 0) {
				sound.volume = f;
				print(f);
				yield return null;
			}
			else {
				sound.volume = 0;
				break;
			}
			#endregion
		}
	}
	private void OnDestroy() {
		Statics.music = null;
	}
}
