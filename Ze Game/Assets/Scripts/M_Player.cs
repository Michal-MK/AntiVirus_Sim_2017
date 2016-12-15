using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

	public class M_Player : MonoBehaviour {
	public int attemptNr;
	public float speed = 20f;
	public static bool doNotMove;
	public Vector3 move;
	public GameObject quitButton;
	public GameObject restartButton;
	public GameObject quitToMenu;
	public Spike nr;
	public CameraMovement cam;
	private bool toggle;



	void Start() {
		restartButton.SetActive (false);
		quitToMenu.SetActive (false);
		attemptNr = 11;

	}

	void FixedUpdate () {



		move = new Vector3 (0, 0, 0);

//		if (stepRight == 1 && Input.GetAxis ("Mouse X") > 0) {
//			move.x = + Input.GetAxis ("Mouse X");
//		} 
//		else if (stepLeft == 1 && Input.GetAxis ("Mouse X") < 0) {
//			move.x = + Input.GetAxis ("Mouse X");
//		}
//		if (stepUp == 1 && Input.GetAxis ("Mouse Y") > 0) {
//			move.y = + Input.GetAxis ("Mouse Y");
//		} 
//		else if (stepDown == 1 && Input.GetAxis ("Mouse Y") < 0) {
//			move.y = + Input.GetAxis ("Mouse Y");
//		}


		if (Input.GetKey (KeyCode.UpArrow)) {

			float distanceToWall = Mathf.Infinity;


			Debug.DrawRay (transform.position, Vector2.up * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, Vector2.up,1000);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall") {
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
			float distanceToWall = Mathf.Infinity;


			Debug.DrawRay (transform.position, Vector2.down * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, Vector2.down,1000);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall") {
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
			float distanceToWall = Mathf.Infinity;


			Debug.DrawRay (transform.position, Vector2.right * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, Vector2.right,1000);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall") {
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
			float distanceToWall = Mathf.Infinity;


			Debug.DrawRay (transform.position, Vector2.left * 100, Color.red);
			RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, Vector2.left,1000);
			foreach (RaycastHit2D hits in result) {
				if (hits.transform.tag == "Wall") {
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
			
			} else {
				M_Player.doNotMove = false;
				Cursor.visible = false;
				timer.run = true;
				restartButton.SetActive (false);
				quitToMenu.SetActive (false);
			
			}
		}
	}
	void OnTriggerEnter2D (Collider2D col){
		if (col.name == "killerblock") {
			GameOver ();
		}
	}

	public void Rayer() {
		Debug.DrawRay (transform.position, move * 1000, Color.red);
		RaycastHit2D[] result = Physics2D.RaycastAll ((Vector2)transform.position, (Vector2)move,Mathf.Infinity);
		if (result.Length > 1) {
			Debug.Log (result[1].distance);
		}

		return;
	}

	public void GameOver(){

		if (nr.i == 5) {
			

			restartButton.SetActive (true);
			quitToMenu.SetActive (true);
			M_Player.doNotMove = true;
			Cursor.visible = true;
			timer.run = false;
			nr.SendMessage ("saveScore");

		}
		else{

			restartButton.SetActive (true);
			quitToMenu.SetActive (true);
			M_Player.doNotMove = true;
			Cursor.visible = true;
			timer.run = false;
				
		}
	}
}
