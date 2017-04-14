using UnityEngine;

public class Coins : MonoBehaviour {
	public Transform collec;
	public GameObject amount;
	public RectTransform BG;
	public RectTransform coin;
	public Spike spike;
	
	Vector3 oldpos;
	float scale;

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

			coinsCollected = coinsCollected + 1;
			CoinBehavior();
			SoundFXHandler.sound.PlayFX(SoundFXHandler.sound.CoinCollected);
			Statics.canvasRenderer.Counters("Coin");
		}
	}
	public void CoinBehavior() {
		print(coinsCollected);
		if (coinsCollected <= 4) {
			oldpos = gameObject.transform.position;
			Vector3 newpos = GenerateNewPos(oldpos);
			timer.run = true;

			Statics.enemySpawner.spawnKillerBlock();


			gameObject.transform.position = newpos;
			Statics.guide.Recalculate(gameObject, true);

		}
		if (coinsCollected == 5) {
			print(coinsCollected);
			Statics.guide.disableGuide();
			coin.gameObject.SetActive(false);
			if (Spike.spikesCollected == 0) {
				print("This");
				print(Spike.spikesCollected);
				spike.SetPosition();
			}
		}
	}
	public void ChatchUpToAttempt(int attempt) {
		for(int i = 0; i <= attempt; i++) {
			Statics.enemySpawner.spawnKillerBlock();
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
		//print(newpos);
		return newpos;
	}

	private void OnDestroy() {
		Statics.coins = null;
	}
}
