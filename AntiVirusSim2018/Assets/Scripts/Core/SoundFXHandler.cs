using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundFXHandler : MonoBehaviour {

	private List<AudioSource> sources = new List<AudioSource>();

	public AudioClip ArrowSound;
	public AudioClip ArrowCollected;
	public AudioClip CoinCollected;
	public AudioClip ELShock;

	private bool lastClip = false;

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
				print("Added");
				added = true;
				s.clip = newClip;
				s.Play();
				StartCoroutine(RemoveClipAfterFinish(s));
				return;
			}
		}
		if (!added) {
			print("Not Enough Sound players");
		}

	}

	public IEnumerator RemoveClipAfterFinish(AudioSource s) {
		yield return new WaitUntil(() => !s.isPlaying);
		s.clip = null;
		print("Removed");
	}

	private void OnDestroy() {
		script = null;
	}
}
