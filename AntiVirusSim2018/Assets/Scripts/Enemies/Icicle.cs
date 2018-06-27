using UnityEngine;
using Igor.Constants.Strings;

public class Icicle : Projectile {

	public Sprite icicleSprite;
	public Sprite crackedIcicleSprite;

	public GameObject emmitter;

	private int hitCount = 1;

	protected override void OnCollisionEnter2D(Collision2D col) {
		base.OnCollisionEnter2D(col);
		if (col.transform.name == "Block") {
			selfRender.sprite = crackedIcicleSprite;
			gameObject.tag = Tags.ENEMY_INACTIVE;
			StartCoroutine(Fade());
			SpawnParticles(2);
			selfRigid.velocity /= 1.4f;
			hitCount++;
		}
	}

	protected void OnDisable() {
		selfRender.sprite = icicleSprite;
		hitCount = 1;
	}

	private void SpawnParticles(float time) {
		ParticleSystem ps = Instantiate(emmitter, transform.position, transform.rotation).GetComponent<ParticleSystem>();
		ps.Emit(Random.Range(8, 16) / hitCount);
		SelfDestructIn(time);
	}
}
