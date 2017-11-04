using UnityEngine;

public class CamFadeOut : MonoBehaviour {
	public Animator anim;

	public static CamFadeOut script;

	public enum CameraModeChanges {
		DIM_CAMERA,
 		TRANSITION_SCENES,
	}

	private void Awake() {
		if(script == null) {
			DontDestroyOnLoad(transform.parent.gameObject);
			script = this;
		}
		else if(script != this) {
			Destroy(gameObject);
		}
	}

	[System.Obsolete("Use the function with enum instead!")]
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
				else
				{
					anim.Play("CamTransition");
				}
				gameObject.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 2;
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

	private void OnDestroy() {
		
	}
}
