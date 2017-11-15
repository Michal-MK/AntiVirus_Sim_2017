using System.Collections;
using UnityEngine;

public class MazeEscape : MonoBehaviour {

	public GameObject player;
	public RectTransform BG;
	public Spike spike;
	public MazeEntrance entrance;

	public bool pathOpen = false;

	public GameObject wall;

	public static event Maze.MazeBehaviour OnMazeEscape;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			StartCoroutine(FromMazeTrans());
			MusicHandler.script.MusicTransition(MusicHandler.script.room1);
		}
	}

	public IEnumerator FromMazeTrans() {
		entrance.gameObject.SetActive(false);
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1f);
		yield return new WaitForSeconds(1.5f);
		if(OnMazeEscape != null) {
			OnMazeEscape();
		}
		Maze.inMaze = false;

		Camera.main.GetComponent<CameraMovement>().inMaze = false;
		Zoom.canZoom = true;
		player.transform.position = new Vector3(BG.position.x, BG.position.y + BG.sizeDelta.y / 2 - 10, 0);
		Camera.main.orthographicSize = 25;
		Camera.main.transform.position = player.transform.position;
		player.transform.localScale = Vector3.one;
		spike.SetPosition();
		StartCoroutine(FadeWalls());
		pathOpen = true;

		SaveManager.canSave = true;
	}

	private IEnumerator FadeWalls() {
		SpriteRenderer wallSprite = wall.GetComponent<SpriteRenderer>();

		Color32 newColor;

		for (float f = 255; f >= -1; f -= 0.5f) {
			if (Time.timeScale != 1) {
				f = f + 0.5f;
				yield return null;
			}
			else {
				newColor = new Color32(255, 255, 255, (byte)f);
				wallSprite.color = newColor;

				if (f > 0) {
					yield return null;
				}
				else if (f <= 0) {
					wall.SetActive(false);
					break;
				}
			}
		}
	}
}
