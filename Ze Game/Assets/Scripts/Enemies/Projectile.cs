using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public float speed = 10;
	public bool ready = false;

	void OnEnable(){
		ready = true;
	}


	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "Player") {
			gameObject.transform.SetParent (GameObject.Find ("Spikes").transform);
			M_Player.gameProgression = -1;
		}

		if (coll.tag == "Wall" || coll.tag == "Wall/Door") {
			gameObject.SetActive (false);

		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.tag == "BG") { 
			gameObject.SetActive (false);

		}
	}
	void Update(){
		if (ready == true) {
			transform.position += transform.rotation * new Vector3 (0, -1, 0) * Time.deltaTime * speed;
		}
	}
	void OnDisable(){
		ready = false;
	}
}
