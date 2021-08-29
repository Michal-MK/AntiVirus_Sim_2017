using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Igor.Constants.Strings;
using UnityEngine;

public class Icicle : Projectile {

	public Sprite icicleSprite;
	public Sprite crackedIcicleSprite;

	public GameObject emmitter;
	public GameObject darkModeLight;

	private int hitCount = 1;

	private void Start() {
		GetComponent<MeshRenderer>().sortingLayerName = "Enemy";
		GetComponent<MeshRenderer>().sortingOrder = 1;
	}

	private IEnumerator DelayClear() {
		GetComponent<PolygonCollider2D>().enabled = false;
		GetComponent<MeshRenderer>().enabled = false;
		yield return FadeFrag(GetComponent<Explodable>().fragments);
		gameObject.SetActive(false);
		foreach (GameObject fragment in GetComponent<Explodable>().fragments) {
			Destroy(fragment);
		}
		GetComponent<Explodable>().fragments.Clear();
		GetComponent<PolygonCollider2D>().enabled = true;
		GetComponent<MeshRenderer>().enabled = true;
		gameObject.tag = Tags.ENEMY_INACTIVE;
	}

	private IEnumerator FadeFrag(List<GameObject> frags) {
		MeshRenderer refRender = frags[0].GetComponent<MeshRenderer>();
		Color c = refRender.material.color;
		while (c.a > 0) {
			foreach (GameObject frag in frags) {
				float decrease = Time.fixedDeltaTime / 32;
				c = frag.GetComponent<MeshRenderer>().material.color = new Color(c.a - decrease, c.r, c.b, c.g);
				print($"Dec: {decrease}, Curr: {c.a}");
			}
			yield return new WaitForFixedUpdate();
		}
	}

	private void Explode() {
		Explodable explodable = GetComponent<Explodable>();

		explodable.Explode();

		foreach (MeshRenderer m in explodable.fragments.Select(s => s.GetComponent<MeshRenderer>())) {
			m.sortingLayerName = "Enemy";
			m.sortingOrder = 1;

		}
		foreach (Rigidbody2D rg in explodable.fragments.Select(s => s.GetComponent<Rigidbody2D>())) {
			rg.gravityScale = 0;
			rg.mass = 0.2f;
			rg.drag = 0.2f;
			rg.angularDrag = 0.4f;
		}

		StartCoroutine(DelayClear());
	}

	private void OnCollisionEnter2D(Collision2D col) {
		if (col.transform.name == ObjNames.BLOCK) {
			Explode();
		}
		if (col.transform.name == ObjNames.PRESSURE_PLATE_WALL) {
			FadeSetup();
		}
	}

	private void FadeSetup() {
		gameObject.tag = Tags.ENEMY_INACTIVE;
		selfRigid.velocity /= 1.4f;
		hitCount++;
		StartCoroutine(Fade());
	}

	public override void MapModeSwitch(MapMode mode) {
		base.MapModeSwitch(mode);
		if (mode == MapMode.DARK) {
			GameObject g = Instantiate(darkModeLight, transform);
			g.transform.localPosition = new Vector3(-0.5f, -4.5f, -4);
		}
		else {
			Destroy(transform.GetChild(0).gameObject);
		}
	}

	protected void OnDisable() {
		hitCount = 1;
	}
}