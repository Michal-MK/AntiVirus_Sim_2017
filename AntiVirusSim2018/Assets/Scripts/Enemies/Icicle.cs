using Igor.Constants.Strings;
using UnityEngine;

public class Icicle : Projectile {

	public Sprite icicleSprite;
	public Sprite crackedIcicleSprite;

	public GameObject emmitter;
	public GameObject darkModeLight;

	private int hitCount = 1;

	private void OnCollisionEnter2D(Collision2D col) {
		if (col.transform.name == ObjNames.BLOCK) {
			FadeSetup();
		}
		if (col.transform.name == ObjNames.PRESSURE_PLATE_WALL) {
			FadeSetup();
		}
	}

	private void FadeSetup() {
		selfRender.sprite = crackedIcicleSprite;
		gameObject.tag = Tags.ENEMY_INACTIVE;
		StartCoroutine(Fade());
		SpawnParticles();
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
		selfRender.sprite = icicleSprite;
		hitCount = 1;
	}

	private void SpawnParticles() {
		ParticleSystem ps = Instantiate(emmitter, transform.position, transform.rotation).GetComponent<ParticleSystem>();
		ps.Emit(Random.Range(8, 16) / hitCount);
	}
}
