using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ShowDifficultyInfo : MonoBehaviour {
	private Text text;
	private bool coroutineActive;
	#region Lifecycle

	private void OnEnable() {
		text = gameObject.GetComponent<Text>();
		StartCoroutine(_Appear());
	}

	private void OnDisable() {
		text.color = new Color(1, 1, 1, 0);
	}

	#endregion

	public void Appear() {
		if (!coroutineActive) {
			StartCoroutine(_Appear());
		}
	}

	private IEnumerator _Appear() {
		coroutineActive = true;
		for (float f = 0; f <= 255; f += 5) {
			if (!coroutineActive) yield break;
			text.color = new Color32(255, 255, 255, (byte)f);
			yield return null;
		}
		coroutineActive = false;
	}


	public void Hide() {
		coroutineActive = false;
		text.color = new Color(1, 1, 1, 0);
	}
}
