using System.Collections;
using UnityEngine;

public class RedArrow : Projectile {

	public GameObject darkModeLight;

	private const int TOTAL_COLLIDERS = 4;

	private readonly Vector3 particlesPosition = new Vector3(0, 2.93f);

	public override void MapModeSwitch(MapData.MapMode mode) {
		base.MapModeSwitch(mode);
		if (mode == MapData.MapMode.DARK) {
			GameObject g = Instantiate(darkModeLight, transform);
			g.transform.localPosition = new Vector3(0, -1.5f, -1);
		}
		else {
			Destroy(transform.GetChild(0).gameObject);
		}
	}

	public override void Kill() {
		StartCoroutine(KillDelay());
		foreach (CircleCollider2D col in GetComponentsInChildren<CircleCollider2D>()) {
			col.enabled = false;
		}
	}

	private IEnumerator KillDelay() {
		yield return new WaitForSeconds(0.5f);
		GameObject g = new GameObject("Particles holder");
		transform.GetChild(0).SetParent(g.transform);
		yield return new WaitForSeconds(1f);
		g.transform.GetChild(0).SetParent(transform);
		transform.GetChild(0).localPosition = particlesPosition;
		base.Kill();
		Destroy(g);
		foreach (CircleCollider2D col in GetComponentsInChildren<CircleCollider2D>()) {
			col.enabled = true;
		}
	}

	private int collidersOut = 0;
	protected override void OnTriggerExit2D(Collider2D col) {
		collidersOut++;
		if(collidersOut == TOTAL_COLLIDERS) {
			Kill();
			collidersOut = 0;
		}
	}
}
