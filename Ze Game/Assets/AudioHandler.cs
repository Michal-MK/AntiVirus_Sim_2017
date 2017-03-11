using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour {

	public AudioSource sound;

	public AudioClip menu;
	public AudioClip room1;
	public AudioClip room2;
	public AudioClip maze;
	public AudioClip boss;
	
	public static AudioHandler script;

	private void Awake() {
		script = this;
	}

	// Use this for initialization
	void Start() {
		sound.clip = room1;
		sound.Play();
	}

	public void MusicTransition(AudioClip newClip) {
		StartCoroutine(StopMusic(newClip));
	}

	#region MusicTransition Code
	private IEnumerator StopMusic(AudioClip clip) {
		if (sound.clip == null) {
			sound.clip = clip;
			StartCoroutine(StartMusic());
			StopCoroutine(StopMusic(clip));
		}
		else {
			for (float f = 1; f >= -1; f -= Time.deltaTime * 0.5f) {

				if (f >= 0) {
					sound.volume = f;
					yield return null;
				}
				else {
					sound.volume = 0;
					sound.Stop();
					sound.clip = clip;
					StartCoroutine(StartMusic());
					break;
				}
			}
		}
	}
	private IEnumerator StartMusic() {
		sound.volume = 0;
		sound.Play();

		for (float f = 0; f <= 2; f += Time.deltaTime * 0.5f) {
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
	#endregion
}
