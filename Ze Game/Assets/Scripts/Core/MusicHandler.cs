using System.Collections;
using UnityEngine;

public class MusicHandler : MonoBehaviour {

	public AudioSource sound;

	public AudioClip room1;
	public AudioClip room2;
	public AudioClip avoidance;
	public AudioClip maze;
	public AudioClip boss;
	public AudioClip gameOver;

	private bool avoid = false;
	private bool lastClip = false;
	public bool stopOnce = true;

	Coroutine currentMusic;

	public static MusicHandler script;

	private void Awake() {
		if(script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	private void Start() {
		M_Player.OnRoomEnter += NewRoom;
	}

	private void NewRoom(RectTransform background, M_Player sender) {
		if (background.name == "Background_room_1") {
			MusicTransition(room2);
		}
		if (background.name == "Background_room_2a") {
			MusicTransition(room1);
		}
	}

	public void PlayMusic(AudioClip clip) {
		StartCoroutine(PlayClip(clip));
	}

	private IEnumerator PlayClip(AudioClip clip) {

		avoid = true;
		yield return new WaitForSecondsRealtime(0.1f);
		float volume = sound.volume;
		for (float f = volume; f > -1; f -= Time.unscaledDeltaTime) {
			if (f > 0) {
				sound.volume = f;
				yield return null;
			}
			else {
				sound.volume = 0;
				sound.Stop();
				continue;
			}
		}
		sound.clip = clip;

		sound.Play();
		for (float f = 0; f < 2; f += Time.unscaledDeltaTime * 0.5f) {
			if (f < 1) {
				sound.volume = f;
				yield return null;
			}
			else {
				sound.volume = 1;
				avoid = false;
				break;
			}
		}
	}

	public void MusicTransition(AudioClip newClip) {
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

		print(newClip);
		if (sound.clip == null) {
			sound.clip = newClip;
			StartCoroutine(StartMusic());
			StopCoroutine(Transition(newClip));
		}

		else {
			if (sound.clip != newClip) {
				for (float f = 1; f >= -1; f -= Time.unscaledDeltaTime * 0.5f) {
					if (avoid) {
						print("New sound playing: Avoidance" + sound.clip + " insted of the selected " + newClip);
						avoid = false;
						break;
					}
					if (f >= 0) {
						sound.volume = f;
						yield return null;
					}
					else {
						sound.volume = 0;
						sound.Stop();
						if (newClip != null) {
							sound.clip = newClip;
							StartCoroutine(StartMusic());
						}
						break;
					}
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
	#endregion

	public IEnumerator StopMusic() {
		if (stopOnce) {
			float soundVolume = sound.volume;
			for (float f = soundVolume; f >= -1; f -= Time.unscaledDeltaTime * 0.5f) {

				if (f > 0) {
					sound.volume = f;
					yield return null;
				}
				else {
					sound.volume = 0;
					stopOnce = false;
					break;
				}
			}
		}
	}
	private void OnApplicationFocus(bool focus) {
		if (focus == false) {
			sound.Pause();
		}
		else {
			sound.UnPause();
		}
	}

	private void OnDestroy() {
		script = null;
		M_Player.OnRoomEnter -= NewRoom;
	}
}
