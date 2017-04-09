using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFadeOut : MonoBehaviour {
	public Animator anim;

	private void Awake() {
		if(Statics.camFade == null) {
			DontDestroyOnLoad(transform.parent.gameObject);
			Statics.camFade = this;
		}
		else if(Statics.camFade != this) {
			Destroy(gameObject);
		}
	}

	public void PlayTransition(string name) {
		switch (name) {
			case "Dim": {
				anim.Play("DimCamera");
				gameObject.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 0;
				//print("Dimming");
				break;
			}
			case "Trans": {
				anim.Play("CamTransition");
				gameObject.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 2;
				//print("Transitioning");
				break;
			}
		}
	}
}
