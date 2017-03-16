using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class M_Player : MonoBehaviour {
	public int attemptNr;
	public float Speed;
	public static bool doNotMove;
	public Vector3 move;
	public GameObject quitButton;
	public GameObject restartButton;
	public GameObject quitToMenu;
	public GameObject TransitionCam;
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

	private int mode = 0;
	public float gravity;
	public float UpVelocity;
	public float linearDrag;
	private bool doFlappy = false;
	private int attempts;

	void Start() {
		Cursor.lockState = CursorLockMode.Confined;
		restartButton.SetActive(false);
		quitToMenu.SetActive(false);




		attempts = PlayerPrefs.GetInt("Attempts");

		if (PlayerPrefs.GetInt("difficulty") == 0) {
			attemptNr = 10;
		}
		if (PlayerPrefs.GetInt("difficulty") == 1) {
			attemptNr = 21;
		}
		if (PlayerPrefs.GetInt("difficulty") == 2) {
			attemptNr = 32;
		}
		if (PlayerPrefs.GetInt("difficulty") == 3) {
			attemptNr = 43;
		}
		if (PlayerPrefs.GetInt("difficulty") == 4) {
			attemptNr = 54;
		}
		StartCoroutine(DelayIntro());
		rg.freezeRotation = true;
	}

	private IEnumerator DelayIntro() {
		yield return new WaitForSeconds(1);
		attempts++;
		Canvas_Renderer.script.infoRenderer("Welcome! \n" +
											"This is your " + attempts + ". attempt. \n\n" +
											"This box will appear only when I have something important to say, otherwise look for information in the upper left corner, so it is less disruptive. \n",
											"Good luck & Have fun!");
		PlayerPrefs.SetInt("Attempts", attempts);
	}

	private void FixedUpdate() {

		switch (mode) {

			case 0:
			//Move();
			ArrowMove();
			doFlappy = false;
			break;

			case 1:
			doFlappy = true;
			break;
		}

	}
	private void Update() {

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
		//move = new Vector3(0, 0, 0);

		if (doNotMove == false) {


			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
				up = true;
				if (!cam.inBossRoom && !cam.inMaze) {
					rg.AddForce(new Vector2(0, 1 * Speed));
				}
				else if (cam.inBossRoom) {
					rg.AddForce(new Vector2(0, 1 * Speed) * 5);
				}
				else if (cam.inMaze) {
					rg.AddForce(new Vector2(0, 1 * Speed) * 4);
				}
			}
			else {
				up = false;
			}

			if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
				right = true;
				if (!cam.inBossRoom && !cam.inMaze) {
					rg.AddForce(new Vector2(1 * Speed, 0));
				}
				else if (cam.inBossRoom) {
					rg.AddForce(new Vector2(1 * Speed, 0) * 5);
				}
				else if (cam.inMaze) {
					rg.AddForce(new Vector2(1 * Speed, 0) * 4);
				}
			}
			else {
				right = false;
			}

			if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
				down = true;
				if (!cam.inBossRoom && !cam.inMaze) {
					rg.AddForce(new Vector2(0, -1 * Speed));
				}
				else if (cam.inBossRoom) {
					rg.AddForce(new Vector2(0, -1 * Speed) * 5);
				}
				else if (cam.inMaze) {
					rg.AddForce(new Vector2(0, -1 * Speed) * 4);
				}
			}
			else {
				down = false;
			}

			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
				left = true;
				if (!cam.inBossRoom && !cam.inMaze) {
					rg.AddForce(new Vector2(-1 * Speed, 0));
				}
				else if (cam.inBossRoom) {
					rg.AddForce(new Vector2(-1 * Speed, 0) * 5);
				}
				else if (cam.inMaze) {
					rg.AddForce(new Vector2(-1 * Speed, 0) * 4);
				}
			}
			else {
				left = false;
			}
			if (!up && !right && !down && !left) {
				rg.velocity = Vector2.zero;
			}

		}
		//Old movement script
		//
		/*
		if (Input.GetKey(KeyCode.UpArrow)) {

			distanceToWall = Mathf.Infinity;


			Debug.DrawRay(transform.position, Vector2.up * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll((Vector2)transform.position, Vector2.up, 100);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall" || hits.transform.tag == "Wall/Door") {
					distanceToWall = hits.distance;
					break;
				}

			}
			if (distanceToWall > 2.1f || distanceToWall == Mathf.Infinity) {
				move.y = 1;
			}
			else {
				move.y = distanceToWall - 2 * transform.localScale.y;
			}
		}

		if (Input.GetKey(KeyCode.DownArrow)) {
			distanceToWall = Mathf.Infinity;


			Debug.DrawRay(transform.position, Vector2.down * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll((Vector2)transform.position, Vector2.down, 100);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall" || hits.transform.tag == "Wall/Door") {
					distanceToWall = hits.distance;
					break;
				}

			}
			if (distanceToWall > 2.1f || distanceToWall == Mathf.Infinity) {
				move.y = -1;
			}
			else {
				move.y = -distanceToWall + 2 * transform.localScale.y;
			}
		}

		if (Input.GetKey(KeyCode.RightArrow)) {
			distanceToWall = Mathf.Infinity;


			Debug.DrawRay(transform.position, Vector2.right * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll((Vector2)transform.position, Vector2.right, 100);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall" || hits.transform.tag == "Wall/Door") {
					distanceToWall = hits.distance;
					break;
				}

			}
			if (distanceToWall > 2.1f || distanceToWall == Mathf.Infinity) {
				move.x = 1;
			}
			else {
				move.x = distanceToWall - 2 * transform.localScale.x;
			}
		}

		if (Input.GetKey(KeyCode.LeftArrow)) {
			distanceToWall = Mathf.Infinity;


			Debug.DrawRay(transform.position, Vector2.left * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll((Vector2)transform.position, Vector2.left, 100);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall" || hits.transform.tag == "Wall/Door") {
					distanceToWall = hits.distance;
					break;
				}

			}
			if (distanceToWall > 2.1f || distanceToWall == Mathf.Infinity) {
				move.x = -1;
			}
			else {
				move.x = -distanceToWall + 2 * transform.localScale.x;
			}
		}
		if (doNotMove == false) {
			gameObject.transform.position += move * Time.deltaTime * ARRspeed;
		}
		*/
	}
	//

	//Moving the character FlappyBird style
	public void ChangeFlappy(bool start = false) {
		switch (start) {
			case true:
			rg.gravityScale = gravity;
			rg.drag = 0;
			mode = 1;
			break;


			case false:
			rg.gravityScale = 0;
			rg.drag = linearDrag;
			mode = 0;
			break;
		}
	}
	public void Flappy() {
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			rg.velocity = new Vector2(0, UpVelocity);
		}

	}
	//

	/* Deprecated move Function
	public void Movement() {
		move = new Vector3(0, 0, 0);

		if (Input.GetAxis("Mouse X") > 0) {

			distanceToWall = Mathf.Infinity;


			Debug.DrawRay(transform.position, Vector2.right * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll((Vector2)transform.position, Vector2.right, 100);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall" || hits.transform.tag == "Wall/Door") {
					distanceToWall = hits.distance;
					break;
				}

			}
			float totalDist = Input.GetAxis("Mouse X") * Time.smoothDeltaTime * speed;
			if (totalDist >= distanceToWall - 2) {
				move.x = distanceToWall - 2;
			}
			else {
				move.x = Input.GetAxis("Mouse X") / 2;
			}

		}
		else if (Input.GetAxis("Mouse X") < 0) {

			distanceToWall = Mathf.Infinity;


			Debug.DrawRay(transform.position, Vector2.left * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll((Vector2)transform.position, Vector2.left, 100);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall" || hits.transform.tag == "Wall/Door") {
					distanceToWall = hits.distance;
					break;
				}

			}
			float totalDist = Mathf.Abs(Input.GetAxis("Mouse X") * Time.smoothDeltaTime * speed);
			if (totalDist >= distanceToWall - 2) {
				move.x = -distanceToWall + 2;
			}
			else {
				move.x = Input.GetAxis("Mouse X") / 2;
			}



		}
		if (Input.GetAxis("Mouse Y") > 0) {

			distanceToWall = Mathf.Infinity;


			Debug.DrawRay(transform.position, Vector2.up * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll((Vector2)transform.position, Vector2.up, 100);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall" || hits.transform.tag == "Wall/Door") {
					distanceToWall = hits.distance;
					break;
				}

			}
			float totalDist = Input.GetAxis("Mouse Y") * Time.smoothDeltaTime * speed;
			if (totalDist >= distanceToWall - 2) {
				move.y = distanceToWall - 2;
			}
			else {
				move.y = Input.GetAxis("Mouse Y") / 2;
			}

		}
		else if (Input.GetAxis("Mouse Y") < 0) {

			distanceToWall = Mathf.Infinity;


			Debug.DrawRay(transform.position, Vector2.down * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll((Vector2)transform.position, Vector2.down, 100);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall" || hits.transform.tag == "Wall/Door") {
					distanceToWall = hits.distance;
					break;
				}

			}
			float totalDist = Mathf.Abs(Input.GetAxis("Mouse Y") * Time.smoothDeltaTime * speed);
			if (totalDist >= distanceToWall - 2) {
				move.y = -distanceToWall + 2;
			}
			else {
				move.y = Input.GetAxis("Mouse Y") / 2;
			}
		}
		if (doNotMove == false) {
			gameObject.transform.position += move * Time.deltaTime * speed * 2;
		}
	}
	*/

	private bool once = true;
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.name == "killerblock") {
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ELShock);
		}
		if (once && collision.transform.name == "Block") {
			guide.enableGuide();
			guide.Recalculate(GameObject.Find("Pressure_Plate"), true);
			once = false;
		}
		if (collision.transform.tag == "Enemy") {
			if (collision.gameObject.GetComponent<Rigidbody2D>() != null) {
				gameObject.GetComponent<CircleCollider2D>().enabled = false;
				collision.gameObject.GetComponent<Rigidbody2D>().velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity / 10;
			}
			collision.transform.parent = GameObject.Find("Collectibles").transform;
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ELShock);
			GameOver();
		}
	}

	private void OnTriggerEnter2D(Collider2D col) {

		if (col.tag == "Enemy") {
			if(col.gameObject.GetComponent<Rigidbody2D>() != null) {
				col.gameObject.GetComponent<Rigidbody2D>().velocity = col.gameObject.GetComponent<Rigidbody2D>().velocity / 10;
			}
			//col.transform.parent = GameObject.Find("Collectibles").transform;
			col.transform.SetParent(GameObject.Find("Collectibles").transform, false);
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ELShock);
			GameOver();

		}
		if (col.transform.tag == "BG") {
			currentBG_name = col.name;
			cam.raycastForRooms();
			spawner.spawnArrowTrap();

			if (col.name == "Background_room_1") {
				AudioHandler.script.MusicTransition(AudioHandler.script.room2);
				spawner.InvokeRepeating("spawnKillerWall", 0, 0.7f);
				if (gameProgression == 3) {
					Canvas_Renderer.script.infoRenderer(null, "Go down even further.");
				}
			}

		}
		if (col.name == "Background_room_Boss_1") {
			gameProgression = 10;
			roomPregression.script.Progress();

		}
		if (col.transform.tag == "Spike") {
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowCollected);
			roomPregression.script.Progress();
			PlayerAttack.bullets++;
			if (gameObject.GetComponent<PlayerAttack>().visibleAlready == true) {
				gameObject.GetComponent<PlayerAttack>().bulletCount.text = "x " + PlayerAttack.bullets;
			}

		}
		if (col.name == "BombPickup") {
			PlayerAttack.bombs++;
			Destroy(col.gameObject);
			Canvas_Renderer.script.infoRenderer("You found a bomb, it will be useful later on.", null);
		}
		if (col.name == "Test") {
			save.saveScore();
			print("Saved");
		}
	}
	private void OnTriggerExit2D(Collider2D col) {
		if (col.name == "Background_room_1") {
			foreach (GameObject Projectile in spawner.KWProjectiles) {
				Projectile.SetActive(false);
			}
		}
	}


	public void FloorComplete() {

		doNotMove = true;
		Cursor.visible = true;
		timer.run = false;
		save.saveScore();

	}

	bool delEnemies = true;

	public void GameOver() {
		restartButton.SetActive(true);
		quitToMenu.SetActive(true);
		doNotMove = true;
		Cursor.visible = true;
		timer.run = false;
		TransitionCam.GetComponent<Animator>().Play("DimCamera");
		GameOverImg.SetTrigger("Appear");
		AudioHandler.script.MusicTransition(null);
		if (delEnemies) {
			Destroy(GameObject.Find("Enemies").gameObject);
			delEnemies = false;
		}
		StartCoroutine(PauseAfterAudioFade());


	}
	private IEnumerator PauseAfterAudioFade() {
		yield return new WaitUntil(() => AudioHandler.script.sound.volume == 0);
		Time.timeScale = 0;
	}

}