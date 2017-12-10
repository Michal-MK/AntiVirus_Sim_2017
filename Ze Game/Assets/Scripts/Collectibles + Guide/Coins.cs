using UnityEngine;
using Igor.Constants.Strings;

public class Coins : MonoBehaviour {

	public GameObject amount;
	public RectTransform BG;
	public Spike spike;

	private Vector3 oldpos;
	private float scale;

	private static int _coinsCollected = 0;

	public static event Guide.GuideTarget OnNewTarget;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		M_Player.OnCoinPickup += CoinBehavior;
	}

	void Start() {
		oldpos = transform.position;
		scale = GetComponent<RectTransform>().sizeDelta.x / 2;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		coinsCollected = data.player.coinsCollected;

		if (data.player.coinsCollected == 5) {
			gameObject.SetActive(false);
			if (Spike.spikesCollected == 0) {
				gameObject.SetActive(false);
				spike.SetPosition();
			}
		}
		else if (data.player.coinsCollected <= 4) {
			CoinBehavior(null, null);
			if (OnNewTarget != null) {
				OnNewTarget(gameObject, true);
			}
		}
	}

	public void CoinBehavior(M_Player sender, GameObject coinObj) {
		if (sender != null) {
			coinsCollected++;
		}

		if (coinsCollected <= 4) {
			oldpos = gameObject.transform.position;
			Vector3 newpos = GenerateNewPos(oldpos);
			if (M_Player.playerState == M_Player.PlayerState.NORMAL) {
				Timer.StartTimer(1f);
			}
			else {
				Timer.StartTimer(2f);
			}
			transform.position = newpos;
		}
		if (coinsCollected == 5) {
			if (Spike.spikesCollected == 0) {
				gameObject.SetActive(false);
				spike.SetPosition();
			}
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
			Canvas_Renderer.script.UpdateCounters(ObjNames.COIN);
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		M_Player.OnCoinPickup -= CoinBehavior;

	}
}
