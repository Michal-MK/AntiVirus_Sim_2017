using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public enum EnemyClass {
		TOUCH,
		CLOSE_RANGE,
		LONG_RANGE,
	}

	public enum EnemyType {
		KILLERBLOCK,
		PROJECTILE_SIMPLE,
		PROJECTILE_ACCURATE,
		PROJECTILE_ICICLE,
	}

	protected bool _instantDeath = true;
	protected bool _is_Destroyable = true;
	protected float _damage = 1;
	protected EnemyClass _class;

	public virtual void Kill() {
		if (_is_Destroyable) {
			print("Destroying " + gameObject.name);
			Destroy(gameObject);
		}
	}

	public virtual void Damage(float amount) {
		print("Damaged for " + amount + " points of damage.");
	}

	public virtual void DealDamage(float damage) {

	}

	public bool isDestroyable {
		get { return _is_Destroyable; }
		set { _is_Destroyable = value; }
	}

	public float damage {
		get { return _damage; }
		set { _damage = value; }
	}

	public EnemyClass enemyClass {
		get { return _class; }
		set { _class = value; }
	}
}
