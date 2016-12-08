using UnityEngine;
using System.Collections;

	public class M_Player : MonoBehaviour {
	public int attemptNr;
	public float speed = 20f;
	public bool step;
	public int stepUp = 1;
	public int stepDown = 1;
	public int stepLeft = 1;
	public int stepRight = 1;
	public static bool doNotMove;
	public Vector3 move;
	public GameObject quitButton;
	public GameObject restartButton;
	public GameObject quitToMenu;
	public Spike nr;

	void Start() {
		quitButton.SetActive (false);
		restartButton.SetActive (false);
		quitToMenu.SetActive (false);
		attemptNr = 11;
	}

	void FixedUpdate () {
		move = new Vector3 (0, 0, 0);
		if (stepRight == 1 && Input.GetAxis ("Mouse X") > 0) {
			move.x = Input.GetAxis ("Mouse X");
		} 
		else if (stepLeft == 1 && Input.GetAxis ("Mouse X") < 0) {
			move.x = Input.GetAxis ("Mouse X");
		}
		if (stepUp == 1 && Input.GetAxis ("Mouse Y") > 0) {
			move.y = Input.GetAxis ("Mouse Y");
		} 
		else if (stepDown == 1 && Input.GetAxis ("Mouse Y") < 0) {
			move.y = Input.GetAxis ("Mouse Y");
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			move.y = + stepUp;
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			move.y = - stepDown;
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			move.x = + stepRight;
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			move.x = - stepLeft;
		}
			
		if (doNotMove == false) {
			gameObject.transform.position += move *Time.deltaTime* speed;
		} 
	}
	void OnTriggerEnter2D (Collider2D col){
		if (col.name == "killerblock") {
			GameOver();
		}
	}
	public void GameOver(){

		if (nr.i == 5) {
			
			quitButton.SetActive (true);
			restartButton.SetActive (true);
			quitToMenu.SetActive (true);
			M_Player.doNotMove = true;
			Cursor.visible = true;
			timer.run = false;
			nr.SendMessage ("saveScore");

		}
		else{
			quitButton.SetActive (true);
			restartButton.SetActive (true);
			quitToMenu.SetActive (true);
			M_Player.doNotMove = true;
			Cursor.visible = true;
			timer.run = false;
				
		}
	}
}
