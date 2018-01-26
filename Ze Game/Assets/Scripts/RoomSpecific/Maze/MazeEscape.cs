using System.Collections;
using UnityEngine;

public class MazeEscape : MonoBehaviour {

	public Spike spike;
	public MazeEntrance entrance;

	public GameObject wall;

	public static event Maze.MazeBehaviour OnMazeEscape;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
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
		RectTransform room3BG = MapData.script.GetBackground(4);

		if (OnMazeEscape != null) {
			OnMazeEscape();
		}

		Camera.main.GetComponent<CameraMovement>().inMaze = false;
		Zoom.canZoom = true;
		M_Player.player.transform.position = new Vector3(room3BG.position.x, room3BG.position.y + room3BG.sizeDelta.y / 2 - 10, 0);
		Camera.main.orthographicSize = 25;
		Camera.main.transform.position = M_Player.player.transform.position;
		M_Player.player.transform.localScale = Vector3.one;
		spike.SetPosition();
		spike.transform.localScale = Vector3.one;
		StartCoroutine(FadeWalls());
		Player_Movement.canMove = true;
		SaveManager.canSave = true;
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}

	private IEnumerator FadeWalls() {
		SpriteRenderer wallSprite = wall.GetComponent<SpriteRenderer>();
		Color32 newColor;

		for (float f = 255; f >= 0; f -= 0.5f) {
			newColor = new Color32(255, 255, 255, (byte)f);
			wallSprite.color = newColor;
			yield return null;
		}
		wall.SetActive(false);
	}
}
