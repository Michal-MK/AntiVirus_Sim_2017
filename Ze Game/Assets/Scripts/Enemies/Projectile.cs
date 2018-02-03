using UnityEngine;
using System.Collections;

public class Projectile : Enemy {

	protected float _projectileSpeed = 15;
	protected Rigidbody2D selfRigid;
	protected SpriteRenderer selfRender;

	public float selfDestructTimer = -1;

	protected virtual void OnEnable() {
		selfRigid = GetComponent<Rigidbody2D>();
		selfRender = GetComponent<SpriteRenderer>();
	}

	public void Fire(float delay = 0) {
		if (delay != 0) {
			StartCoroutine(DelayedFire(delay));
		}
		else {
			selfRigid.velocity = transform.up * -_projectileSpeed;
		}
	}

	protected IEnumerator DelayedFire(float optionalDelay) {
		yield return new WaitForSeconds(optionalDelay);
		selfRigid.velocity = transform.up * -_projectileSpeed;
	}

	public IEnumerator Deactivate(float timeTillDestruction) {
		yield return new WaitForSeconds(timeTillDestruction);
		gameObject.SetActive(false);
	}

	protected IEnumerator TimedDestruction(GameObject target, float seconds) {
		yield return new WaitForSeconds(seconds);
		Destroy(target);
	}

	public void SetSprite(Sprite sprite) {
		selfRender.sprite = sprite;
	}

	protected IEnumerator Fade() {
		Color colour;
		for (float f = 0; f < 1; f += Time.deltaTime) {
			colour = new Color(1, 1, 1, 1 - f);
			selfRender.color = colour;
			yield return null;
		}
		gameObject.SetActive(false);
		selfRender.color = new Color(1, 1, 1, 1);
	}

	protected virtual void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "BG") {
			gameObject.SetActive(false);
		}
	}

	protected virtual void OnCollisionEnter2D(Collision2D col) {
		if (col.transform.name == "Block") {
			print("Collided with a block");
		}
	}

	public float projectileSpeed {
		get { return _projectileSpeed; }
		set { _projectileSpeed = value; }
	}
}
