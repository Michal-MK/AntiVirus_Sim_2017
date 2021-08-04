using UnityEngine;

public class BossHitDetection : MonoBehaviour, IDamageable {
	public BossHealth hp;

	public WeaponType DamagedBy { get; set; }

	public bool DestroyOnKill => false;

	public bool DeactivateOnKill => true;

	public void TakeDamage(GameObject by, WeaponType type) {
		switch (type) {
			case WeaponType.BULLET: {
				hp.OnCollision(by, gameObject);
				break;
			}
			case WeaponType.BOMB: {
				if (DeactivateOnKill) {
					gameObject.SetActive(false);
				}
				return;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.GetComponent<IWeaponType>() != null) {
			TakeDamage(col.gameObject, col.gameObject.GetComponent<IWeaponType>().WeaponType);
		}
	}
}
