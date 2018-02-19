using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour {

	public bool checkTriggers;
	public bool checkColliders;

	public AudioSource source;
	public AudioClip bombExplosion;

	void Start() {
		StartCoroutine(Explode());
	}

	private void OnTriggerEnter2D(Collider2D col) {
		if (checkTriggers) {
			print(col.transform.name);
			col.transform.SendMessage("HitByBombExplosion", this, SendMessageOptions.RequireReceiver);
		}
	}

	private void OnCollisionEnter2D(Collision2D col) {
		if (checkColliders) {
			col.transform.SendMessage("HitByBombExplosion", this, SendMessageOptions.RequireReceiver);
		}
	}

	private IEnumerator Explode() {
		source.Play();
		yield return new WaitForSeconds(1.5f);
		GetComponent<Collider2D>().enabled = true;
		source.clip = bombExplosion;
		source.Play();
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}

}
