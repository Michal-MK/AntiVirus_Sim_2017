using System;
using UnityEngine;

[Flags]
public enum WeaponType {
	BULLET,
	BOMB,
}

interface IDamageable {
	WeaponType damageType { get; set; }

	void Damaged(Collision2D by, WeaponType type);
}
