using System.Collections;
using UnityEngine;

public class CamFadeOut : MonoBehaviour {
	public Animator anim;
	public const float CAM_FULLY_FADED_NORMAL = 1.5f;
	public const float CAM_FULLY_FADED_DIMMED = 1.5f;

	public static CamFadeOut script;

	public delegate void CamFaded();

	public static event CamFaded OnCamFullyFaded;

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

	/// <summary>
	/// Transitions camera into deifferent mode.
	/// </summary>
	/// <param name="changes">Transition mode</param>
	/// <param name="speed">Animator speed</param>
	public void PlayTransition(CameraModeChanges changes, float speed) {
		switch (changes) {
			case CameraModeChanges.DIM_CAMERA: {
				anim.Play("DimCamera");
				anim.speed = speed;
				gameObject.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 0;
				break;
			}
			case CameraModeChanges.TRANSITION_SCENES: {
				if (anim.GetCurrentAnimatorStateInfo(0).IsName("DimCamera")) {
					anim.Play("TransitionFromDim");
					anim.speed = speed;
					StartCoroutine(AnimState(CAM_FULLY_FADED_DIMMED));
				}
				else {
					anim.Play("CamTransition");
					anim.speed = speed;
					StartCoroutine(AnimState(CAM_FULLY_FADED_NORMAL));
				}
				gameObject.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 2;
				break;
			}
		}
	}

	private IEnumerator AnimState(float delay) {
		yield return new WaitForSecondsRealtime(delay);
		if(OnCamFullyFaded != null) {
			OnCamFullyFaded();
		}
	}
}
