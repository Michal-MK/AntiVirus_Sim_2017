using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public delegate void BackgroundChanged(RectTransform background);
public class M_Player : MonoBehaviour {
	public int attemptNr;
	public float Speed;
	public static bool doNotMove;
	public Vector3 move;
	public GameObject saveButton;
	public GameObject restartButton;
	public GameObject quitToMenu;
	public GameObject loadButton;
	public Animator GameOverImg;

	public static float distanceToWall;
	public static int gameProgression;
	public static string currentBG_name;
	public Rigidbody2D rg;

	public CameraMovement cam;
	public EnemySpawner spawner;
	public BossBehaviour boss;
	public SaveGame save;
	public Guide guide;

	public bool disableSavesByBoss = false;
	private int mode = 0;
	public bool newGame = true;
	public bool gameOver = false;

	public float gravity;
	public float UpVelocity;
	public float linearDrag;
	private bool doFlappy = false;
	private int attempts;

	public GameObject face;

	public Sprite smile;
	public Sprite happy;
	public Sprite sad;

	private Sprite previous;
	int i = 0;

	public static event BackgroundChanged OnRoomEnter;

	private void Awake() {
		Statics.mPlayer = this;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		throw new NotImplementedException();
	}

	void Start() {
		Cursor.lockState = CursorLockMode.Confined;
		restartButton.SetActive(false);
		quitToMenu.SetActive(false);
		saveButton.SetActive(false);
		loadButton.SetActive(false);

		string name = PlayerPrefs.GetString("player_name");

		if (name == null || name == "") {
			PlayerPrefs.SetInt("Attempts", 0);
		}
		attempts = PlayerPrefs.GetInt("Attempts");

		StartCoroutine(DelayIntro());
	}

	private IEnumerator DelayIntro() {
		newGame = Control.script.isNewGame;
		yield return new WaitForSeconds(1);
		Statics.gameProgression.Progress();
		if (newGame && !Control.script.isRestarting) {

			//Control.script.Save(true);
			Statics.music.PlayMusic(Statics.music.room1);
			attempts++;
			Statics.canvasRenderer.InfoRenderer("Welcome! \n" +
												"This is your " + attempts + ". attempt to put the virus into a quaratine. \n\n" +
												"This box will appear only when I have something important to say,\n otherwise look for information in the upper left corner, so it is less disruptive. \n",
												"Good luck & Have fun!");

			PlayerPrefs.SetInt("Attempts", attempts);
			newGame = false;
		}
		else if (Control.script.isRestarting) {
			print("ISRESTARTING");
			Statics.music.PlayMusic(Statics.music.room1);
			Statics.canvasRenderer.InfoRenderer(null, "Good luck & Have fun!");
			Control.script.isRestarting = false;
		}
		Control.script.isNewGame = false;
		Control.script.isRestarting = false;
	}

	private void FixedUpdate() {

		switch (mode) {

			case 0: {
				//Move();
				ArrowMove();
				doFlappy = false;
				break;
			}

			case 1: {
				doFlappy = true;
				break;
			}
		}
	}

	private void Update() {
		if (disableSavesByBoss && saveButton.GetComponent<Toggle>().interactable) {
			saveButton.GetComponent<Toggle>().interactable = false;
		}

		if (doFlappy) {
			Flappy();
		}
	}
	//Moving the Character using a Rigidbody 2D
	public void Move() {
		move = new Vector3(0, 0, 0);

		if (doNotMove == false) {
			if (Input.GetAxis("Mouse X") > 0) {
				rg.AddForce(new Vector2(Speed * Mathf.Abs(Input.GetAxis("Mouse X")) * 2, 0));
			}

			else if (Input.GetAxis("Mouse X") < 0) {
				rg.AddForce(new Vector2(-Speed * Mathf.Abs(Input.GetAxis("Mouse X")) * 2, 0));
			}

			if (Input.GetAxis("Mouse Y") > 0) {
				rg.AddForce(new Vector2(0, Speed * Mathf.Abs(Input.GetAxis("Mouse Y")) * 2));
			}

			else if (Input.GetAxis("Mouse Y") < 0) {
				rg.AddForce(new Vector2(0, -Speed * Mathf.Abs(Input.GetAxis("Mouse Y")) * 2));
			}
		}
	}

