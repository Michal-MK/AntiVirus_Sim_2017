using UnityEngine;
using System.Collections;

public class Projectile : Enemy {
	public static float projectileSpeed = 15;
	public float timeTillDestruct = -1337;

	public bool ready = false;

	private Rigidbody2D selfRigid;
	private EdgeCollider2D selfCol;
	private SpriteRenderer selfRender;

	public Sprite Solid;
	public Sprite Trigger;
	public Sprite Icicle;
	public Sprite Cracked;

	public bool disableCollisions = false;

	public static bool spawnedByAvoidance = false;
	public static bool spawnedByKillerWall = false;

	public bool byBoss = false;
	public bool byKillerWall;

	private void Start() {
		selfRigid = gameObject.GetComponent<Rigidbody2D>();
		selfCol = gameObject.GetComponent<EdgeCollider2D>();
		selfRender = gameObject.GetComponent<SpriteRenderer>();
	}


	void OnEnable() {
		selfRigid = gameObject.GetComponent<Rigidbody2D>();
		selfCol = gameObject.GetComponent<EdgeCollider2D>();
		selfRender = gameObject.GetComponent<SpriteRenderer>();

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
		else {
			StartCoroutine(BossAttack());
		}
	}

	private IEnumerator BossAttack() {
		yield return new WaitForSeconds(1);
		selfRigid.velocity = transform.rotation * Vector3.down * projectileSpeed;
		StopCoroutine(BossAttack());
	}

	public IEnumerator SelfDestruct(float timeTillDestruction) {
		yield return new WaitForSeconds(timeTillDestruction);
		if (spawnedByKillerWall == true) {
			selfRender.sprite = Icicle;
			gameObject.tag = "Enemy";
		}
		gameObject.SetActive(false);
	}


	private void OnTriggerEnter2D(Collider2D col) {
		if (!disableCollisions) {
			if (col.tag == "Wall" || col.tag == "Wall/Door") {
				selfRender.sprite = Trigger;
				if (spawnedByKillerWall) {
					selfRender.sprite = Icicle;
				}
				gameObject.SetActive(false);
			}
			if (col.name == "Blocker") {
				gameObject.SetActive(false);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D col) {
		if (!disableCollisions) {
			if (col.tag == "BG") {
				if (!spawnedByKillerWall) {
					selfCol.isTrigger = true;
					selfRender.sprite = Trigger;
				}

				if (spawnedByKillerWall) {
					selfRender.sprite = Icicle;
				}
				gameObject.SetActive(false);
			}
		}

	}

	private void OnCollisionEnter2D(Collision2D col) {
		if (!disableCollisions) {
			if (col.transform.tag == "Wall" || col.transform.tag == "Wall/Door") {
				gameObject.SetActive(false);
			}
			if (col.transform.name == "Block") {
				selfRender.sprite = Cracked;
				gameObject.tag = "EnemyInactive";
				StartCoroutine(SelfDestruct(2));
			}
			if (col.transform.name == "Blocker") {
				gameObject.SetActive(false);
			}
		}
	}


	void OnDisable() {
		ready = false;
		byBoss = false;
	}
}
