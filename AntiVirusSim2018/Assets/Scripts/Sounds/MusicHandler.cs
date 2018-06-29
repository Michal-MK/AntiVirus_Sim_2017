using System.Collections;
using UnityEngine;

public class MusicHandler : MonoBehaviour {
	public AudioSource musicPlayer;

	public AudioClip room1_1;
	public AudioClip room1_2;
	public AudioClip room1_3_avoidance;
	public AudioClip room_maze;
	public AudioClip room_1_boss;
	public AudioClip gameOver;
	public AudioClip darkWorld;

	private AudioClip current;

	public static MusicHandler script;

	public bool isAnythingPlaying { get; private set; } = false;

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	private void UpdateMusicVol(float newValue) {
		musicPlayer.volume = newValue;
	}

	private void Start() {
		M_Player.OnRoomEnter += NewRoom;
		GameSettings.script.OnMusicVolumeChanged += UpdateMusicVol;
	}

	#region EventHandling
	private void NewRoom(RectTransform background, M_Player sender) {

		if(background == MapData.script.GetBackground(1)) {
			if (!isAnythingPlaying) {
				PlayMusic(room1_1);
			}
			else {
				TransitionMusic(room1_1);
			}
		}
		else if (background == MapData.script.GetBackground(2)) {
			if (!isAnythingPlaying) {
				PlayMusic(room1_2);
			}
			else {
				TransitionMusic(room1_2);
			}
		}
		else if (background == MapData.script.GetBackground(3)) {
			if (!isAnythingPlaying) {
				PlayMusic(room1_1);
			}
			else {
				TransitionMusic(room1_1);
			}
		}
		else if (background == MapData.script.GetBackground(4)) {
			if (!isAnythingPlaying) {
				PlayMusic(room1_1);
			}
			else {
				TransitionMusic(room1_1);
			}
		}
	}
	#endregion

	public void PlayMusic(AudioClip clip) {
		if (!isAnythingPlaying) {
			StartCoroutine(_PlayMusic(clip));
		}
		else {
			Debug.Log("Already Playing");
		}
	}

	public void FadeMusic() {
		StartCoroutine(_FadeMusic());
	}

	public void TransitionMusic(AudioClip newClip) {
		if (isAnythingPlaying) {
			StartCoroutine(_TransitionMusic(newClip));
		}
		else {
			StartCoroutine(_PlayMusic(newClip));
			print("Nothing to transition from! Attempting to play normally");
			//throw new System.Exception("Nothing to transition from!");
		}
	}

	private IEnumerator _PlayMusic(AudioClip clip) {
		musicPlayer.clip = current = clip;
		musicPlayer.Play();
		musicPlayer.volume = 0;
		for (float f = 0; f < GameSettings.audioVolume; f += Time.deltaTime) {
			musicPlayer.volume = f;
			yield return null;
		}
		musicPlayer.volume = GameSettings.audioVolume;
		isAnythingPlaying = true;
	}

	private IEnumerator _FadeMusic() {
		for (float f = musicPlayer.volume; f > 0; f -= Time.unscaledDeltaTime) {
			musicPlayer.volume = f;
			yield return null;
		}
		isAnythingPlaying = false;
		current = null;
	}

	private IEnumerator _TransitionMusic(AudioClip clip) {
		if(clip == current) {
			//print("Transitioning to the same clip, skipping");
			yield break;
		}

		float initialVolume = musicPlayer.volume;
		for (float f = initialVolume; f >= 0; f -= Time.unscaledDeltaTime) {
			musicPlayer.volume = f;
			yield return null;
		}

		musicPlayer.volume = 0;
		musicPlayer.Stop();

		musicPlayer.clip = current = clip;
		musicPlayer.Play();
		for (float f = 0; f <= GameSettings.audioVolume; f += Time.unscaledDeltaTime) {
			musicPlayer.volume = f;
			yield return null;
		}
		musicPlayer.volume = GameSettings.audioVolume;
		isAnythingPlaying = true;
	}

	private void OnApplicationFocus(bool focus) {
		if (focus == false) {
			musicPlayer.Pause();
		}
		else {
			musicPlayer.UnPause();
		}
	}


	private void OnDestroy() {
		script = null;
		M_Player.OnRoomEnter -= NewRoom;
		GameSettings.script.OnMusicVolumeChanged -= UpdateMusicVol;
	}
}
