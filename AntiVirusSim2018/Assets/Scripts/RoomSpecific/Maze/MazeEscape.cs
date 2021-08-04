using Igor.Constants.Strings;
using System;
using System.Collections;
using UnityEngine;

public class MazeEscape : MonoBehaviour {

	[SerializeField]
	private Spike spike = null;
	[SerializeField]
	private MazeEntrance entrance = null;
	[SerializeField]
	private GameObject wall = null;
	[SerializeField]
	private CameraControls camControls = null;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag(Tags.PLAYER)) {
			FromMazeTrans();
			MusicHandler.script.TransitionMusic(MusicHandler.script.room1_1);
		}
	}

	public void FromMazeTrans() {
		entrance.gameObject.SetActive(false);
		CamFadeOut.Instance.PlayTransition(CameraTransitionModes.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
		PlayerMovement.CanMove = false;
		PlayerMovement.SpeedMultiplier = 1;
	}

	private void CamFadeOut_OnCamFullyFaded() {
		camControls.CamMovement.OnMazeEscaped();
		Zoom.CanZoom = true;
		Player.Instance.transform.position = entrance.transform.position;
		camControls.Zoom.ZoomTo(camControls.Zoom.NormalZoom);
		Player.Instance.transform.localScale = Vector3.one;
		spike.SetPosition();
		StartCoroutine(FadeWall());
		PlayerMovement.CanMove = true;
		Control.Instance.saveManager.canSave = true;
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}

	private IEnumerator FadeWall() {
		HUDisplay.Instance.DisplayInfo(null, "Ok we are past that... Hey! That wall!");
		SpriteRenderer wallSprite = wall.GetComponentInChildren<SpriteRenderer>();
		Color32 newColor;

		for (float f = 255; f >= 0; f -= 0.8f) {
			newColor = new Color32(255, 255, 255, (byte)f);
			wallSprite.color = newColor;
			yield return null;
		}
		wall.SetActive(false);
	}
}
