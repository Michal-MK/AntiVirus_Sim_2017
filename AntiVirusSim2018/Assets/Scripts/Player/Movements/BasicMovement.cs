using System;
using UnityEngine;
using static Player_Movement;

public class BasicMovement : MonoBehaviour, IPlayerMovement {

	public PlayerMovement movementType => PlayerMovement.ARROW;

	private PlayerMovementModifiers _modifiers = PlayerMovementModifiers.NONE;

	public PlayerMovementModifiers movementModifier {
		get { return _modifiers; }
		set {
			switch (value) {
				case PlayerMovementModifiers.INVERT: {
					print("Inverting.");
					movementSpeed = -movementSpeed;
					_modifiers = value;
					break;
				}
			}
		}
	}

	public float movementSpeed { get; set; } = 50;

	public float movementDrag = 30;

	public Rigidbody2D body;

	public void Setup(Rigidbody2D body) {
		this.body = body;
		body.gravityScale = 0;
		body.drag = movementDrag;
	}

	public void Move() {
		if (Input.GetAxis("VertMovement") > 0) {
			if (!CameraMovement.script.inBossRoom && !CameraMovement.script.inMaze) {
				body.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")));
			}
			else if (CameraMovement.script.inBossRoom) {
				body.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")) * BossBehaviour.getPlayerSpeedMultiplier);
			}
			else if (CameraMovement.script.inMaze) {
				body.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")) * Maze.getMazeSpeedMultiplier);
			}
		}

		if (Input.GetAxis("HorMovement") > 0) {

			if (!CameraMovement.script.inBossRoom && !CameraMovement.script.inMaze) {
				body.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0));
			}
			else if (CameraMovement.script.inBossRoom) {
				body.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0) * BossBehaviour.getPlayerSpeedMultiplier);
			}
			else if (CameraMovement.script.inMaze) {
				body.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0) * Maze.getMazeSpeedMultiplier);
			}
		}

		if (Input.GetAxis("VertMovement") < 0) {
			if (!CameraMovement.script.inBossRoom && !CameraMovement.script.inMaze) {
				body.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")));
			}
			else if (CameraMovement.script.inBossRoom) {
				body.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")) * BossBehaviour.getPlayerSpeedMultiplier);
			}
			else if (CameraMovement.script.inMaze) {
				body.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")) * Maze.getMazeSpeedMultiplier);
			}
		}

		if (Input.GetAxis("HorMovement") < 0) {
			if (!CameraMovement.script.inBossRoom && !CameraMovement.script.inMaze) {
				body.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0));
			}
			else if (CameraMovement.script.inBossRoom) {
				body.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0) * BossBehaviour.getPlayerSpeedMultiplier);
			}
			else if (CameraMovement.script.inMaze) {
				body.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0) * Maze.getMazeSpeedMultiplier);
			}
		}
	}

	public void Stop() {
		//Nothing to cleanup
	}
}
