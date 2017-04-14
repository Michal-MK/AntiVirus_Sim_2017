using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandler : MonoBehaviour {

	public static MusicHandler music;

	public AudioSource source;

	public AudioClip menu;
	public AudioClip room1;
	public AudioClip room2;
	public AudioClip avoidance;
	public AudioClip maze;
	public AudioClip boss;
	public AudioClip endgame;
	public AudioClip gameOver;


	private bool lastClip = false;
	public bool stopOnce = true;

	Coroutine currentMusic;

	private void Awake() {
		if (music == null) {
			music = this;
			print("Set");
			DontDestroyOnLoad(gameObject);
		}
		else if(music != this) {
			Destroy(gameObject);
		}

	}
	private void Start() {
		if(SceneManager.GetActiveScene().buildIndex == 3) {
			MusicTransition(endgame);
		}
		if(SceneManager.GetActiveScene().buildIndex == 0) {
			if(source.clip != menu) {
				PlayMusic(menu);
			}
		}
	}

	public void PlayMusic(AudioClip clip) {
		source.volume = 1;
		source.clip = clip;
		source.Play();
	}

	public void MusicTransition(AudioClip newClip) {
		print("Play " + newClip);
		if (newClip != null) {
			if (currentMusic != null) {
				StopCoroutine(currentMusic);
			}
			currentMusic = StartCoroutine(Transition(newClip));
		}
		else {
			if (!lastClip) {
				StartCoroutine(StopMusic());
				lastClip = true;
			}
		}
	}

	#region MusicTransition Code
	private IEnumerator Transition(AudioClip newClip) {

		print(source.clip +" & "+ newClip);
		if (source.clip == null) {
			source.clip = newClip;
			StartCoroutine(StartMusic());
			StopCoroutine(Transition(newClip));
		}
		if (source.clip != newClip) {
			print(true);
			for (float f = 1; f >= -1; f -= Time.unscaledDeltaTime * 0.5f) {

				if (f >= 0) {
					source.volume = f;
					yield return null;
				}
				else {
					source.volume = 0;
					source.Stop();
					if (newClip != null) {
						source.clip = newClip;
						StartCoroutine(StartMusic());
					}
					break;
				}
			}
		}
	}


	private IEnumerator StartMusic() {
		source.volume = 0;
		source.Play();

		for (float f = 0; f <= 2; f += Time.unscaledDeltaTime * 0.5f) {
			if (f <= 1) {
				source.volume = f;
				yield return null;
			}
			else {
				source.volume = 1;
				StopAllCoroutines();
				break;
			}
		}
	}
	#endregion

	public IEnumerator StopMusic() {
		if (stopOnce) {
			for (float f = 1; f >= -1; f -= Time.unscaledDeltaTime * 0.5f) {
				if (f > 0) {
					source.volume = f;
					yield return null;
				}
				else {
					source.volume = 0;
					stopOnce = false;
					break;
				}
			}
		}
	}
}
