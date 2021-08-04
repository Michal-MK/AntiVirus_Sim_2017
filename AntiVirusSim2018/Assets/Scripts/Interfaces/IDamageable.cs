using UnityEngine;

interface IDamageable {
	WeaponType DamagedBy { get; set; }

	void TakeDamage(GameObject by, WeaponType type);

	bool DeactivateOnKill { get; }
}
