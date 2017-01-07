using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

	public class M_Player : MonoBehaviour {
	public int attemptNr;
	public float speed = 10f;
	public static bool doNotMove;
	public Vector3 move;
	public GameObject quitButton;
	public GameObject restartButton;
	public GameObject quitToMenu;
	public Spike nr;
	public CameraMovement cam;
	public EnemySpawner spawner;
	private bool toggle;
	public static float distanceToWall;
	public static int gameProgression;
	public static string currentBG_name;



	void Start() {
		restartButton.SetActive (false);
		quitToMenu.SetActive (false);
		currentBG_name = "";


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

	}


	void FixedUpdate () {
		if (gameProgression == -1) {
			GameOver ();
		}

		move = new Vector3 (0, 0, 0);

		if (Input.GetAxis ("Mouse X") > 0) {
			distanceToWall = Mathf.Infinity;


			Debug.DrawRay (transform.position, Vector2.right * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, Vector2.right,100);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall" || hits.transform.tag == "Wall/Door") {
					distanceToWall = hits.distance;
					break;
				}

			}
			float totalDist = Input.GetAxis ("Mouse X") * Time.deltaTime * speed;
			if (totalDist >= distanceToWall - 2) {
				move.x = distanceToWall - 2;
			} 
			else {
				move.x = Input.GetAxis ("Mouse X");
			}
				
		} 
		else if (Input.GetAxis ("Mouse X") < 0) {

			distanceToWall = Mathf.Infinity;


			Debug.DrawRay (transform.position, Vector2.left * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, Vector2.left,100);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall" || hits.transform.tag == "Wall/Door") {
					distanceToWall = hits.distance;
					break;
				}

			}
			float totalDist = Mathf.Abs(Input.GetAxis ("Mouse X") * Time.deltaTime * speed);
			if (totalDist >= distanceToWall - 2) {
				move.x = -distanceToWall + 2;
			} 
			else {
				move.x = Input.GetAxis ("Mouse X");
			}



		}
		if (Input.GetAxis ("Mouse Y") > 0) {

			distanceToWall = Mathf.Infinity;


			Debug.DrawRay (transform.position, Vector2.up * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, Vector2.up,100);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall" || hits.transform.tag == "Wall/Door") {
					distanceToWall = hits.distance;
					break;
				}

			}
			float totalDist = Input.GetAxis ("Mouse Y") * Time.deltaTime * speed;
			if (totalDist >= distanceToWall - 2) {
				move.y = distanceToWall - 2;
			} 
			else {
				move.y = Input.GetAxis ("Mouse Y");
			}
				
		} 
		else if (Input.GetAxis ("Mouse Y") < 0) {

			distanceToWall = Mathf.Infinity;


			Debug.DrawRay (transform.position, Vector2.down * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, Vector2.down,100);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall" || hits.transform.tag == "Wall/Door") {
					distanceToWall = hits.distance;
					break;
				}

			}
			float totalDist = Mathf.Abs(Input.GetAxis ("Mouse Y") * Time.deltaTime * speed);
			if (totalDist >= distanceToWall - 2) {
				move.y = -distanceToWall + 2;
			} 
			else {
				move.y = Input.GetAxis ("Mouse Y");
			}
				
		}


		if (Input.GetKey (KeyCode.UpArrow)) {

			distanceToWall = Mathf.Infinity;


			Debug.DrawRay (transform.position, Vector2.up * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, Vector2.up,100);
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
				move.y = distanceToWall - 2;
			}
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			distanceToWall = Mathf.Infinity;


			Debug.DrawRay (transform.position, Vector2.down * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, Vector2.down,100);
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
				move.y = -distanceToWall + 2;
			}
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			distanceToWall = Mathf.Infinity;


			Debug.DrawRay (transform.position, Vector2.right * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, Vector2.right,100);
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
				move.x = distanceToWall - 2;
			}
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			distanceToWall = Mathf.Infinity;


			Debug.DrawRay (transform.position, Vector2.left * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, Vector2.left,100);
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
				move.x = -distanceToWall + 2;
			}
		}
		if (doNotMove == false) {
			gameObject.transform.position += move * Time.deltaTime * speed;
		}

		if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene ().buildIndex == 1) {
			toggle = !toggle;

			if (toggle) {
				
				M_Player.doNotMove = true;
				Cursor.visible = true;
				timer.run = false;
				restartButton.SetActive (true);
				quitToMenu.SetActive (true);
				Time.timeScale = 0;
			
			} else {
				M_Player.doNotMove = false;
				Cursor.visible = false;
				timer.run = true;
				restartButton.SetActive (false);
				quitToMenu.SetActive (false);
				Time.timeScale = 1;
			
			}
		}
	}




	void OnTriggerEnter2D (Collider2D col){

		if (col.name == "killerblock") {
			GameOver ();
		}
		if (col.transform.tag == "BG") {
			currentBG_name = col.name;
			cam.SendMessage ("Progress");
			spawner.SendMessage ("spawnArrowTrap");

		}
	}
		

	public void GameOver(){

		if (Spike.i == 5) {
			

			restartButton.SetActive (true);
			quitToMenu.SetActive (true);
			M_Player.doNotMove = true;
			Cursor.visible = true;
			timer.run = false;
			nr.SendMessage ("saveScore");
			Time.timeScale = 0;

		}
		else{

			restartButton.SetActive (true);
			quitToMenu.SetActive (true);
			M_Player.doNotMove = true;
			Cursor.visible = true;
			timer.run = false;
			Time.timeScale = 0;	
			Destroy (GameObject.Find ("Enemies").gameObject);	
		}
	}



//	private IEnumerator coroutine;
//
//	void Start()
//	{
//		print ("Starting " + Time.time);
//		coroutine = WaitAndPrint(2.0f);
//		StartCoroutine(coroutine);
//	}
//
//	// every 2 seconds perform the print()
//	private IEnumerator WaitAndPrint(float waitTime) {
//		while (true) {
//			yield return new WaitForSeconds(waitTime);
//			print("WaitAndPrint " + Time.time);
//		}
//	}
}
