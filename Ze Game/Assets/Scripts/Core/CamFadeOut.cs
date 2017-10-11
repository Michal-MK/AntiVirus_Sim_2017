using UnityEngine;

public class CamFadeOut : MonoBehaviour {

	public enum CamTransitionModes {
		DIM_CAMERA,
		TRANSITION_SCENES,
	}

	public Animator anim;
	
	private void Awake() {
		if (Statics.camFade == null) {
			DontDestroyOnLoad(transform.parent.gameObject);
			Statics.camFade = this;
		}
		else if (Statics.camFade != this) {
			Destroy(gameObject);
		}
	}

	[System.Obsolete("Use the method with Enums instead!")]
	public void PlayTransition(string name) {
		switch (name) {
			case "Dim": {
				anim.Play("DimCamera");
				gameObject.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 0;
				break;
			}
			case "Trans": {
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

	public void PlayTransition(CamTransitionModes mode) {
		switch (mode) {
			case CamTransitionModes.DIM_CAMERA: {
				anim.Play("DimCamera");
				gameObject.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 0;
				break;
			}
			case CamTransitionModes.TRANSITION_SCENES: {
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
