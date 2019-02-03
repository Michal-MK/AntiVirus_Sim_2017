using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : SoundBase {

	public AudioClip room1_1;
	public AudioClip room1_2;
	public AudioClip room1_3_avoidance;
	public AudioClip room_maze;
	public AudioClip room_1_boss;
	public AudioClip darkWorld;

	public Dictionary<AudioClip, float> positions = new Dictionary<AudioClip, float>();

	public AudioClip gameOver;

	public static MusicHandler script;

	private bool isPlaying => source.isPlaying;

	private bool isTransitioning = false;

	private Coroutine transitionRoutine;

	public const float transitionTime = 2;

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	private void Start() {
		M_Player.OnRoomEnter += NewRoom;
		GameSettings.script.OnMusicVolumeChanged += UpdateMusicVol;
		positions.Add(room1_1, 0);
		positions.Add(room1_2, 0);
		positions.Add(room1_3_avoidance, 0);
		positions.Add(room_maze, 0);
		positions.Add(room_1_boss, 0);
		positions.Add(darkWorld, 0);
	}


	#region EventHandling

	private void NewRoom(M_Player sender, RectTransform background, RectTransform previous) {

		if (background == MapData.script.GetRoom(1).background) {
			if (!isPlaying) {
				PlayMusic(room1_1);
			}
			else {
				TransitionMusic(room1_1);
			}
		}
		else if (background == MapData.script.GetRoom(2).background) {
			if (!isPlaying) {
				PlayMusic(room1_2);
			}
			else {
				TransitionMusic(room1_2);
			}
		}
		else if (background == MapData.script.GetRoom(3).background) {
			if (!isPlaying) {
				PlayMusic(room1_1);
			}
			else {
				TransitionMusic(room1_1);
			}
		}
		else if (background == MapData.script.GetRoom(4).background) {
			if (!isPlaying) {
				PlayMusic(room1_1);
			}
			else {
				TransitionMusic(room1_1);
			}
		}
	}

	private void UpdateMusicVol(float newValue) {
		source.volume = newValue;
	}

	#endregion

	/// <summary>
	/// Starts playing music from 0 volume fading in for 1 second
	/// </summary>
	public void PlayMusic(AudioClip clip) {
		if (!isPlaying) {
			transitionRoutine = StartCoroutine(_PlayMusic(clip));
		}
	}

	/// <summary>
	/// Fade out music for 1 second
	/// </summary>
	public void FadeMusic() {
		if (!isTransitioning) {
			StartCoroutine(_FadeMusic());
		}
	}

	/// <summary>
	/// Fades out current music and fades in the new one, potentially just plays the clip normally, if nothing was playing before.
	/// </summary>
	public void TransitionMusic(AudioClip newClip) {
		if (isPlaying) {
			if (transitionRoutine != null) {
				StopCoroutine(transitionRoutine);
			}
			transitionRoutine = StartCoroutine(_TransitionMusic(newClip));
		}
		else {
			PlayMusic(newClip);
		}
	}

	private IEnumerator _PlayMusic(AudioClip clip) {
		isTransitioning = true;
		source.clip = clip;
		source.Play();
		yield return _TransitionVolume(0, GameSettings.audioVolume);
		isTransitioning = false;
	}

	private IEnumerator _FadeMusic() {
		isTransitioning = true;
		float originalVolume = source.volume;
		yield return _TransitionVolume(originalVolume, 0);
		positions[source.clip] = 0;
		source.Stop();
		isTransitioning = false;
	}

	private IEnumerator _TransitionMusic(AudioClip clip) {
		isTransitioning = true;
		float currentVolume = source.volume;

		if (clip == source.clip) {
			yield return _TransitionVolume_SameClip(currentVolume);
			isTransitioning = false;
			yield break;
		}

		yield return _TransitionVolume(currentVolume, 0);
		positions[source.clip] = source.time;
		source.clip = clip;
		source.time = positions[source.clip];
		source.Play();
		yield return _TransitionVolume(0, GameSettings.audioVolume);

		isTransitioning = false;
	}

	private IEnumerator _TransitionVolume_SameClip(float currentVolume) {
		float fadeVal = Mathf.Lerp(0, GameSettings.audioVolume, Time.unscaledDeltaTime / transitionTime);
		for (float f = currentVolume; f < GameSettings.audioVolume; f += fadeVal) {
			source.volume = f;
			yield return null;
		}
		source.volume = GameSettings.audioVolume;
	}

	private IEnumerator _TransitionVolume(float initValue, float endValue) {

		if (initValue < endValue) {
			float fadeVal = Mathf.Lerp(initValue, endValue, Time.unscaledDeltaTime / transitionTime);

			for (float f = initValue; f < endValue; f += fadeVal) {
				source.volume = f;
				yield return null;
			}
			source.volume = endValue;
		}
		else {
			float fadeVal = Mathf.Lerp(endValue, initValue, Time.unscaledDeltaTime / transitionTime);
			for (float f = initValue; f > endValue; f -= fadeVal) {
				source.volume = f;
				yield return null;
			}
			source.volume = endValue;
		}
	}

	private void OnDestroy() {
		script = null;
		M_Player.OnRoomEnter -= NewRoom;
		GameSettings.script.OnMusicVolumeChanged -= UpdateMusicVol;
	}
}
