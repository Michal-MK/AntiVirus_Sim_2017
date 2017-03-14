using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollisionDetection : MonoBehaviour {
	public BombScript bomb;

	private void OnTriggerEnter2D(Collider2D col) {
		if(col.name == "Top" || col.name == "Right" || col.name == "Bottom" || col.name == "Left")
		bomb.Collided(col);
	}
	private void Update() {
		if(bomb.primed == true) {
			bomb.col.enabled = false;
		}
	}
}
