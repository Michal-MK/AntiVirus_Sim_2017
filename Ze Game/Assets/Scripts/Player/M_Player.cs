using System.Collections;
using UnityEngine;

public delegate void BackgroundChanged(RectTransform background, M_Player sender);
public delegate void CoinEvents(M_Player sender);
public delegate void SpikeEvents(M_Player sender);
public delegate void PlayerColision(M_Player sender, GameObject other);
public delegate void PlayerDeath(M_Player sender);

public class M_Player : MonoBehaviour {
	#region PrefabReferences
	public Rigidbody2D rg;
	public GameObject face;
	public PlayerAttack pAttack;
	private Sprite previous;
	#endregion

	public Animator GameOverImg;

	public static int gameProgression;
	public static string currentBG_name;

	public bool newGame = true;
	public bool gameOver = false;

	private int attempts;

	public Sprite smile;
	public Sprite happy;
	public Sprite sad;

	public static M_Player player;

	public static event BackgroundChanged OnRoomEnter;
	public static event PlayerColision OnSpikePickup;
	public static event PlayerColision OnCoinPickup;
	public static event PlayerColision OnTargetableObjectCollision;
	public static event Zoom.Zooming OnZoomModeSwitch;
	public static event PlayerDeath OnPlayerDeath;

	public static PlayerState playerState = PlayerState.NORMAL;

	public enum PlayerState {
		NORMAL,
		ATTACKING
	}

	private void Awake() {
		if (player == null) {
			player = this;
		}
		else if (player != this) {
			Destroy(gameObject);
		}
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		transform.position = data.player.playerPos;
		gameProgression = data.player.spikesCollected;
		attempts = data.core.localAttempt;
	}

	void Start() {
		Cursor.lockState = CursorLockMode.Confined;
#if !UNITY_EDITOR
		string name = Control.currProfile.getProfileName;
#endif
		StartCoroutine(DelayIntro());
	}

	private IEnumerator DelayIntro() {
		yield return new WaitForSeconds(1);
		GameProgression.script.Progress();
		if (newGame) {
			attempts++;
			Canvas_Renderer.script.InfoRenderer("Welcome! \n" +
												"This is your " + attempts + ". attempt to put the virus into a quaratine. \n\n" +
												"This box will appear only when I have something important to say,\n otherwise look for information in the upper left corner, so it is less disruptive. \n"
												, null);

			Control.currAttempt = attempts;
		}

		Canvas_Renderer.script.InfoRenderer(null, "Good luck & Have fun!");
	}

	//private void Update() {
	//	switch (movementMode) {
	//		case PlayerMovent.ARROW: {
	//			ArrowMove();
	//			break;
	//		}

	//		case PlayerMovent.FLAPPY: {
	//			Flappy();
	//			break;
	//		}

	//		case PlayerMovent.MOUSE: {
	//			Move();
	//			throw new System.NotImplementedException();
	//		}
	//	}
	//}
	//Moving the Character using a Rigidbody 2D
	//public void Move() {
	//	move = new Vector3(0, 0, 0);

	//	if (doNotMove == false) {
	//		if (Input.GetAxis("Mouse X") > 0) {
	//			rg.AddForce(new Vector2(Speed * Mathf.Abs(Input.GetAxis("Mouse X")) * 2, 0));
	//		}

	//		else if (Input.GetAxis("Mouse X") < 0) {
	//			rg.AddForce(new Vector2(-Speed * Mathf.Abs(Input.GetAxis("Mouse X")) * 2, 0));
	//		}

	//		if (Input.GetAxis("Mouse Y") > 0) {
	//			rg.AddForce(new Vector2(0, Speed * Mathf.Abs(Input.GetAxis("Mouse Y")) * 2));
	//		}

	//		else if (Input.GetAxis("Mouse Y") < 0) {
	//			rg.AddForce(new Vector2(0, -Speed * Mathf.Abs(Input.GetAxis("Mouse Y")) * 2));
	//		}
	//	}
	//}

	//Moving the Character using a Keyboard
	//public void ArrowMove() {

