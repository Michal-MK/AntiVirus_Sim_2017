using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ShowDifficultyInfo : MonoBehaviour {
	private Text text;

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
		StartCoroutine(_Appear());
	}

	private IEnumerator _Appear() {
		for (float f = 0; f <= 255; f += 5) {
			text.color = new Color32(255, 255, 255, (byte)f);
			yield return null;
		}
	}


	public void Hide() {
		text.color = new Color(1, 1, 1, 0);
	}
}
