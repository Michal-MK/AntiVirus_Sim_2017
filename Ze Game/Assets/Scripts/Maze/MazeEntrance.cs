using System.Collections;
using UnityEngine;
using WindowsInput;

public class MazeEntrance : MonoBehaviour {
	public Maze maze;
	public GameObject player;
	public Spike spike;
	public CameraMovement cam;
	public RectTransform MazeBG;
	public Zoom zoom;
	public GameObject infoBoardMaze;

	public float playerIdleTime = 10;
	public bool inMazePropoerly = false;
	private bool entered = false;

	public static event Maze.MazeBehaviour OnMazeEnter;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {

			Debug.Log("Remove after");
			M_Player.gameProgression = 3;
			Spike.spikesCollected = 3;
			Coins.coinsCollected = 5;
			PlayerAttack.bullets = 3;


			if (Spike.spikesCollected == 3 && !entered) {
				entered = true;
				M_Player.gameProgression = 3;
				StartCoroutine(TransToPos());
				MusicHandler.script.MusicTransition(MusicHandler.script.maze);
			}
		}
	}

	private IEnumerator TransToPos() {

		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += MazeTransition;
		cam.inMaze = true;

		CameraMovement.doneMoving = false;
		yield return new WaitUntil(() => CameraMovement.doneMoving);

		StartCoroutine(PreventPlayerIdle());
		Canvas_Renderer.script.InfoRenderer("What do we have here...? \n" +
											"Grab the spike and let's get out of this place.", "A maze ... duh?!", new Color32(255, 255, 255, 200));

		yield return new WaitWhile(() => Canvas_Renderer.script.isRunning);

		if (Control.currDifficulty >= 3) {
			StartCoroutine(LerpCamPos(cam.transform.position, player.transform.position));
			StartCoroutine(cam.LerpSize(Camera.main.orthographicSize, 80, 0.5f));
		}


		inMazePropoerly = true;
		Player_Movement.canMove = true;
	}

	private void MazeTransition() {
		if (OnMazeEnter != null) {
			OnMazeEnter();
		}

		Maze.inMaze = true;
		Player_Movement.canMove = false;

		cam.transform.position = new Vector3(MazeBG.position.x, MazeBG.position.y, -10);
		StartCoroutine(cam.LerpSize(CameraMovement.defaultCamSize, MazeBG.sizeDelta.x * Screen.height / Screen.width * 0.5f, 0.2f, new Vector3(MazeBG.position.x, MazeBG.position.y, -10)));

		spike.SetPosition();
		Vector2 rndEdge = maze.GetEdgeCell();
		player.transform.position = maze.grid[(int)rndEdge.x, (int)rndEdge.y].transform.position;
		player.transform.localScale = new Vector3(2, 2, 0);

		Vector2 rndSignPos = maze.GetEdgeCell(rndEdge);
		infoBoardMaze.transform.position = maze.grid[(int)rndSignPos.x, (int)rndSignPos.y].transform.position;

		SaveManager.canSave = false;
		Zoom.canZoom = false;

		CamFadeOut.OnCamFullyFaded -= MazeTransition;

	}

	private IEnumerator PreventPlayerIdle() {
		yield return new WaitForSecondsRealtime(playerIdleTime);
		InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
	}

	private IEnumerator LerpCamPos(Vector3 start, Vector3 end) {
		for (float f = 0; f < 2; f += Time.deltaTime) {
			if (f < 1) {
				float x = Mathf.Lerp(start.x, end.x, f);
				float y = Mathf.Lerp(start.y, end.y, f);
				cam.transform.position = new Vector3(x, y, -10);
				yield return null;
			}
			else {
				break;
			}
		}
	}
}