	//	if (doNotMove == false) {
	//		if (Input.GetAxis("VertMovement") > 0) {
	//			if (!cam.inBossRoom && !cam.inMaze) {
	//				rg.AddForce(new Vector2(0, Speed * Input.GetAxis("VertMovement")));
	//			}
	//			else if (cam.inBossRoom) {
	//				rg.AddForce(new Vector2(0, Speed * Input.GetAxis("VertMovement")) * BossBehaviour.playerSpeedMultiplier);
	//			}
	//			else if (cam.inMaze) {
	//				rg.AddForce(new Vector2(0, Speed * Input.GetAxis("VertMovement")) * mazeSpeedMultiplier);
	//			}
	//		}

	//		if (Input.GetAxis("HorMovement") > 0) {

	//			if (!cam.inBossRoom && !cam.inMaze) {
	//				rg.AddForce(new Vector2(Input.GetAxis("HorMovement") * Speed, 0));
	//			}
	//			else if (cam.inBossRoom) {
	//				rg.AddForce(new Vector2(Input.GetAxis("HorMovement") * Speed, 0) * BossBehaviour.playerSpeedMultiplier);
	//			}
	//			else if (cam.inMaze) {
	//				rg.AddForce(new Vector2(Input.GetAxis("HorMovement") * Speed, 0) * mazeSpeedMultiplier);
	//			}
	//		}

	//		if (Input.GetAxis("VertMovement") < 0) {
	//			if (!cam.inBossRoom && !cam.inMaze) {
	//				rg.AddForce(new Vector2(0, Speed * Input.GetAxis("VertMovement")));
	//			}
	//			else if (cam.inBossRoom) {
	//				rg.AddForce(new Vector2(0, Speed * Input.GetAxis("VertMovement")) * BossBehaviour.playerSpeedMultiplier);
	//			}
	//			else if (cam.inMaze) {
	//				rg.AddForce(new Vector2(0, Speed * Input.GetAxis("VertMovement")) * mazeSpeedMultiplier);
	//			}
	//		}

	//		if (Input.GetAxis("HorMovement") < 0) {
	//			if (!cam.inBossRoom && !cam.inMaze) {
	//				rg.AddForce(new Vector2(Input.GetAxis("HorMovement") * Speed, 0));
	//			}
	//			else if (cam.inBossRoom) {
	//				rg.AddForce(new Vector2(Input.GetAxis("HorMovement") * Speed, 0) * BossBehaviour.playerSpeedMultiplier);
	//			}
	//			else if (cam.inMaze) {
	//				rg.AddForce(new Vector2(Input.GetAxis("HorMovement") * Speed, 0) * mazeSpeedMultiplier);
	//			}
	//		}
	//	}
	//}

	//Moving the character FlappyBird style

	//public void SetFlappyMode(bool enable) {
	//	switch (enable) {
	//		case true: {
	//			print("Switching to flappy mode.");
	//			rg.gravityScale = gravity;
	//			rg.drag = 0;
	//			movementMode = PlayerMovent.FLAPPY;
	//			return;
	//		}
	//		case false: {
	//			print("Switching from flappy mode.");
	//			rg.gravityScale = 0;
	//			rg.drag = linearDrag;
	//			movementMode = PlayerMovent.ARROW;
	//			return;
	//		}
	//	}
	//}

	//private void Flappy() {
	//	if (Input.GetAxis("VertMovement") > 0.5f) {
	//		if (onceOnAxis) {
	//			rg.velocity = new Vector2(0, UpVelocity);
	//			onceOnAxis = false;
	//			StartCoroutine(FlapAgain());
	//		}
	//	}
	//	if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
	//		rg.velocity = new Vector2(0, UpVelocity);
	//	}
	//}