	//Moving the Character using a Keyboard
	private bool right = false;
	private bool down = false;
	private bool left = false;
	private bool up = false;

	public void ArrowMove() {

		if (doNotMove == false) {
			if (Input.GetAxis("Vertical") > 0) {
				up = true;
				if (!cam.inBossRoom && !cam.inMaze) {
					rg.AddForce(new Vector2(0, Speed * Input.GetAxis("Vertical")));
				}
				else if (cam.inBossRoom) {
					rg.AddForce(new Vector2(0, Speed * Input.GetAxis("Vertical")) * Statics.bossBehaviour.playerSpeedMultiplier);
				}
				else if (cam.inMaze) {
					rg.AddForce(new Vector2(0, Speed * Input.GetAxis("Vertical")) * Statics.mazeEntrance.multiplier);
				}
			}
			else {
				up = false;
			}

			if (Input.GetAxis("Horizontal") > 0) {
				right = true;
				if (!cam.inBossRoom && !cam.inMaze) {
					rg.AddForce(new Vector2(Input.GetAxis("Horizontal") * Speed, 0));
				}
				else if (cam.inBossRoom) {
					rg.AddForce(new Vector2(Input.GetAxis("Horizontal") * Speed, 0) * Statics.bossBehaviour.playerSpeedMultiplier);
				}
				else if (cam.inMaze) {
					rg.AddForce(new Vector2(Input.GetAxis("Horizontal") * Speed, 0) * Statics.mazeEntrance.multiplier);
				}
			}
			else {
				right = false;
			}

			if (Input.GetAxis("Vertical") < 0) {
				down = true;
				if (!cam.inBossRoom && !cam.inMaze) {
					rg.AddForce(new Vector2(0, Speed * Input.GetAxis("Vertical")));
				}
				else if (cam.inBossRoom) {
					rg.AddForce(new Vector2(0, Speed * Input.GetAxis("Vertical")) * Statics.bossBehaviour.playerSpeedMultiplier);
				}
				else if (cam.inMaze) {
					rg.AddForce(new Vector2(0, Speed * Input.GetAxis("Vertical")) * Statics.mazeEntrance.multiplier);
				}
			}
			else {
				down = false;
			}

			if (Input.GetAxis("Horizontal") < 0) {
				left = true;
				if (!cam.inBossRoom && !cam.inMaze) {
					rg.AddForce(new Vector2(Input.GetAxis("Horizontal") * Speed, 0));
				}
				else if (cam.inBossRoom) {
					rg.AddForce(new Vector2(Input.GetAxis("Horizontal") * Speed, 0) * Statics.bossBehaviour.playerSpeedMultiplier);
				}
				else if (cam.inMaze) {
					rg.AddForce(new Vector2(Input.GetAxis("Horizontal") * Speed, 0) * Statics.mazeEntrance.multiplier);
				}
			}
			else {
				left = false;
			}
			if (!up && !right && !down && !left) {
				rg.velocity = Vector2.zero;
			}
		}
	}

	//Moving the character FlappyBird style
	public void ChangeFlappy(bool start = false) {
		print("ChnagedToFlappy");
		switch (start) {
			case true: {
				rg.gravityScale = gravity;
				rg.drag = 0;
				mode = 1;
				break;
			}
			case false: {
				rg.gravityScale = 0;
				rg.drag = linearDrag;
				mode = 0;
				break;
			}
		}
	}

	private bool onceOnAxis = true;

	public void Flappy() {
		if (Input.GetAxis("Vertical") > 0.5f) {
			if (onceOnAxis) {
				rg.velocity = new Vector2(0, UpVelocity);
				onceOnAxis = false;
				StartCoroutine(FlapAgain());
			}
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			rg.velocity = new Vector2(0, UpVelocity);
		}
	}

