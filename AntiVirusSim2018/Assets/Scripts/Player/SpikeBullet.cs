using System.Collections;
using UnityEngine;

public class SpikeBullet : MonoBehaviour, IWeaponType {


	public GameObject bulletPickup;
	public Rigidbody2D rg;
	public float bulletSpeed;
	public float bulletDuration = 1.5f;
	public PlayerAttack player;

	public WeaponType weaponType { get; set; } = WeaponType.BULLET;

	private void OnEnable() {
		rg.velocity = transform.up * bulletSpeed;
		StartCoroutine(StopBullet(bulletDuration));
	}

	private void OnCollisionEnter2D(Collision2D col) {
		print(col.transform.name);
		if (col.transform.tag == "Wall") {
			CreatePickup(col);
		}
		if(col.transform.tag == "SpikeDetectBoss") {
			rg.velocity = Vector3.zero;
			Destroy(gameObject);
		}
	}

	private IEnumerator StopBullet(float duration) {
		yield return new WaitForSeconds(duration);
		CreatePickup();
	}

	private void CreatePickup(Collision2D info = null) {
		rg.velocity = Vector3.zero;
		Vector3 impactPosition;
		if(info!= null) {
			impactPosition = info.contacts[0].point;
		}
		else {
			impactPosition = transform.position;
		}
		GameObject newspikeBullet = Instantiate(bulletPickup, impactPosition, transform.rotation);
		newspikeBullet.transform.parent = GameObject.Find("Collectibles").transform;
		newspikeBullet.name = "FiredBullet";
		newspikeBullet.GetComponent<Collectible>().objectToCheckCollisionWith = player.transform;
		Destroy(gameObject);
	}
}
