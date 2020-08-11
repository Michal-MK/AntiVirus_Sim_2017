using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IPoolable {
	[SerializeField]
	private Material lightMat = null;
	[SerializeField]
	private Material darkMat = null;

	public bool IsKillable { get; set; } = false;
	public bool IsPooled { get; set; } = false;
	protected EnemyClass Class { get; set; }
	protected EnemyType Type { get; set; }

	/// <summary>
	/// Override to modify killing behaviour. <para/>
	/// <see langword="Always"/> call <see langword="base"/>!
	/// </summary>
	public virtual void Kill() {
		if (IsPooled) {
			gameObject.SetActive(false);
			return;
		}

		if (!IsKillable) {
			print("This enemy is not something you can kill");
			return;
		}
		else {
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Override to modify delayed killing behaviour. <para/>
	/// <see langword="Never"/> call <see langword="base"/>!
	/// </summary>
	public virtual IEnumerator Kill(float seconds) {
		yield return new WaitForSeconds(seconds);
		Kill();
	}

	/// <summary>
	/// Override to change behaviour for <see cref="MapMode"/> change.<para/>
	/// Calling <see langword="base"/> is not required!
	/// </summary>
	public virtual void MapModeSwitch(MapMode mode) {
		if (mode == MapMode.DARK) {
			gameObject.layer = (int)Igor.Constants.Integers.Layers.Layer.DARK_ENEMIES;
			gameObject.GetComponent<SpriteRenderer>().material = darkMat;
		}
		else {
			gameObject.layer = (int)Igor.Constants.Integers.Layers.Layer.LIGHT_ENEMIES;
			gameObject.GetComponent<SpriteRenderer>().material = lightMat;
		}
	}
}
