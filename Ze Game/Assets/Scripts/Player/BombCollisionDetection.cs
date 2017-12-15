using UnityEngine;

public class BombCollisionDetection : MonoBehaviour {
	public BombScript bomb;

	private void OnTriggerEnter2D(Collider2D col) {
		bomb.Collided(col);
	}

	private void Update() {
		if (bomb.primed) {
			bomb.col.enabled = false;
		}
	}
}
