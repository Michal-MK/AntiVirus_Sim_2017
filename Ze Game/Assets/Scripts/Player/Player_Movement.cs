using System.Collections;
using UnityEngine;

public class Player_Movement : MonoBehaviour {

	public enum PlayerMovent {
		ARROW,
		MOUSE,
		FLAPPY
	}

	private PlayerMovent movementMode = 0;

	new public Rigidbody2D rigidbody;

	private static bool _canMove = true;

	public float movementSpeed = 500;

	public float flappyGravity = 8;
	public float movementDrag = 30;
	public float flappyForceScale = 35;
	public bool canFlapAgain = true;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		_canMove = true;
		Time.timeScale = 1;
	}

	private void FixedUpdate() {
		switch (movementMode) {
			case PlayerMovent.ARROW: {
				ArrowMove();
				break;
			}
			case PlayerMovent.MOUSE: {
				Move();
				throw new System.NotImplementedException();
			}
		}
	}

	private void Update() {
		if (movementMode == PlayerMovent.FLAPPY) {
			Flappy();
		}
	}

	//Moving the Character using a Rigidbody 2D
	public void Move() {
		if (_canMove) {
			if (Input.GetAxis("Mouse X") > 0) {
				rigidbody.AddForce(new Vector2(movementSpeed * Mathf.Abs(Input.GetAxis("Mouse X")) * 2, 0));
			}

			else if (Input.GetAxis("Mouse X") < 0) {
				rigidbody.AddForce(new Vector2(-movementSpeed * Mathf.Abs(Input.GetAxis("Mouse X")) * 2, 0));
			}

			if (Input.GetAxis("Mouse Y") > 0) {
				rigidbody.AddForce(new Vector2(0, movementSpeed * Mathf.Abs(Input.GetAxis("Mouse Y")) * 2));
			}

			else if (Input.GetAxis("Mouse Y") < 0) {
				rigidbody.AddForce(new Vector2(0, -movementSpeed * Mathf.Abs(Input.GetAxis("Mouse Y")) * 2));
			}
		}
	}

	//Moving the Character using a Keyboard
	public void ArrowMove() {

		if (_canMove) {
			if (Input.GetAxis("VertMovement") > 0) {
				if (!CameraMovement.script.inBossRoom && !CameraMovement.script.inMaze) {
					rigidbody.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")));
				}
				else if (CameraMovement.script.inBossRoom) {
					rigidbody.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")) * BossBehaviour.getPlayerSpeedMultiplier);
				}
				else if (CameraMovement.script.inMaze) {
					rigidbody.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")) * Maze.getMazeSpeedMultiplier);
				}
			}

			if (Input.GetAxis("HorMovement") > 0) {

				if (!CameraMovement.script.inBossRoom && !CameraMovement.script.inMaze) {
					rigidbody.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0));
				}
				else if (CameraMovement.script.inBossRoom) {
					rigidbody.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0) * BossBehaviour.getPlayerSpeedMultiplier);
				}
				else if (CameraMovement.script.inMaze) {
					rigidbody.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0) * Maze.getMazeSpeedMultiplier);
				}
			}

			if (Input.GetAxis("VertMovement") < 0) {
				if (!CameraMovement.script.inBossRoom && !CameraMovement.script.inMaze) {
					rigidbody.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")));
				}
				else if (CameraMovement.script.inBossRoom) {
					rigidbody.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")) * BossBehaviour.getPlayerSpeedMultiplier);
				}
				else if (CameraMovement.script.inMaze) {
					rigidbody.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")) * Maze.getMazeSpeedMultiplier);
				}
			}

			if (Input.GetAxis("HorMovement") < 0) {
				if (!CameraMovement.script.inBossRoom && !CameraMovement.script.inMaze) {
					rigidbody.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0));
				}
				else if (CameraMovement.script.inBossRoom) {
					rigidbody.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0) * BossBehaviour.getPlayerSpeedMultiplier);
				}
				else if (CameraMovement.script.inMaze) {
					rigidbody.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0) * Maze.getMazeSpeedMultiplier);
				}
			}
		}
	}

	//Moving the character FlappyBird style
	public void SetMovementMode(PlayerMovent type) {
		switch (type) {
			case PlayerMovent.FLAPPY: {
				print("Switching to flappy mode.");
				rigidbody.gravityScale = flappyGravity;
				rigidbody.drag = 0;
				movementMode = PlayerMovent.FLAPPY;
				return;
			}
			case PlayerMovent.ARROW: {
				print("Switching from flappy mode.");
				rigidbody.gravityScale = 0;
				rigidbody.drag = movementDrag;
				movementMode = PlayerMovent.ARROW;
				return;
			}
		}
	}

	private void Flappy() {
		if (_canMove) {
			if (Input.GetAxis("VertMovement") > 0.5f) {
				if (canFlapAgain) {
					rigidbody.velocity = new Vector2(0, flappyForceScale);
					canFlapAgain = false;
					StartCoroutine(FlapAgain());
					print("Flapped");
				}
			}
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
				rigidbody.velocity = new Vector2(0, flappyForceScale);
			}
		}
	}

	private IEnumerator FlapAgain() {
		yield return new WaitUntil(() => Input.GetAxis("VertMovement") <= 0.5f);
		canFlapAgain = true;
	}


	public static bool canMove {
		get { return _canMove; }
		set { _canMove = value; }
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}

