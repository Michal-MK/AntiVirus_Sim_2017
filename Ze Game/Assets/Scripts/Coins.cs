using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour {
	public Transform collec;
	public GameObject amount;
	public RectTransform BG;
	public Transform coin;
	public Guide guide;
	public EnemySpawner spawner;
	public Spike spike;
	Text count;
	Vector3 oldpos;
	Animator anim;

	float coinsCollected = 0;

	// Use this for initialization
	void Start() {
		oldpos = coin.transform.position;
		anim = amount.GetComponent<Animator>();
		count = amount.GetComponent<Text>();
	}

	private void OnTriggerEnter2D(Collider2D col) {
		float scale = coin.GetComponent<CircleCollider2D>().radius;


		if (col.name == "Player" && coinsCollected <=5) {

			Vector3 newpos = gameObject.transform.position;
			oldpos = newpos;
			timer.run = true;
			spawner.spawnKillerBlock();
			count.text = "x " + (coinsCollected + 1);
			anim.Play("MoreHighlights");

			while (Mathf.Abs(Vector3.Distance(newpos, oldpos)) < 40) {

				float x = Random.Range(-BG.sizeDelta.x / 2 + scale, BG.sizeDelta.x / 2 - scale);
				float y = Random.Range(-BG.sizeDelta.y / 2 + scale, BG.sizeDelta.y / 2 - scale);
				float z = 0f;

				newpos = new Vector3(x, y, z);
			}

			gameObject.transform.position = newpos;

			coinsCollected = coinsCollected + 1;

			guide.Recalculate(coin.gameObject,true);

		}
		if (coinsCollected == 5) {
			guide.disableGuide();
			coin.gameObject.SetActive(false);
			count.transform.localPosition = count.transform.localPosition + new Vector3(50, 0, 0);
			count.text = "Completed!";

			spike.SetPosition();

		}
	}
}
