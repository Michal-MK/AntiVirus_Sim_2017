using Igor.Constants.Strings;
using UnityEngine;

public class BossDeathBlocks : MonoBehaviour {
	public Rigidbody2D self;

	private void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == Tags.WALL) {

			self.velocity = Vector2.zero;
			gameObject.SetActive(false);
		}
	}
	private void OnCollisionEnter2D(Collision2D col) {
		if(col.transform.tag == Tags.WALL) {
			self.velocity = Vector2.zero;
			gameObject.SetActive(false);
		}
	}
}
