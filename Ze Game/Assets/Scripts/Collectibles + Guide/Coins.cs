using UnityEngine;

public class Coins : MonoBehaviour {
	public Transform collec;
	public GameObject amount;
	public RectTransform BG;
	public RectTransform coin;
	public Guide guide;
	public EnemySpawner spawner;
	public Spike spike;
	
	Vector3 oldpos;
	float scale;

	public static float coinsCollected = 0;

	void Start() {
		oldpos = coin.transform.position;
		scale = gameObject.GetComponent<RectTransform>().sizeDelta.x / 2;
	}

	private void OnTriggerEnter2D(Collider2D col) {


		if (col.name == "Player" && coinsCollected <=5) {

			SoundFXHandler.script.PlayFX(SoundFXHandler.script.CoinCollected);
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
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.CoinCollected);
			guide.disableGuide();
			coin.gameObject.SetActive(false);
			spike.SetPosition();
		}
	}
}
