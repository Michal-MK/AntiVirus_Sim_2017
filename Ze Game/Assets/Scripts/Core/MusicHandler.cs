using System.Collections;
using UnityEngine;
using Igor.Constants.Strings;

public class MusicHandler : MonoBehaviour {
	public AudioSource musicPlayer;

	public AudioClip room1_1;
	public AudioClip room1_2;
	public AudioClip room1_3_avoidance;
	public AudioClip room_maze;
	public AudioClip room_1_boss;
	public AudioClip gameOver;

	public static MusicHandler script;

	private bool _isAnythingPlaying = false;

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
	}

	#region EventHandling
	private void NewRoom(RectTransform background, M_Player sender) {
		if (background.name == BackgroundNames.BACKGROUND1_2) {
			TrasnsitionMusic(room1_2);
		}
		if (background.name == BackgroundNames.BACKGROUND1_3) {
			TrasnsitionMusic(room1_1);
		}
	}
	#endregion

	public void PlayMusic(AudioClip clip) {
		_isAnythingPlaying = true;
		StartCoroutine(_PlayMusic(clip));
	}

	public void FadeMusic() {
		StartCoroutine(_FadeMusic());
	}

	public void TrasnsitionMusic(AudioClip newClip) {
		if (_isAnythingPlaying) {
			StartCoroutine(_TrasnsitionMusic(newClip));
		}
		else {
			throw new System.Exception("Nothing to transition from!");
		}
	}

	private IEnumerator _PlayMusic(AudioClip clip) {
		musicPlayer.volume = 0;
		musicPlayer.Play();
		for (float f = 0; f < 1; f += Time.unscaledDeltaTime) {
			musicPlayer.volume = f;
			yield return null;
		}
		_isAnythingPlaying = true;
	}

	private IEnumerator _FadeMusic() {
		for (float f = musicPlayer.volume; f > 0; f += Time.unscaledDeltaTime) {
			musicPlayer.volume = f;
			yield return null;
		}
		_isAnythingPlaying = false;
	}

	private IEnumerator _TrasnsitionMusic(AudioClip clip) {
		float initialVolume = musicPlayer.volume;
		for (float f = initialVolume; f >= 0; f -= Time.unscaledDeltaTime) {
			musicPlayer.volume = f;
			yield return null;
		}
		musicPlayer.volume = 0;
		musicPlayer.Stop();

		musicPlayer.clip = clip;
		musicPlayer.Play();
		for (float f = 0; f <= 1; f += Time.unscaledDeltaTime) {
			musicPlayer.volume = f;
			yield return null;
		}
		musicPlayer.volume = 1;
		_isAnythingPlaying = true;
	}

	private void OnApplicationFocus(bool focus) {
		if (focus == false) {
			musicPlayer.Pause();
		}
		else {
			musicPlayer.UnPause();
		}
	}

	public bool isAnythingPlaying {
		get { return _isAnythingPlaying; }
	}

	private void OnDestroy() {
		script = null;
		M_Player.OnRoomEnter -= NewRoom;
	}
}
