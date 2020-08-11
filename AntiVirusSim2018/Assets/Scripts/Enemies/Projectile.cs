using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class Projectile : Enemy {

	public float projectileSpeed { get; set; } = 15;
	protected Rigidbody2D selfRigid;
	protected SpriteRenderer selfRender;


	protected virtual void OnEnable() {
		selfRigid = GetComponent<Rigidbody2D>();
		selfRender = GetComponent<SpriteRenderer>();
		Fire();
	}

	public void Fire(float delay = 0) {
		if (delay != 0) {
			selfRigid.velocity = Vector2.zero;
			StartCoroutine(DelayedFire(delay));
		}
		else {
			Movement m = gameObject.GetComponent<Movement>();
			m.direction = transform.up * -projectileSpeed;
			m.MoveAndDestroyOnWallLeave();
		}
	}

	private IEnumerator DelayedFire(float delay) {
		yield return new WaitForSeconds(delay);
		Movement m = gameObject.GetComponent<Movement>();
		m.direction = transform.up * -projectileSpeed;
		m.MoveAndDestroyOnWallLeave();
	}


	protected IEnumerator Fade() {
		Color colour;
		for (float f = 0; f < 1; f += Time.deltaTime) {
			colour = new Color(1, 1, 1, 1 - f);
			selfRender.color = colour;
			yield return null;
		}
		Kill();
		if (IsPooled) {
			selfRender.color = new Color(1, 1, 1, 1);
		}
	}
}
