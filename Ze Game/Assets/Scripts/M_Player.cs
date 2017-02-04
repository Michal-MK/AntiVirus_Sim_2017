using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class M_Player : MonoBehaviour {
	public int attemptNr;
	public float speed = 10f;
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
	bool experimentalMovement = false;


	void Start() {
		restartButton.SetActive (false);
		quitToMenu.SetActive(false);


		if (PlayerPrefs.GetInt ("difficulty") == 0) {
			attemptNr = 10;
		}
		if (PlayerPrefs.GetInt ("difficulty") == 1) {
			attemptNr = 21;
		}
		if (PlayerPrefs.GetInt ("difficulty") == 2) {
			attemptNr = 32;
		}
		if (PlayerPrefs.GetInt ("difficulty") == 3) {
			attemptNr = 43;
		}
		if (PlayerPrefs.GetInt ("difficulty") == 4) {
			attemptNr = 54;
		}

		Canvas_Renderer.script.infoRenderer("Collect the coins and find your first Spike.");

		rg.freezeRotation = true;

		if(PlayerPrefs.GetInt("exp.") == 1) {
			experimentalMovement = true;
		}
		else {
			experimentalMovement = false;
		}

	}


	public void FixedUpdate() {

		if (experimentalMovement) {
			ExpMove();
			ArrowMove();
		}
		else {
			Movement();
			ArrowMove();
		}
	}

	public void ExpMove() {
		move = new Vector3(0, 0, 0);

		if (Input.GetAxis("Mouse X") > 0) {
			rg.AddForce(new Vector2(200 * Mathf.Abs(Input.GetAxis("Mouse X"))*2, 0));
		}

		else if (Input.GetAxis("Mouse X") < 0) {
			rg.AddForce(new Vector2(- 200 * Mathf.Abs(Input.GetAxis("Mouse X"))*2, 0));
		}

		if (Input.GetAxis("Mouse Y") > 0) {
			rg.AddForce(new Vector2(0, + 200 * Mathf.Abs(Input.GetAxis("Mouse Y"))*2));
		}

		else if (Input.GetAxis("Mouse Y") < 0) {
			rg.AddForce(new Vector2(0, - 200 * Mathf.Abs(Input.GetAxis("Mouse Y"))*2));

		}
	}

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
			gameObject.transform.position += move * Time.deltaTime * speed;
		}
	}


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
			float totalDist = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
			if (totalDist >= distanceToWall - 2) {
				move.x = distanceToWall - 2;
			}
			else {
				move.x = Input.GetAxis("Mouse X")/2;
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
			float totalDist = Mathf.Abs(Input.GetAxis("Mouse X") * Time.deltaTime * speed);
			if (totalDist >= distanceToWall - 2) {
				move.x = -distanceToWall + 2;
			}
			else {
				move.x = Input.GetAxis("Mouse X")/2;
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
			float totalDist = Input.GetAxis("Mouse Y") * Time.deltaTime * speed;
			if (totalDist >= distanceToWall - 2) {
				move.y = distanceToWall - 2;
			}
			else {
				move.y = Input.GetAxis("Mouse Y")/2;
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
			float totalDist = Mathf.Abs(Input.GetAxis("Mouse Y") * Time.deltaTime * speed);
			if (totalDist >= distanceToWall - 2) {
				move.y = -distanceToWall + 2;
			}
			else {
				move.y = Input.GetAxis("Mouse Y")/2;
			}
		}
		if (doNotMove == false) {
			gameObject.transform.position += move * Time.deltaTime * speed*2;
		}
	}




	void OnTriggerEnter2D (Collider2D col){

		if (col.tag == "Enemy") {
			col.transform.parent = GameObject.Find("Collectibles").transform;
			GameOver ();
		}
		if (col.transform.tag == "BG") {
			currentBG_name = col.name;
			cam.raycastForRooms();
			spawner.spawnArrowTrap();

			if (col.name == "Background_room_1") {
				spawner.InvokeRepeatingScript("spawnKillerWall");
				Canvas_Renderer.script.infoRenderer("Find a pressure plate and put that block on it.");
				if (gameProgression == 3) {
					Canvas_Renderer.script.infoRenderer("Go down even further");
				}
			}


		}
		if (col.name == "Boss1_teleporter") {
			gameProgression = 10;
			roomPregression.script.Progress ();
			Canvas_Renderer.script.Disable ();

		}
		if (col.transform.tag == "Spike") {
			roomPregression.script.Progress ();

			if (Spike.spikesCollected == 5) {
				Canvas_Renderer.script.infoRenderer("The Spike is gone! " + "Find the teleporter.");
			}
		}
	}
		

	public void FloorComlpete(){

		restartButton.SetActive (true);
		quitToMenu.SetActive (true);
		doNotMove = true;
		Cursor.visible = true;
		timer.run = false;
        SaveGame.script.saveScore();
		Time.timeScale = 0;

	}
	public void GameOver(){
		
		restartButton.SetActive (true);
		quitToMenu.SetActive (true);
        doNotMove = true;
		Cursor.visible = true;
		timer.run = false;
		Time.timeScale = 0;	
		Destroy (GameObject.Find ("Enemies").gameObject);
	}
}
