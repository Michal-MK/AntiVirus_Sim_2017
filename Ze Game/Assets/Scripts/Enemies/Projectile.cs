using UnityEngine;
using System.Collections;

public class Projectile : Enemy {

	private float _projectileSpeed = 15;
	public float timeTillDestruct = -1;

	private Rigidbody2D selfRigid;
	private SpriteRenderer selfRender;

	public Sprite Solid;
	public Sprite Icicle;
	public Sprite Cracked;

	public bool byBoss = false;

	void OnEnable() {
		selfRigid = GetComponent<Rigidbody2D>();
		selfRender = GetComponent<SpriteRenderer>();
	}

	public void Fire() {
		if (byBoss) {
			StartCoroutine(BossAttack());
		}
		else {
			selfRigid.velocity = transform.up * -_projectileSpeed;
		}
	}

	private IEnumerator BossAttack() {
		yield return new WaitForSeconds(1);
		selfRigid.velocity = transform.up * -_projectileSpeed;
	}

	public IEnumerator SelfDestruct(float timeTillDestruction) {
		yield return new WaitForSeconds(timeTillDestruction);
		gameObject.SetActive(false);
	}

	public void SetSprite(Sprite sprite) {
		selfRender.sprite = sprite;
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

	public float projectileSpeed {
		get { return _projectileSpeed; }
		set { _projectileSpeed = value; }
	}
}