	//private IEnumerator FlapAgain() {
	//	yield return new WaitUntil(() => Input.GetAxis("VertMovement") <= 0.5f);
	//	onceOnAxis = true;
	//}


	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.name == "killerblock") {
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ELShock);
		}
		if (collision.transform.name == "Block") {
			if (!BlockScript.pressurePlateTriggered && OnTargetableObjectCollision != null) {
				OnTargetableObjectCollision(this, collision.gameObject);
			}
		}
		if (collision.transform.tag == "Enemy") {
			print("Collided");
			if (collision.gameObject.GetComponent<Rigidbody2D>() != null) {
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
				collision.gameObject.GetComponent<Rigidbody2D>().velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity / 10;
			}
			collision.transform.parent = GameObject.Find("Collectibles").transform;
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ELShock);
			GameOver();
		}
	}

	private void OnTriggerEnter2D(Collider2D col) {

		if (col.tag == "Enemy") {
			print(col.gameObject.name);
			if (col.gameObject.GetComponent<Rigidbody2D>() != null) {
				col.gameObject.GetComponent<Rigidbody2D>().velocity = col.gameObject.GetComponent<Rigidbody2D>().velocity / 10;
			}
			col.transform.SetParent(GameObject.Find("Collectibles").transform, false);
			face.GetComponent<SpriteRenderer>().sprite = sad;
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ELShock);
			GameOver();

		}
		if (col.transform.tag == "BG") {
			if (OnRoomEnter != null) {
				OnRoomEnter(col.GetComponent<RectTransform>(), this);
			}
			currentBG_name = col.name;
			CameraMovement.script.RaycastForRooms();

			if (col.name == "Background_room_1") {
				if (gameProgression == 3) {
					Canvas_Renderer.script.InfoRenderer(null, "Go down even further.");
				}
			}
			if (col.name == "Background_room_Boss_1") {
				gameProgression = 10;
			}
		}

		if (col.tag == "Spike") {
			if (OnSpikePickup != null) {
				OnSpikePickup(this, col.gameObject);
			}
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowCollected);
			GameProgression.script.Progress();
			face.GetComponent<SpriteRenderer>().sprite = happy;
			PlayerAttack.bullets++;
			if (gameObject.GetComponent<PlayerAttack>().visibleAlready == true) {
				gameObject.GetComponent<PlayerAttack>().bulletCount.text = "x " + PlayerAttack.bullets;
			}

			if (Spike.spikesCollected == 5) {
				string text;
				if (pAttack.displayShootingInfo) {
					text = "You found all the bullets.\n You can fire them by switching into \"ShootMode\" (Space) and target using your mouse.\n The bullets are limited, don't lose them!";
					pAttack.displayShootingInfo = false;
				}
				else {
					text = "You found all the bullets.\n You can fire them by... oh, you already know. Well... don't lose them!";
				}
				Canvas_Renderer.script.InfoRenderer(text, "Don't give up now.");
			}

		}
		if (col.name == "Coin") {
			face.GetComponent<SpriteRenderer>().sprite = happy;
			if (OnCoinPickup != null) {
				OnCoinPickup(this, col.gameObject);
			}
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.CoinCollected);
			Canvas_Renderer.script.UpdateCounters("Coin");
		}

		if (col.name == "BombPickup") {
			PlayerAttack.bombs++;
			Destroy(col.gameObject);
			Canvas_Renderer.script.InfoRenderer("You found a bomb, it will be useful later on.", null);
		}

		//if (col.name == "Test") {
		//	if (i % 2 == 0) {
		//		print(i % 2 + " " + i);
		//		SetFlappyMode(true);
		//		i++;
		//	}
		//	else {
		//		print(i % 2 + " " + i);
		//		SetFlappyMode(false);
		//		i++;
		//	}
		//}

		if (col.tag == "ArrowTrap") {
			previous = face.GetComponent<SpriteRenderer>().sprite;
			face.GetComponent<SpriteRenderer>().sprite = sad;
		}
	}

	private void OnTriggerExit2D(Collider2D col) {
		//if (col.transform.tag == "BG") {
		//	CameraMovement.script.RaycastForRooms();
		//}
		if (col.tag == "ArrowTrap") {
			face.GetComponent<SpriteRenderer>().sprite = previous;
		}
	}

	public void FloorComplete() {
		Player_Movement.canMove = false;
		Cursor.visible = true;
		Timer.PauseTimer();
#if !UNITY_EDITOR
		UploadScore score = new UploadScore();
#endif
	}

	public void GameOver() {

		if (OnPlayerDeath != null) {
			OnPlayerDeath(this);
		}

		Player_Movement.canMove = false;
		Cursor.visible = true;
		Timer.PauseTimer();

		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.DIM_CAMERA, 1f);
		GameOverImg.SetTrigger("Appear");
		MusicHandler.script.StartCoroutine(MusicHandler.script.StopMusic());
		if (OnZoomModeSwitch != null) {
			OnZoomModeSwitch(false);
		}
		gameOver = true;

		Destroy(GameObject.Find("Enemies"));
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}
