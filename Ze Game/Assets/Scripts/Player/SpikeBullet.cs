using System.Collections;
using UnityEngine;

public class SpikeBullet : MonoBehaviour {
	public GameObject spikeLike;
	public Rigidbody2D rg;

	private void OnEnable() {
		rg.velocity = transform.rotation * Vector3.up * 30f;
		StartCoroutine(StopBullet());
	}

	private void OnCollisionEnter2D(Collision2D col) {
		print(col.transform.name);
		if (col.transform.tag == "Wall") {
			rg.velocity = Vector3.zero;
			GameObject newspikeBullet = Instantiate(spikeLike, col.contacts[0].point, transform.rotation);
			newspikeBullet.transform.parent = GameObject.Find("Collectibles").transform;
			newspikeBullet.transform.localScale = new Vector3(0.5f, 0.5f, 1);
			newspikeBullet.name = "Spike1";
			gameObject.SetActive(false);
		}
		if(col.transform.tag == "SpikeDetectBoss") {
			rg.velocity = Vector3.zero;
			gameObject.SetActive(false);
		}
	}

	public IEnumerator StopBullet() {
		yield return new WaitForSeconds(1.5f);
		rg.velocity = Vector3.zero;
		GameObject newspikeBullet = Instantiate(spikeLike, transform.position, transform.rotation);
		newspikeBullet.transform.parent = GameObject.Find("Collectibles").transform;
		newspikeBullet.transform.localScale = new Vector3(0.5f, 0.5f, 1);
		newspikeBullet.name = "Spike1";
		gameObject.SetActive(false);
		StopAllCoroutines();
	}
}
