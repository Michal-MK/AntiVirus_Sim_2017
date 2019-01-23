using System;
using UnityEngine;

[Flags]
public enum WeaponType {
	BULLET,
	BOMB,
}

interface IDamageable {
	WeaponType damagedByType { get; set; }

	void TakeDamage(GameObject by, WeaponType type);
}
