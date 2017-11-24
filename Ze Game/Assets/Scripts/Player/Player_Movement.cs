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

	public float movementSpeed = 0;

	private float defaultGravity = 0;
	public float flappyDrag;
	public float flappyForceScale = 1;
	public bool canFlapAgain = false;

	private void Update() {
		switch (movementMode) {
			case PlayerMovent.ARROW: {
				ArrowMove();
				break;
			}

			case PlayerMovent.FLAPPY: {
				Flappy();
				break;
			}

			case PlayerMovent.MOUSE: {
				Move();
				throw new System.NotImplementedException();
			}
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
					rigidbody.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")) * BossBehaviour.playerSpeedMultiplier);
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
					rigidbody.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0) * BossBehaviour.playerSpeedMultiplier);
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
					rigidbody.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")) * BossBehaviour.playerSpeedMultiplier);
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
					rigidbody.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0) * BossBehaviour.playerSpeedMultiplier);
				}
				else if (CameraMovement.script.inMaze) {
					rigidbody.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0) * Maze.getMazeSpeedMultiplier);
				}
			}
		}
	}

	//Moving the character FlappyBird style
	public void SetFlappyMode(bool enable) {
		switch (enable) {
			case true: {
				print("Switching to flappy mode.");
				rigidbody.gravityScale = defaultGravity;
				rigidbody.drag = 0;
				movementMode = PlayerMovent.FLAPPY;
				return;
			}
			case false: {
				print("Switching from flappy mode.");
				rigidbody.gravityScale = 0;
				rigidbody.drag = flappyDrag;
				movementMode = PlayerMovent.ARROW;
				return;
			}
		}
	}

	private void Flappy() {
		if (Input.GetAxis("VertMovement") > 0.5f) {
			if (canFlapAgain) {
				rigidbody.velocity = new Vector2(0, flappyForceScale);
				canFlapAgain = false;
				StartCoroutine(FlapAgain());
			}
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			rigidbody.velocity = new Vector2(0, flappyForceScale);
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
}
