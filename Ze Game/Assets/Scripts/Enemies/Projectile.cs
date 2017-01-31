using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public static float projectileSpeed = 15;
	public bool ready = false;
	public static bool spawnedByAvoidance = false;
	public Rigidbody2D self;

	void OnEnable() {
		ready = true;
		self.velocity = transform.rotation * Vector3.down * projectileSpeed;
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

	void OnDisable(){
		ready = false;
	}
}
