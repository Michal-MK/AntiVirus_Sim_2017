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
	
	Vector3 oldpos;

	public static float coinsCollected = 0;

	void Start() {
		oldpos = coin.transform.position;
	}

	private void OnTriggerEnter2D(Collider2D col) {
		float scale = coin.GetComponent<CircleCollider2D>().radius;


		if (col.name == "Player" && coinsCollected <=5) {

			Vector3 newpos = gameObject.transform.position;
			oldpos = newpos;
			timer.run = true;
			spawner.spawnKillerBlock();
			Canvas_Renderer.script.Counters("Coin");

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
			spike.SetPosition();
		}
	}
}
