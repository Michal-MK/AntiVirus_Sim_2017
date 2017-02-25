using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitDetection : MonoBehaviour {
	public BossHealth hp;

	private void OnCollisionEnter2D(Collision2D collision) {
		hp.Collided(collision,gameObject);
	}

}
