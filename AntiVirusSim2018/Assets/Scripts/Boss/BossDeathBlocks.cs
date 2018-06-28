using UnityEngine;

public class BossDeathBlocks : MonoBehaviour {
	public Rigidbody2D self;

	private void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Wall") {

			self.velocity = Vector2.zero;
			gameObject.SetActive(false);
		}
	}
	private void OnCollisionEnter2D(Collision2D col) {
		if(col.transform.tag == "Wall") {
			self.velocity = Vector2.zero;
			gameObject.SetActive(false);
		}
	}
}
