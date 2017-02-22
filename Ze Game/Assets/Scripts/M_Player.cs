using UnityEngine;

public class M_Player : MonoBehaviour {
	public int attemptNr;
	public float RGspeed;
	public float ARRspeed;
	public static bool doNotMove;
	public Vector3 move;
	public GameObject quitButton;
	public GameObject restartButton;
	public GameObject quitToMenu;
	public GameObject continueButton;
	public Spike nr;
	public CameraMovement cam;
	public EnemySpawner spawner;
	public static float distanceToWall;
	public static int gameProgression;
	public static string currentBG_name;
	public Rigidbody2D rg;
	public BossBehaviour boss;


	void Start() {
		restartButton.SetActive(false);
		quitToMenu.SetActive(false);


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

		Canvas_Renderer.script.infoRenderer("Collect the coins and find your first Spike.");

		rg.freezeRotation = true;
	}

	private Vector3 gravity = Vector3.down;// = new Vector3(Random.value, Random.value, 0);
	private Vector3 vel;

	public void Update() {

		RGspeed = 100 * (Time.deltaTime + 3f);
		ARRspeed = 10 * (Time.deltaTime + 1);

		if (cam.inBossRoom || cam.inMaze) {
			RGspeed = RGspeed * 4;
			ARRspeed = ARRspeed * 2;
		}
		if (boss == null || !boss.Attack5) {
			Move();
			ArrowMove();
			//FlapLike();
		}
		else {
			FlapLike();
		}

	}
	//Moving the Character using a Rigidbody 2D
	public void Move() {
		move = new Vector3(0, 0, 0);

		if (doNotMove == false) {
			if (Input.GetAxis("Mouse X") > 0) {
				rg.AddForce(new Vector2(RGspeed * Mathf.Abs(Input.GetAxis("Mouse X")) * 2, 0));
			}

			else if (Input.GetAxis("Mouse X") < 0) {
				rg.AddForce(new Vector2(-RGspeed * Mathf.Abs(Input.GetAxis("Mouse X")) * 2, 0));
			}

			if (Input.GetAxis("Mouse Y") > 0) {
				rg.AddForce(new Vector2(0, RGspeed * Mathf.Abs(Input.GetAxis("Mouse Y")) * 2));
			}

			else if (Input.GetAxis("Mouse Y") < 0) {
				rg.AddForce(new Vector2(0, -RGspeed * Mathf.Abs(Input.GetAxis("Mouse Y")) * 2));
			}
		}
	}

	//Moving the Character using a Keyboard
	public void ArrowMove() {
		move = new Vector3(0, 0, 0);

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
	}

	//Moving the character FlappyBird style
	public void FlapLike() {
		transform.position += vel;
		vel += gravity;
		if (Input.GetMouseButtonDown(1)) {
			vel = new Vector3(0,0.5f,0);
		}
	}

	/* Dprecated move Function
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



	private void OnTriggerEnter2D(Collider2D col) {

		if (col.tag == "Enemy") {
			col.transform.parent = GameObject.Find("Collectibles").transform;
			GameOver();
		}
		if (col.transform.tag == "BG") {
			currentBG_name = col.name;
			cam.raycastForRooms();
			spawner.spawnArrowTrap();

			if (col.name == "Background_room_1") {
				spawner.InvokeRepeatingScript("spawnKillerWall");
				if (gameProgression == 3) {
					Canvas_Renderer.script.infoRenderer("Go down even further");
				}
				else {
					Canvas_Renderer.script.infoRenderer("Find a pressure plate and put that block on it.");
				}
			}

		}
		if (col.name == "Boss1_teleporter" || col.name == "Background_room_Boss_1") {
			gameProgression = 10;
			roomPregression.script.Progress();

		}
		if(col.tag == "SpikeBullet") {
			Spike.spikesCollected++;
			Destroy(col.gameObject);
		}
		if (col.transform.tag == "Spike") {
			roomPregression.script.Progress();

			if (Spike.spikesCollected == 5) {
				Canvas_Renderer.script.infoRenderer("The Spike is gone! " + "Find the teleporter.");
			}
		}
		if(col.tag == "Wall") {
			gravity = Vector3.zero;
		}
	}
	private void OnTriggerExit2D(Collider2D col) {
		if(col.tag == "Wall") {
			gravity = Vector3.down;
		}
	}

	public void FloorComplete() {

		restartButton.SetActive(true);
		quitToMenu.SetActive(true);
		doNotMove = true;
		Cursor.visible = true;
		timer.run = false;
		SaveGame.script.saveScore();
		Time.timeScale = 0;

	}
	public void GameOver() {

		restartButton.SetActive(true);
		quitToMenu.SetActive(true);
		doNotMove = true;
		Cursor.visible = true;
		timer.run = false;
		Time.timeScale = 0;
		Destroy(GameObject.Find("Enemies").gameObject);
	}
}
