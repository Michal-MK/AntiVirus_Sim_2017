using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundFXHandler : MonoBehaviour {

	private List<AudioSource> sources = new List<AudioSource>();

	public AudioClip ArrowSound;
	public AudioClip ArrowCollected;
	public AudioClip CoinCollected;
	public AudioClip ELShock;

	public static SoundFXHandler script;

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	void Start() {
		foreach (AudioSource s in transform.GetComponentsInChildren<AudioSource>()) {
			sources.Add(s);
		}
	}

	public void PlayFX(AudioClip newClip) {
		bool added = false;
		foreach (AudioSource s in sources) {
			if (s.clip == null) {
				added = true;
				s.clip = newClip;
				s.Play();
				StartCoroutine(RemoveClipAfterFinish(s));
				return;
			}
		}
		if (!added) {
			print("Not Enough Sound players to play " + newClip.name);
		}

	}

	public IEnumerator RemoveClipAfterFinish(AudioSource s) {
		yield return new WaitUntil(() => !s.isPlaying);
		s.clip = null;
	}

	private void OnDestroy() {
		script = null;
	}
}
