using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IKillable {
	public enum EnemyType {
		TOUCH,
		CLOSE_RANGE,
		LONG_RANGE,
	}

	private bool _instantDeath = true;
	private bool _is_Destroyable;
	private float _damage;
	private EnemyType _type;

	public virtual void DealDamage(float damage) {
		if (_instantDeath) { }
	}

	public void Kill() {
		print("Destroying " + gameObject.name);
		Destroy(gameObject);
	}
}
