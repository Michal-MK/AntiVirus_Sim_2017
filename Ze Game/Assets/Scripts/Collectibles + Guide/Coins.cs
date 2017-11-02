using UnityEngine;

public class Coins : MonoBehaviour {
	public Transform collec;
	public GameObject amount;
	public RectTransform BG;
	public RectTransform coin;
	public Spike spike;
	
	private Vector3 oldpos;
	private float scale;

	public static int coinsCollected = 0;

	private void Awake() {
		Statics.coins = this;
	}

	void Start() {
		oldpos = coin.transform.position;
		scale = gameObject.GetComponent<RectTransform>().sizeDelta.x / 2;
	}

	private void OnTriggerEnter2D(Collider2D col) {
		if (col.name == "Player") {
			Statics.mPlayer.face.GetComponent<SpriteRenderer>().sprite = Statics.mPlayer.happy;

			coinsCollected += 1;
			CoinBehavior();
			Statics.sound.PlayFX(Statics.sound.CoinCollected);
			Statics.canvasRenderer.Counters("Coin");
		}
	}
	public void CoinBehavior() {
		//print(coinsCollected);
		if (coinsCollected <= 4) {
			oldpos = gameObject.transform.position;
			Vector3 newpos = GenerateNewPos(oldpos);
			Timer.run = true;

			Statics.enemySpawner.SpawnKillerBlock();

			gameObject.transform.position = newpos;
			Statics.guide.Recalculate(gameObject, true);

		}
		if (coinsCollected == 5) {
			Statics.guide.gameObject.SetActive(false);
			coin.gameObject.SetActive(false);
			if (Spike.spikesCollected == 0) {
				spike.SetPosition();
			}
		}
	}
	public void ChatchUpToAttempt(int attempt) {
		for(int i = 0; i <= attempt; i++) {
			Statics.enemySpawner.SpawnKillerBlock();
		}
	}

	private Vector3 GenerateNewPos(Vector3 oldpos) {
		//print(oldpos);
		Vector3 newpos = oldpos;
		while (Mathf.Abs(Vector3.Distance(newpos, oldpos)) < 40) {

			float x = Random.Range(-BG.sizeDelta.x / 2 + scale, BG.sizeDelta.x / 2 - scale);
			float y = Random.Range(-BG.sizeDelta.y / 2 + scale, BG.sizeDelta.y / 2 - scale);
			float z = 0f;

			newpos = new Vector3(x, y, z);
		}
		return newpos;
	}

	private void OnDestroy() {
		Statics.coins = null;
	}
}
