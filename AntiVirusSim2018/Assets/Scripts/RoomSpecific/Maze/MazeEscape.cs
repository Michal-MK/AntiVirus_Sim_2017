using Igor.Constants.Strings;
using System.Collections;
using UnityEngine;

public class MazeEscape : MonoBehaviour {

	public Spike spike;
	public MazeEntrance entrance;

	public GameObject wall;

	public static event Maze.MazeBehaviour OnMazeEscape;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == Tags.PLAYER) {
			FromMazeTrans();
			MusicHandler.script.TransitionMusic(MusicHandler.script.room1_1);
		}
	}

	public void FromMazeTrans() {
		entrance.gameObject.SetActive(false);
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
		Player_Movement.canMove = false;
	}

	private void CamFadeOut_OnCamFullyFaded() {
		OnMazeEscape?.Invoke();

		Zoom.canZoom = true;
		M_Player.player.transform.position = entrance.transform.position;
		Camera.main.orthographicSize = 25;
		Camera.main.transform.position = M_Player.player.transform.position;
		M_Player.player.transform.localScale = Vector3.one;
		spike.SetPosition();
		StartCoroutine(FadeWall());
		Player_Movement.canMove = true;
		Control.script.saveManager.canSave = true;
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}

	private IEnumerator FadeWall() {
		Canvas_Renderer.script.DisplayInfo(null, "Ok we are past that... Hey! That wall!");
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
