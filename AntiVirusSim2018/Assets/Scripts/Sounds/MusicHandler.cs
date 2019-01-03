using System.Collections;
using UnityEngine;

public class MusicHandler : SoundBase {

	public AudioClip room1_1;
	public AudioClip room1_2;
	public AudioClip room1_3_avoidance;
	public AudioClip room_maze;
	public AudioClip room_1_boss;
	public AudioClip darkWorld;

	public AudioClip gameOver;

	private AudioClip current;

	public static MusicHandler script;

	private bool isPlaying { get { return source.isPlaying; } }

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	private void UpdateMusicVol(float newValue) {
		source.volume = newValue;
	}

	private void Start() {
		M_Player.OnRoomEnter += NewRoom;
		GameSettings.script.OnMusicVolumeChanged += UpdateMusicVol;
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
	#endregion

	public void PlayMusic(AudioClip clip) {
		if (!isPlaying) {
			StartCoroutine(_PlayMusic(clip));
		}
	}

	public void FadeMusic() {
		StartCoroutine(_FadeMusic());
	}

	public void TransitionMusic(AudioClip newClip) {
		if (isPlaying) {
			StartCoroutine(_TransitionMusic(newClip));
		}
		else {
			StartCoroutine(_PlayMusic(newClip));
			Canvas_Renderer.script.DisplayInfo("Nothing to transition from! Attempting to play normally", null);
		}
	}

	private IEnumerator _PlayMusic(AudioClip clip) {
		source.clip = current = clip;
		source.Play();
		source.volume = 0;
		for (float f = 0; f < GameSettings.audioVolume; f += Time.unscaledDeltaTime * 0.5f) {
			source.volume = f;
			yield return null;
		}
		source.volume = GameSettings.audioVolume;
	}

	private IEnumerator _FadeMusic() {
		for (float f = source.volume; f > 0; f -= Time.unscaledDeltaTime * 0.5f) {
			source.volume = f;
			yield return null;
		}
		current = null;
	}

	private IEnumerator _TransitionMusic(AudioClip clip) {
		if (clip == current) {
			//print("Transitioning to the same clip, skipping");
			yield break;
		}

		float initialVolume = source.volume;
		for (float f = initialVolume; f >= 0; f -= Time.unscaledDeltaTime * 0.5f) {
			source.volume = f;
			yield return null;
		}

		source.volume = 0;
		source.Stop();

		source.clip = current = clip;
		source.Play();
		for (float f = 0; f <= GameSettings.audioVolume; f += Time.unscaledDeltaTime * 0.5f) {
			source.volume = f;
			yield return null;
		}
		source.volume = GameSettings.audioVolume;
	}


	private void OnDestroy() {
		script = null;
		M_Player.OnRoomEnter -= NewRoom;
		GameSettings.script.OnMusicVolumeChanged -= UpdateMusicVol;
	}
}
