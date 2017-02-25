using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public static float projectileSpeed = 15;
	public bool ready = false;
	public static bool spawnedByAvoidance = false;
	public Rigidbody2D self;
	public bool byBoss = false;
	public float timeTillDestruct = -1337;
	public bool DisableCollisions = false;

	void OnEnable() {
		if (!byBoss) {
			ready = true;
			self.velocity = transform.rotation * Vector3.down * projectileSpeed;
		}
		if (byBoss) {
			StartCoroutine(BossAttack());
		}
		if (timeTillDestruct != -1337) {
			StartCoroutine(SelfDestruct());

		}
	}

	private IEnumerator BossAttack() {
		yield return new WaitForSeconds(1);
		self.velocity = transform.rotation * Vector3.down * projectileSpeed;
		StopCoroutine(BossAttack());
	}
	private IEnumerator SelfDestruct() {
		yield return new WaitForSeconds(timeTillDestruct);
		gameObject.SetActive(false);
	}


	private void OnTriggerEnter2D(Collider2D col){
		if (DisableCollisions == false) {
			if (col.tag == "Wall" || col.tag == "Wall/Door") {
				//print('A');
				gameObject.SetActive(false);

			}
		}
	}

	private void OnTriggerExit2D(Collider2D col){
		if (DisableCollisions == false) {
			if (col.tag == "BG") {
				gameObject.SetActive(false);
				print('B');

			}
		}

	}

	private void OnCollisionEnter2D(Collision2D col) {
		if (DisableCollisions == false) {
			if(col.transform.tag == "Wall" || col.transform.tag == "Wall/Door") {
				gameObject.SetActive(false);
				print('C');

			}
		}
		
	}


	void OnDisable(){
		ready = false;
		byBoss = false;
	}
}
