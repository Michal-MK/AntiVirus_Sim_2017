using Igor.Constants.Strings;
using System;
using System.Collections;
using UnityEngine;
using WindowsInput;

public class MazeEntrance : MonoBehaviour {
	public Maze maze;
	public Spike spike;

	public float playerIdleTime = 6;

	public static event EventHandler OnMazeEnter;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag(Tags.PLAYER)) {

			//Debug.Log("Remove after");
			//M_Player.gameProgression = 3;
			//Spike.spikesCollected = 3;
			//Coins.coinsCollected = 5;
			//PlayerAttack.bullets = 3;

			StartCoroutine(TransToPos());
			MusicHandler.script.TransitionMusic(MusicHandler.script.room_maze);
			GetComponent<Collider2D>().enabled = false;
		}
	}

	private IEnumerator TransToPos() {

		CamFadeOut.Instance.PlayTransition(CameraTransitionModes.TRANSITION_SCENES, 1f);
		CamFadeOut.OnCamFullyFaded += MazeTransition;
		PlayerMovement.CanMove = false;
		CameraMovement.Instance.IsInMaze = true;

		yield return new WaitUntil(() => !CameraMovement.Instance.CameraStill);
		yield return new WaitUntil(() => CameraMovement.Instance.CameraStill);

		StartCoroutine(PreventPlayerIdle());
		HUDisplay.script.DisplayInfo("What do we have here...? \nGrab the spike and let's get out of this place.", "A maze ... duh?!");

		yield return new WaitWhile(() => HUDisplay.script.isRunning);

		if (Control.currDifficulty >= 3) {
			StartCoroutine(LerpCamPos(CameraMovement.Instance.transform.position, Player.Instance.transform.position));
			StartCoroutine(CameraMovement.Instance.LerpSize(Camera.main.orthographicSize, 80, 0.5f));
		}
		PlayerMovement.CanMove = true;
	}

	private void MazeTransition() {
		OnMazeEnter?.Invoke(this, EventArgs.Empty);

		CameraMovement.Instance.transform.position = new Vector3(maze.middleCell.position.x, maze.middleCell.position.y, -10);
		StartCoroutine(CameraMovement.Instance.LerpSize(CameraMovement.DEFAULT_CAM_SIZE, maze.mazeSize/* * Screen.height / Screen.width * 0.5f*/, 0.2f));

		spike.SetPosition(false);
		Vector2Int rndEdge = maze.GetEdgeCell();
		Player.Instance.transform.position = maze.grid[rndEdge.x, rndEdge.y].transform.position;
		Player.Instance.transform.localScale = new Vector3(2, 2, 0);
		maze.playerEntrancePosition = rndEdge;


		Control.Instance.saveManager.canSave = false;
		Zoom.CanZoom = false;
		CamFadeOut.OnCamFullyFaded -= MazeTransition;
	}

	private IEnumerator PreventPlayerIdle() {
		yield return new WaitForSecondsRealtime(playerIdleTime / (Control.currDifficulty + 1));
		InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
	}

	private IEnumerator LerpCamPos(Vector3 start, Vector3 end) {
		for (float f = 0; f <= 1; f += Time.deltaTime) {
			float x = Mathf.Lerp(start.x, end.x, f);
			float y = Mathf.Lerp(start.y, end.y, f);
			CameraMovement.Instance.transform.position = new Vector3(x, y, -10);
			yield return null;
		}
	}
}
