using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollisionDetection : MonoBehaviour {
	public BombScript bomb;

	private void OnTriggerEnter2D(Collider2D collision) {
		bomb.Collided(collision);
	}
	private void Update() {
		if(bomb.primed == true) {
			bomb.col.enabled = false;
		}
	}
}
