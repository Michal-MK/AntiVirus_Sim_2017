using System.Collections;
using UnityEngine;
using Igor.Constants.Strings;

public class Player_Movement : MonoBehaviour {

	public enum PlayerMovement {
		ARROW,
		FLAPPY,
		INVERT,
		REVERT,
		TELEPORT,
		MOUSE,
	}

	private PlayerMovement movementMode = 0;
	private PlayerMovement movementModeModifier;

	new public Rigidbody2D rigidbody;

	private static bool _canMove = true;

	public float movementSpeed = 550;

	public float flappyGravity = 8;
	private float movementDrag = 30;
	public float flappyForceScale = 35;
	private bool canFlapAgain = true;

	public AudioClip FX_Teleport;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		movementDrag = rigidbody.drag;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		_canMove = true;
		Time.timeScale = 1;
	}

	private void FixedUpdate() {
		switch (movementMode) {
			case PlayerMovement.ARROW: {
				ArrowMove();
				break;
			}
			case PlayerMovement.MOUSE: {
				MouseMove();
				throw new System.NotImplementedException();
			}
		}
	}

	private void Update() {
		if (movementMode == PlayerMovement.FLAPPY) {
			Flappy();
		}
		if (Input.GetKeyDown(KeyCode.I)) {
			M_Player.player.isInvincible = !M_Player.player.isInvincible;
			if (M_Player.player.isInvincible) {
				Canvas_Renderer.script.DisplayInfo(null, "Invincibility Enabled");
			}
			else {
				Canvas_Renderer.script.DisplayInfo(null, "Invincibility Disabled");
			}
		}
	}

	//Moving the Character using a Rigidbody 2D
	private void MouseMove() {
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
	private void ArrowMove() {

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

	//Move character using clicks simirar to FlappyBird
	private void Flappy() {
		if (_canMove) {
			if (Input.GetAxis("VertMovement") > 0.5f) {
				if (canFlapAgain) {
					rigidbody.velocity = new Vector2(0, flappyForceScale);
					canFlapAgain = false;
					StartCoroutine(FlapAgain());
					//print("Flapped");
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

	private IEnumerator Teleportation() {
		SpriteRenderer[] tpNodes = transform.Find("_Teleportation").GetComponentsInChildren<SpriteRenderer>();
		Sprite ready = tpNodes[0].sprite;
		Sprite idle = tpNodes[1].sprite;
		tpNodes[0].sprite = idle;
		tpNodes[0].transform.parent.gameObject.SetActive(true);

		while (movementMode == PlayerMovement.TELEPORT) {
			yield return new WaitUntil(() => Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL)) > 0.5f || Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_VERTICAL)) > 0.5f);

			Directions choice = Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL) != 0 ? DetermineDirection(true, Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL)) : DetermineDirection(false, Input.GetAxis(InputNames.MOVEMENT_VERTICAL));

			tpNodes[(int)choice].sprite = ready;
			yield return new WaitUntil(() => Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL)) < 0.5f && Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_VERTICAL)) < 0.5f);

			yield return new WaitUntil(() => Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL)) > 0.5f || Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_VERTICAL)) > 0.5f);
			Directions secondaryChoice = Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL) != 0 ? DetermineDirection(true, Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL)) : DetermineDirection(false, Input.GetAxis(InputNames.MOVEMENT_VERTICAL));

			if (choice == secondaryChoice) {
				//Do something prettier
				SoundFXHandler.script.PlayFX(FX_Teleport);
				transform.position = tpNodes[(int)secondaryChoice].transform.position;
			}
			tpNodes[(int)choice].sprite = idle;
			if (movementMode == PlayerMovement.TELEPORT) {
				yield return new WaitUntil(() => Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL)) < 0.5f && Mathf.Abs(Input.GetAxis(InputNames.MOVEMENT_VERTICAL)) < 0.5f);
			}
		}
		tpNodes[0].sprite = ready;
		tpNodes[1].sprite = idle;
		transform.Find("_Teleportation").gameObject.SetActive(false);
	}


	private Directions DetermineDirection(bool isHorizontal, float value) {
		if (isHorizontal) {
			if (movementModeModifier != PlayerMovement.INVERT) {
				return value > 0 ? Directions.RIGHT : Directions.LEFT;
			}
			else {
				return value > 0 ? Directions.LEFT : Directions.RIGHT;
			}
		}
		else {
			if (movementModeModifier != PlayerMovement.INVERT) {
				return value > 0 ? Directions.TOP : Directions.BOTTOM;
			}
			else {
				return value > 0 ? Directions.BOTTOM : Directions.TOP;
			}
		}
	}

	//Moving the character FlappyBird style
	public void SetMovementMode(PlayerMovement type) {
		switch (type) {
			case PlayerMovement.FLAPPY: {
				//print("Switching to flappy mode.");
				rigidbody.gravityScale = flappyGravity;
				rigidbody.drag = 0;
				break;
			}
			case PlayerMovement.ARROW: {
				print("Switching to arrow mode.");
				rigidbody.gravityScale = 0;
				rigidbody.drag = movementDrag;
				break;
			}
			case PlayerMovement.INVERT: {
				if (type != movementModeModifier) {
					print("Inverting.");
					movementSpeed = -movementSpeed;
					movementModeModifier = type;
				}
				return;
			}
			case PlayerMovement.REVERT: {
				if (movementModeModifier == PlayerMovement.INVERT) {
					print("Reverting.");
					movementSpeed = -movementSpeed;
					movementModeModifier = type;
				}
				return;
			}
			case PlayerMovement.TELEPORT: {
				movementMode = type;
				StartCoroutine(Teleportation());
				return;
			}
		}
		if (type != PlayerMovement.TELEPORT) {
			StopCoroutine(Teleportation());
			transform.Find("_Teleportation").gameObject.SetActive(false);
		}
		movementMode = type;
	}



	public static bool canMove {
		get { return _canMove; }
		set { _canMove = value; }
	}

	public PlayerMovement getCurrentMovementMode {
		get { return movementMode; }
	}

	public PlayerMovement getCurrentMovementModifier {
		get { return movementModeModifier; }
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}

