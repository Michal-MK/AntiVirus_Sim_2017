using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public static float projectileSpeed = 15;
	public float timeTillDestruct = -1337;

	public bool ready = false;

	public Rigidbody2D selfRigid;
	public EdgeCollider2D selfCol;
	public SpriteRenderer selfRender;

	public Sprite Solid;
	public Sprite Trigger;

	


	public bool DisableCollisions = false;
	public static bool spawnedByAvoidance = false;
	public bool byBoss = false;
	public bool byKillerWall;


	void OnEnable() {
		if (!byBoss) {
			if (spawnedByAvoidance) {
				ready = true;
				selfRigid.velocity = transform.rotation * Vector3.down * projectileSpeed * 1.4f;
			}
			else {
				ready = true;
				selfRigid.velocity = transform.rotation * Vector3.down * projectileSpeed;
			}
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
		selfRigid.velocity = transform.rotation * Vector3.down * projectileSpeed;
		StopCoroutine(BossAttack());
	}
	private IEnumerator SelfDestruct() {
		yield return new WaitForSeconds(timeTillDestruct);
		gameObject.SetActive(false);
	}


	private void OnTriggerEnter2D(Collider2D col){
		if (DisableCollisions == false) {
			if (col.tag == "Wall" || col.tag == "Wall/Door") {
				gameObject.SetActive(false);

			}
			if(col.name == "Block") {
				selfRender.sprite = Solid;
				selfCol.isTrigger = false;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D col){
		if (DisableCollisions == false) {
			if (col.tag == "BG") {
				selfCol.isTrigger = true;
				selfRender.sprite = Trigger;
				gameObject.SetActive(false);

			}
		}

	}

	private void OnCollisionEnter2D(Collision2D col) {
		if (DisableCollisions == false) {
			if(col.transform.tag == "Wall" || col.transform.tag == "Wall/Door") {
				selfCol.isTrigger = true;
				selfRender.sprite = Trigger;
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
