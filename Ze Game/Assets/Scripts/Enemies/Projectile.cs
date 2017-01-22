using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public static float projectileSpeed = 10;
	public bool ready = false;
	public static bool spawnedByAvoidance = false;

	void OnEnable(){
		ready = true;
		//if (spawnedByAvoidance == true) {
		//	StartCoroutine("increaseSpeed");
		//}
	}


	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "Player") {
			gameObject.transform.SetParent (GameObject.Find ("Collectibles").transform);
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
			transform.position += transform.rotation * new Vector3 (0, -1, 0) * projectileSpeed * Time.deltaTime;
		}
	}
	//private IEnumerator increaseSpeed() {
	//	while (true) {
	//		yield return new WaitForSeconds(1);
	//		projectileSpeed += 0.01f;
	//	}
	//}

	void OnDisable(){
		ready = false;
	}
}
