using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFadeOut : MonoBehaviour {
	public Animator anim;

	public enum CameraModeChanges {
		DIM_CAMERA,
 		TRANSITION_SCENES,
	}

	private void Awake() {
		if(Statics.camFade == null) {
			DontDestroyOnLoad(transform.parent.gameObject);
			Statics.camFade = this;
		}
		else if(Statics.camFade != this) {
			Destroy(gameObject);
		}
	}

	[System.Obsolete("Use the function with enum instead!")]
	public void PlayTransition(string name) {
		switch (name) {
			case "Dim": {
				anim.Play("DimCamera");
				gameObject.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 0;
				//print("Dimming");
				break;
			}
			case "Trans": {
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("DimCamera")) {
					anim.Play("TransitionFromDim");
				}
				else
				{
					anim.Play("CamTransition");
				}
				gameObject.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 2;
				//print("Transitioning");
				break;
			}
		}
	}

	public void PlayTransition(CameraModeChanges changes) {
		switch (changes) {
			case CameraModeChanges.DIM_CAMERA: {
				anim.Play("DimCamera");
				gameObject.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 0;
				break;
			}
			case CameraModeChanges.TRANSITION_SCENES: {
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("DimCamera")) {
					anim.Play("TransitionFromDim");
				}
				else {
					anim.Play("CamTransition");
				}
				gameObject.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 2;
				break;
			}
		}
	}
}