	private IEnumerator FlapAgain() {
		yield return new WaitUntil(() => Input.GetAxis("Vertical") <= 0.5f);
		onceOnAxis = true;
	}


	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.name == "killerblock") {
			Statics.sound.PlayFX(Statics.sound.ELShock);
		}
		if (collision.transform.name == "Block") {
			print("Collided!");
			if (!BlockScript.pressurePlateTriggered) {
				guide.enableGuide();
				guide.Recalculate(GameObject.Find("Pressure_Plate"), true);
			}
		}
		if (collision.transform.tag == "Enemy") {
			print("Collided");
			if (collision.gameObject.GetComponent<Rigidbody2D>() != null) {
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
				collision.gameObject.GetComponent<Rigidbody2D>().velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity / 10;
			}
			collision.transform.parent = GameObject.Find("Collectibles").transform;
			Statics.sound.PlayFX(Statics.sound.ELShock);
			GameOver();
		}
	}

	private void OnTriggerEnter2D(Collider2D col) {

		if (col.tag == "Enemy") {
			if (col.gameObject.GetComponent<Rigidbody2D>() != null) {
				col.gameObject.GetComponent<Rigidbody2D>().velocity = col.gameObject.GetComponent<Rigidbody2D>().velocity / 10;
			}
			col.transform.SetParent(GameObject.Find("Collectibles").transform, false);
			face.GetComponent<SpriteRenderer>().sprite = sad;
			Statics.sound.PlayFX(Statics.sound.ELShock);
			GameOver();

		}
		if (col.transform.tag == "BG") {
			if (OnRoomEnter != null) {
				OnRoomEnter(col.GetComponent<RectTransform>());
			}
			currentBG_name = col.name;
			cam.RaycastForRooms();

			if (col.name == "Background_room_1") {
				if (gameProgression == 3) {
					Statics.canvasRenderer.InfoRenderer(null, "Go down even further.");
				}
			}
			if (col.name == "Background_room_Boss_1") {
				gameProgression = 10;
			}
		}

		if (col.tag == "Spike") {
			Statics.sound.PlayFX(Statics.sound.ArrowCollected);
			Statics.gameProgression.Progress();
			face.GetComponent<SpriteRenderer>().sprite = happy;
			PlayerAttack.bullets++;
			if (gameObject.GetComponent<PlayerAttack>().visibleAlready == true) {
				gameObject.GetComponent<PlayerAttack>().bulletCount.text = "x " + PlayerAttack.bullets;
			}

		}
		if (col.name == "BombPickup") {
			PlayerAttack.bombs++;
			Destroy(col.gameObject);
			Statics.canvasRenderer.InfoRenderer("You found a bomb, it will be useful later on.", null);
		}

		if (col.name == "Test") {
			if (i % 2 == 0) {
				print(i % 2 + " " + i);
				ChangeFlappy(true);
				i++;
			}
			else {
				print(i % 2 + " " + i);
				ChangeFlappy(false);
				i++;
			}
			//save.saveScore();
			//print("Saved");
		}

		if (col.tag == "ArrowTrap") {
			previous = face.GetComponent<SpriteRenderer>().sprite;
			face.GetComponent<SpriteRenderer>().sprite = sad;
		}
	}
	private void OnTriggerExit2D(Collider2D col) {
		if (col.transform.tag == "BG") {
			cam.RaycastForRooms();
		}

		if (col.name == "Background_room_1") {
			foreach (GameObject Projectile in spawner.KWProjectiles) {
				Projectile.SetActive(false);
			}
		}
		if (col.tag == "ArrowTrap") {
			face.GetComponent<SpriteRenderer>().sprite = previous;
		}
	}

	public void FloorComplete() {
		doNotMove = true;
		Cursor.visible = true;
		Timer.run = false;
		save.SaveScore();
	}

	bool delEnemies = true;

	public void GameOver() {
		restartButton.SetActive(true);
		quitToMenu.SetActive(true);
		loadButton.SetActive(true);

		doNotMove = true;
		Cursor.visible = true;
		Timer.run = false;

		Statics.camFade.PlayTransition(CamFadeOut.CameraModeChanges.DIM_CAMERA);
		GameOverImg.SetTrigger("Appear");
		Statics.music.StartCoroutine(Statics.music.StopMusic());
		Statics.zoom.canZoom = false;
		StartCoroutine(StopTime());
		gameOver = true;

		EventSystem e = EventSystem.current;
		e.SetSelectedGameObject(quitToMenu);
		if (delEnemies) {
			Destroy(GameObject.Find("Enemies").gameObject);
			delEnemies = false;
		}
	}
	private IEnumerator StopTime() {
		yield return new WaitForSeconds(1);
		Time.timeScale = 0;
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		Statics.mPlayer = null;
	}
}
