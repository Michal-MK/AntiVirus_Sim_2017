using System.Collections;
using UnityEngine;

public class MoveScript : MonoBehaviour {

	public Rigidbody2D rg;
	public Vector2 movVelocity;

	public bool destroy;
	public bool deactivate;
	public bool timedRemoval;
	public float timeTillSelfDesctuct;

	public void Move() {
		if (timedRemoval) {
			StartCoroutine(SelfDestruct(timeTillSelfDesctuct));
		}
		rg.velocity = movVelocity;
	}

	private IEnumerator SelfDestruct(float timeRemaining) {
		yield return new WaitForSeconds(timeRemaining);
		if (destroy && !deactivate) {
			Destroy(gameObject);
		}
		else if (deactivate) {
			gameObject.SetActive(false);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.tag == "BG") {
			if (destroy && !deactivate) {
				Destroy(gameObject);
			}
			else if (deactivate) {
				gameObject.SetActive(false);
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Wall") {
			if (destroy && !deactivate) {
				Destroy(gameObject);
			}
			else if (deactivate) {
				gameObject.SetActive(false);
			}
		}
	}
}