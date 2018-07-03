using System;
using UnityEngine;

[Flags]
public enum WeaponType {
	BULLET,
	BOMB,
}

interface IDamageable {
	WeaponType damageType { get; set; }

	void Damaged(GameObject by, WeaponType type);
}
