using UnityEngine;

public class Coins : MonoBehaviour/*, ICollectible*/ {
	public Transform collec;
	public GameObject amount;
	public RectTransform BG;
	public RectTransform coin;
	public Spike spike;
	
	private Vector3 oldpos;
	private float scale;

	private static int _coinsCollected = 0;

	public static event Guide.GuideTarget OnNewTarget;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		M_Player.OnCoinPickup += CoinBehavior;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		if (data.player.coinsCollected == 5) {
			ChatchUpToAttempt(data.player.coinsCollected - 2);
			GameObject.Find("Coin").SetActive(false);
			CoinBehavior(null,null);
		}
		else if (data.player.coinsCollected <= 4) {
			ChatchUpToAttempt(data.player.coinsCollected - 2);
			CoinBehavior(null, null);
			if(OnNewTarget != null) {
				OnNewTarget(gameObject);
			}
		}
	}

	void Start() {
		oldpos = coin.transform.position;
		scale = gameObject.GetComponent<RectTransform>().sizeDelta.x / 2;
	}

	public void CoinBehavior(M_Player sender, GameObject coinObj) {
		coinsCollected++;
		if (coinsCollected <= 4) {
			oldpos = gameObject.transform.position;
			Vector3 newpos = GenerateNewPos(oldpos);
			if (M_Player.playerState == M_Player.PlayerState.NORMAL) {
				Timer.StartTimer(1f);
			}
			else {
				Timer.StartTimer(2f);
			}
			gameObject.transform.position = newpos;
		}
		if (coinsCollected == 5) {
			coin.gameObject.SetActive(false);
			if (Spike.spikesCollected == 0) {
				spike.SetPosition();
			}
		}
	}

	public void ChatchUpToAttempt(int attempt) {
		for (int i = 0; i <= attempt; i++) {
			print("Borked");
			//Statics.enemySpawner.SpawnKillerBlock();
		}
	}

	private Vector3 GenerateNewPos(Vector3 oldpos) {
		Vector3 newpos = oldpos;
		while (Mathf.Abs(Vector3.Distance(newpos, oldpos)) < 40) {

			float x = Random.Range(-BG.sizeDelta.x / 2 + scale, BG.sizeDelta.x / 2 - scale);
			float y = Random.Range(-BG.sizeDelta.y / 2 + scale, BG.sizeDelta.y / 2 - scale);
			float z = 0f;

			newpos = new Vector3(x, y, z);
		}
		return newpos;
	}

	public static int coinsCollected {
		get { return _coinsCollected; }
		set {
			_coinsCollected = value;
			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene") {
				Canvas_Renderer.script.Counters("Coins");
			}
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		M_Player.OnCoinPickup -= CoinBehavior;

	}
}
