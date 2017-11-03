using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IKillable {
	public enum EnemyType {
		TOUCH,
		CLOSE_RANGE,
		LONG_RANGE,
	}

	protected bool _instantDeath = true;
	protected bool _is_Destroyable = true;
	protected float _damage = 1;
	protected EnemyType _type;

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

	public EnemyType enemyType {
		get { return _type; }
		set { _type = value; }
	}
}
