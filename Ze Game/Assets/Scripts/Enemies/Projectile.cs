using UnityEngine;
using System.Collections;

public class Projectile : Enemy {

	public static float projectileSpeed = 15;
	public float timeTillDestruct = -1;

	private Rigidbody2D selfRigid;
	public SpriteRenderer selfRender;

	public Sprite Solid;
	public Sprite Icicle;
	public Sprite Cracked;

	public bool byBoss = false;

	void OnEnable() {
		selfRigid = gameObject.GetComponent<Rigidbody2D>();
		if (byBoss) {
			StartCoroutine(BossAttack());
		}
	}

	public void Fire() {
		selfRigid.velocity = transform.up * -projectileSpeed;
	}

	private IEnumerator BossAttack() {
		yield return new WaitForSeconds(1);
		selfRigid.velocity = transform.up * -projectileSpeed;
	}

	public IEnumerator SelfDestruct(float timeTillDestruction) {
		yield return new WaitForSeconds(timeTillDestruction);
		gameObject.SetActive(false);
	}


	private void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Wall" || col.tag == "Wall/Door") {
			gameObject.SetActive(false);
		}
		if (col.name == "Blocker") {
			gameObject.SetActive(false);
		}
	}

	private void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "BG") {
			gameObject.SetActive(false);
		}
	}

	private void OnCollisionEnter2D(Collision2D col) {
		if (col.transform.name == "Block") {
			selfRender.sprite = Cracked;
			gameObject.tag = "EnemyInactive";
			StartCoroutine(SelfDestruct(2));
		}
	}

	void OnDisable() {
		byBoss = false;
	}
}
