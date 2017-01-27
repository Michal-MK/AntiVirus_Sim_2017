using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public static float projectileSpeed = 10;
	public bool ready = false;
	public static bool spawnedByAvoidance = false;

	void OnEnable(){
		ready = true;
	}


	void OnTriggerEnter2D(Collider2D coll){

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
			transform.position += transform.rotation * new Vector3 (0, -1, 0) * projectileSpeed * Time.deltaTime;
		}
	}

	void OnDisable(){
		ready = false;
	}
}
