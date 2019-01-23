using UnityEngine;

[RequireComponent(typeof(DamageConfig))]
public class BossHitDetection : MonoBehaviour, IDamageable {
	public BossHealth hp;

	public WeaponType damagedByType { get; set; }

	private DamageConfig configuration;

	private void Start() {
		configuration = GetComponent<DamageConfig>();
	}


	public void TakeDamage(GameObject by, WeaponType type) {
		switch (type) {
			case WeaponType.BULLET: {
				hp.Collided(by, gameObject);
				break;
			}
			case WeaponType.BOMB: {
				if (configuration.destroy) {
					Destroy(gameObject);
				}
				else if (configuration.deactivate) {
					gameObject.SetActive(false);
				}
				else if (configuration.destroy && configuration.deactivate) {
					throw new System.InvalidOperationException("Attempting to both deactivate and destroy object " + gameObject.name);
				}
				break;
			}
		}
	}


	private void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.GetComponent<IWeaponType>() != null) {
			TakeDamage(col.gameObject, col.gameObject.GetComponent<IWeaponType>().weaponType);
		}
	}


}
