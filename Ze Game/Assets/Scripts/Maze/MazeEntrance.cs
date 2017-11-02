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
	public Wrapper wrapper;

	public bool inMazePropoerly = false;
	public int multiplier;
	private bool entered = false;

	private void Awake() {
		Statics.mazeEntrance = this;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {

			M_Player.gameProgression = 3;
			Spike.spikesCollected = 3;
			Coins.coinsCollected = 5;
			PlayerAttack.bullets = 3;


			if (Spike.spikesCollected == 3 && !entered) {
				entered = true;
				M_Player.gameProgression = 3;
				StartCoroutine(TransToPos());
				Statics.music.MusicTransition(Statics.music.maze);
			}
		}
	}

	public IEnumerator TransToPos() {

		Statics.camFade.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES);

		cam.inMaze = true;
		yield return new WaitForSeconds(1.5f);
		M_Player.doNotMove = true;

		cam.transform.position = new Vector3(MazeBG.position.x, MazeBG.position.y, -10);
		StartCoroutine(cam.LerpSize(CameraMovement.defaultCamSize, MazeBG.sizeDelta.x * Screen.height / Screen.width * 0.5f, 0.2f, new Vector3(MazeBG.position.x, MazeBG.position.y, -10)));

		spike.SetPosition();
		player.transform.position = maze.grid[maze.GetRandomGridPos(true), maze.GetRandomGridPos(false)].transform.position;
		player.transform.localScale = new Vector3(2, 2, 0);

		infoBoardMaze.transform.position = maze.grid[maze.GetRandomGridPos(true), maze.GetRandomGridPos(false)].transform.position;
		wrapper.AllowSaving(false);

		Statics.cameraMovement.psA.gameObject.SetActive(false);
		Statics.cameraMovement.psB.gameObject.SetActive(false);
		zoom.canZoom = false;
		yield return new WaitUntil(() => CameraMovement.doneMoving);

		StartCoroutine(PreventPlayerIdle());
		Statics.canvasRenderer.InfoRenderer("What do we have here...? \n" +
											"Grab the spike and let's get out of this place.", "A maze ... duh?!", new Color32(255, 255, 255, 200));
		yield return new WaitWhile(() => Statics.canvasRenderer.isRunning);

		if (PlayerPrefs.GetInt("difficulty") >= 3) {
			StartCoroutine(LerpCamPos(cam.transform.position, player.transform.position));
			StartCoroutine(cam.LerpSize(Camera.main.orthographicSize, 80, 0.5f));
			multiplier = 2;
		}
		else {
			multiplier = 4;
		}

		inMazePropoerly = true;
		M_Player.doNotMove = false;
	}

	private IEnumerator PreventPlayerIdle() {
		yield return new WaitForSecondsRealtime(10);
		InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
	}

	private IEnumerator LerpCamPos(Vector3 start, Vector3 end) {
		for (float f = 0; f < 2; f += Time.deltaTime) {
			if (f < 1) {
				//print(f);
				float x = Mathf.Lerp(start.x, end.x, f);
				float y = Mathf.Lerp(start.y, end.y, f);
				cam.transform.position = new Vector3(x, y, -10);
				yield return null;
			}
			else {
				//print(f);
				StopCoroutine("LerpCamPos");
				break;
			}
		}
	}

	private void OnDestroy() {
		Statics.mazeEntrance = null;
	}
}
