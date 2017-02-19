using System.Collections;
using UnityEngine;

public class SpikeBullet : MonoBehaviour {
	public GameObject spikeBullet;
	public Rigidbody2D rg;

	private void OnEnable() {
		rg.velocity = transform.rotation * Vector3.up * 30f;
		StartCoroutine(StopBullet());
	}

	private void OnCollisionEnter2D(Collision2D col) {

		if (col.transform.tag == "BossHitbox") {







			gameObject.SetActive(false);
		}
		if (col.transform.tag == "Wall") {
			rg.velocity = Vector3.zero;
			GameObject newspikeBullet = Instantiate(spikeBullet, col.contacts[0].point, transform.rotation);
			newspikeBullet.transform.parent = GameObject.Find("Collectibles").transform;
			newspikeBullet.transform.localScale = new Vector3(0.5f, 0.5f, 1);
			gameObject.SetActive(false);
		}
	}

	public IEnumerator StopBullet() {
		yield return new WaitForSeconds(1.5f);
		rg.velocity = Vector3.zero;
		GameObject newspikeBullet = Instantiate(spikeBullet, transform.position, transform.rotation);
		newspikeBullet.transform.parent = GameObject.Find("Collectibles").transform;
		newspikeBullet.transform.localScale = new Vector3(0.5f, 0.5f, 1);
		gameObject.SetActive(false);
		StopAllCoroutines();
	}
	// Update is called once per frame
	void Update () {
		
	}
}
