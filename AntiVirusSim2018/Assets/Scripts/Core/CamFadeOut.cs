using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CamFadeOut : MonoBehaviour {
	[SerializeField]
	private Animator anim = null;
	public Animator Animator => anim;

	public const float CAM_FULLY_FADED_NORMAL = 1.5f;
	public const float CAM_FULLY_FADED_DIMMED = 1.5f;

	public static CamFadeOut Instance { get; private set; }

	public static event EmptyEventHandler OnCamFullyFaded;

	public static bool registerMenuMusicVolumeFade;
	public static bool registerGameMusicVolumeFade;

	private void Awake() {
		if (Instance == null) {
			DontDestroyOnLoad(transform.parent.gameObject);
			Instance = this;
		}
		else if (Instance != this) {
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Transitions camera into different mode.
	/// </summary>
	/// <param name="change">Transition mode</param>
	/// <param name="speed">Animator speed</param>
	public void PlayTransition(CameraTransitionModes change, float speed) {
		switch (change) {
			case CameraTransitionModes.DIM_CAMERA: {
				anim.Play("DimCamera");
				anim.speed = speed;
				gameObject.transform.parent.gameObject.GetComponent<Canvas>().sortingOrder = 0;
				break;
			}
			case CameraTransitionModes.TRANSITION_SCENES: {
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
				if (registerGameMusicVolumeFade) {
					MusicHandler.script.MapAlphaToVolume(GetComponent<Image>());
					registerGameMusicVolumeFade = false;
				}
				if (registerMenuMusicVolumeFade) {
					MenuMusic.script.MapAlphaToVolume(GetComponent<Image>());
					registerMenuMusicVolumeFade = false;
				}
				break;
			}
		}
	}

	private IEnumerator AnimState(float delay) {
		yield return new WaitForSecondsRealtime(delay);
		OnCamFullyFaded?.Invoke();
	}
}
