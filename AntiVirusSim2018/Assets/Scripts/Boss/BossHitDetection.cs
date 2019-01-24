using UnityEngine;

public class BossHitDetection : MonoBehaviour, IDamageable {
	public BossHealth hp;

	public WeaponType damagedByType { get; set; }

	public bool onKillDestroy => false;

	public bool onKillDeactivate => true;

	public void TakeDamage(GameObject by, WeaponType type) {
		switch (type) {
			case WeaponType.BULLET: {
				hp.Collided(by, gameObject);
				break;
			}
			case WeaponType.BOMB: {
				if (onKillDeactivate) {
					gameObject.SetActive(false);
				}
				return;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.GetComponent<IWeaponType>() != null) {
			TakeDamage(col.gameObject, col.gameObject.GetComponent<IWeaponType>().weaponType);
		}
	}
}
