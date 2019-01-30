using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IPoolable {

	public enum EnemyClass {
		TOUCH,
		CLOSE_RANGE,
		LONG_RANGE,
	}

	public enum EnemyType {
		ELECTRIC_BLOCK,
		KILLER_BLOCK,
		PROJECTILE_SIMPLE,
		PROJECTILE_ACCURATE,
		PROJECTILE_ICICLE,
	}

	public Material lightMat;
	public Material darkMat;

	protected bool instantKiller = true;
	public bool isDestroyable { get; set; } = false;
	public bool isPooled { get; set; } = false;
	protected float damage { get; set; } = 1;
	protected EnemyClass enemyClass { get; set; }
	protected EnemyType enemyType { get; set; }


	public virtual void Kill() {

		if (isPooled) {
			gameObject.SetActive(false);

			return;
		}

		if (!isDestroyable) {
			print("This enemy is not something you can kill");
			return;
		}
		else {
			Destroy(gameObject);
		}
	}

	public virtual IEnumerator Kill(float seconds) {
		yield return new WaitForSeconds(seconds);
		Kill();
	}

	public virtual void MapModeSwitch(MapData.MapMode mode) {
		if (mode == MapData.MapMode.DARK) {
			gameObject.layer = (int)Igor.Constants.Integers.Layers.Layer.DARK_ENEMIES;
			gameObject.GetComponent<SpriteRenderer>().material = darkMat;
		}
		else {
			gameObject.layer = (int)Igor.Constants.Integers.Layers.Layer.LIGHT_ENEMIES;
			gameObject.GetComponent<SpriteRenderer>().material = lightMat;
		}
	}

	public virtual void Damage(float amount) {
		print("Damaged for " + amount + " points of damage.");
	}
}
