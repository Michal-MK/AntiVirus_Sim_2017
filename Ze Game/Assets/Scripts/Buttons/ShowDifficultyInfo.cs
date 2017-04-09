using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowDifficultyInfo : MonoBehaviour {

	Text text;

	private void OnEnable() {
		text = gameObject.GetComponent<Text>();
		StartCoroutine(Appear());
	}

	private IEnumerator Appear() {
		for (float f = 0; f < 255; f += 5) {
			text.color = new Color32(255, 255, 255, (byte)f);
			if (f >= 255) {
				break;
			}
			else {
				yield return null;
			}
		}

	}

	private void OnDisable() {
		text.color = new Color(1, 1, 1, 0);
	}

}
