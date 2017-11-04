using UnityEngine;

public class KillerBlocks : MonoBehaviour {
	public Rigidbody2D self;

	private void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Wall") {

			self.velocity = Vector2.zero;
			gameObject.SetActive(false);
		}
	}
	private void OnCollisionEnter2D(Collision2D col) {
		if(col.transform.tag == "BossWall") {
			self.velocity = Vector2.zero;
			gameObject.SetActive(false);
		}
	}
}
