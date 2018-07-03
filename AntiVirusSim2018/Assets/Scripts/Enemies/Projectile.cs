using UnityEngine;
using System.Collections;

public class Projectile : Enemy {

	public float projectileSpeed { get; set; } = 15;
	protected Rigidbody2D selfRigid;
	protected SpriteRenderer selfRender;

	protected virtual void OnEnable() {
		selfRigid = GetComponent<Rigidbody2D>();
		selfRender = GetComponent<SpriteRenderer>();
	}

	public void Fire(float delay = 0) {
		if (delay != 0) {
			StartCoroutine(DelayedFire(delay));
		}
		else {
			selfRigid.velocity = transform.up * -projectileSpeed;
		}
	}

	private IEnumerator DelayedFire(float delay) {
		yield return new WaitForSeconds(delay);
		selfRigid.velocity = transform.up * -projectileSpeed;
	}


	protected IEnumerator Fade() {
		Color colour;
		for (float f = 0; f < 1; f += Time.deltaTime) {
			colour = new Color(1, 1, 1, 1 - f);
			selfRender.color = colour;
			yield return null;
		}
		Kill();
		if (isPooled) {
			selfRender.color = new Color(1, 1, 1, 1);
		}
	}
	
	protected virtual void OnTriggerExit2D(Collider2D col) {
		if (col.tag == Igor.Constants.Strings.Tags.BACKGROUND) {
			Kill();
		}
	}
}
