using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour, IWeaponType {

	public bool checkTriggers;
	public bool checkColliders;

	public WeaponType weaponType { get; set; } = WeaponType.BOMB;
	public int damage { get; } = 1;


	public AudioClip bombFuse;
	public AudioClip bombExplosion;

	void Start() {
		StartCoroutine(Explode());
	}

	private void OnTriggerEnter2D(Collider2D col) {
		if (checkTriggers && col.gameObject.GetComponent<IDamageable>() != null) {
			col.gameObject.GetComponent<IDamageable>().TakeDamage(col.gameObject, WeaponType.BOMB);
		}
	}

	private void OnCollisionEnter2D(Collision2D col) {
		if (checkColliders && col.gameObject.GetComponent<IDamageable>() != null) {
			col.gameObject.GetComponent<IDamageable>().TakeDamage(col.gameObject, WeaponType.BOMB);
		}
	}

	private IEnumerator Explode() {
		SoundFXHandler.script.PlayFxChannel(0,bombFuse);
		yield return new WaitForSeconds(1.5f);
		GetComponent<Collider2D>().enabled = true;
		SoundFXHandler.script.PlayFxChannel(0,bombExplosion);
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}
}
