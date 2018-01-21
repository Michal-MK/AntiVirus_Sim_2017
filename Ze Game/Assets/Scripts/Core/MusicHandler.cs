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

	private AudioClip current;

	public static MusicHandler script;

	private bool _isPlaying = false;

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
		if(background.name == BackgroundNames.BACKGROUND1_1) {
			if (!_isPlaying) {
				PlayMusic(room1_1);
			}
			else {
				TrasnsitionMusic(room1_1);
			}
		}
		if (background.name == BackgroundNames.BACKGROUND1_2) {
			TrasnsitionMusic(room1_2);
		}
		if (background.name == BackgroundNames.BACKGROUND1_3) {
			TrasnsitionMusic(room1_1);
		}
		if (background.name == BackgroundNames.BACKGROUND1_4) {
			PlayMusic(room1_1);
		}
	}
	#endregion

	public void PlayMusic(AudioClip clip) {
		if (!_isPlaying) {
			StartCoroutine(_PlayMusic(clip));
		}
		else {
			Debug.Log("Already Playing");
		}
	}

	public void FadeMusic() {
		StartCoroutine(_FadeMusic());
	}

	public void TrasnsitionMusic(AudioClip newClip) {
		if (_isPlaying) {
			StartCoroutine(_TrasnsitionMusic(newClip));
		}
		else {
			throw new System.Exception("Nothing to transition from!");
		}
	}

	private IEnumerator _PlayMusic(AudioClip clip) {
		musicPlayer.clip = current = clip;
		musicPlayer.Play();
		musicPlayer.volume = 0;
		for (float f = 0; f < 1; f += Time.deltaTime) {
			musicPlayer.volume = f;
			yield return null;
		}
		_isPlaying = true;
	}

	private IEnumerator _FadeMusic() {
		for (float f = musicPlayer.volume; f > 0; f -= Time.unscaledDeltaTime) {
			musicPlayer.volume = f;
			yield return null;
		}
		_isPlaying = false;
		current = null;
	}

	private IEnumerator _TrasnsitionMusic(AudioClip clip) {
		if(clip == current) {
			print("Transitioning to the same clip, skipping");
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
		for (float f = 0; f <= 1; f += Time.unscaledDeltaTime) {
			musicPlayer.volume = f;
			yield return null;
		}
		musicPlayer.volume = 1;
		_isPlaying = true;
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
		get { return _isPlaying; }
	}

	private void OnDestroy() {
		script = null;
		M_Player.OnRoomEnter -= NewRoom;
	}
}
