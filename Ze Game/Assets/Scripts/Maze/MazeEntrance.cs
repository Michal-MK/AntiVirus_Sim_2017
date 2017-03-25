using System.Collections;
using UnityEngine;

public class MazeEntrance : MonoBehaviour {
	public Maze maze;
	public GameObject player;
	public Spike spike;
	public CameraMovement cam;
	public RectTransform MazeBG;
	public Zoom zoom;

	private void Awake() {
		Statics.mazeEntrance = this;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {

			M_Player.gameProgression = 3;
			StartCoroutine(TransToPos());
			Statics.music.MusicTransition(Statics.music.maze);

		}
	}
	public IEnumerator TransToPos() {

		Statics.camFade.PlayTransition("Trans");
		yield return new WaitForSeconds(2);
		StartCoroutine(cam.LerpSize(cam.defaultCamSize, MazeBG.sizeDelta.x * Screen.height / Screen.width * 0.5f, 0.2f, new Vector3(MazeBG.position.x, MazeBG.position.y, -10)));
		spike.SetPosition();
		cam.inMaze = true;
		cam.transform.position = new Vector3 (MazeBG.position.x,MazeBG.position.y, -10);
		zoom.canZoom = false;
		player.transform.position = maze.grid[maze.GetRandomGridPos(true), maze.GetRandomGridPos(false)].transform.position;
		player.transform.localScale = new Vector3(2, 2, 0);
		Statics.wrapper.EnagleSaving(false);
		yield return new WaitForSeconds(5);
		Statics.canvasRenderer.infoRenderer("What do we have here...? \n"+
											"Grab the spike and let's get out of this place.", "A maze ... duh?!",new Color32(255,255,255,200));
		gameObject.SetActive(false);
		StopCoroutine(TransToPos());
	}

	private void OnDestroy() {
		Statics.mazeEntrance = null;
	}
}
