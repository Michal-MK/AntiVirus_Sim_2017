using Igor.Constants.Strings;
using System.Collections;
using UnityEngine;

public class SpikeBullet : MonoBehaviour, IWeaponType {


	[SerializeField]
	private GameObject bulletPickup = null;
	[SerializeField]
	private Rigidbody2D rg = null;
	[SerializeField]
	private float bulletSpeed = 40;
	[SerializeField]
	private float bulletDuration = 1.5f;

	public WeaponType WeaponType => WeaponType.BULLET;
	public int Damage => 1;

	private void OnEnable() {
		rg.velocity = transform.up * bulletSpeed;
		StartCoroutine(StopBullet(bulletDuration));
	}

	private void OnCollisionEnter2D(Collision2D col) {
		if (col.transform.CompareTag(Tags.WALL)) {
			CreatePickup(col);
		}
		if (col.transform.CompareTag("SpikeDetectBoss")) {
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
		if (info != null) {
			impactPosition = info.contacts[0].point;
		}
		else {
			impactPosition = transform.position;
		}
		GameObject newspikeBullet = Instantiate(bulletPickup, impactPosition, transform.rotation);
		newspikeBullet.transform.parent = GameObject.Find("Collectibles").transform;
		newspikeBullet.name = "FiredBullet";
		newspikeBullet.GetComponent<Collectible>().Collector = Player.Instance.transform;
		Destroy(gameObject);
	}
}
