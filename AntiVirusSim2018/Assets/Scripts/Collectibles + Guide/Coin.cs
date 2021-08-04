using UnityEngine;

public class Coin : MonoBehaviour {

	[SerializeField]
	private Spike spike = null;

	private Vector3 oldpos;
	private float scale;

	public static event GuideTargetStaticEventHandler OnNewTarget; // 1 Usage by Guide

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		Player.OnCoinPickup += OnCoinPickup;
	}

	void Start() {
		oldpos = transform.position;
		scale = GetComponent<RectTransform>().sizeDelta.x / 2;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		CoinsCollected = data.player.coinsCollected;

		if (data.player.coinsCollected == 5) {
			gameObject.SetActive(false);
			if (spike.SpikesCollected == 0) {
				gameObject.SetActive(false);
				spike.SetPosition();
			}
		}
		else if (data.player.coinsCollected <= 4) {
			OnCoinPickup(null, null);
			OnNewTarget?.Invoke(gameObject.transform.position);
		}
	}

	public void OnCoinPickup(Player sender, Coin coin) {
		if (sender != null) {
			CoinsCollected++;
		}

		Timer.StartTimer(1);

		if (CoinsCollected <= 4) {
			oldpos = gameObject.transform.position;
			Vector3 newpos = GenerateNewPos(oldpos);
			transform.position = newpos;
			OnNewTarget?.Invoke(transform.position);
		}
		if (CoinsCollected == 5) {
			if (spike.SpikesCollected == 0) {
				gameObject.SetActive(false);
				spike.SetPosition();
			}
		}
	}

	private Vector3 GenerateNewPos(Vector3 oldpos) {
		Vector3 newpos = oldpos;
		while (Mathf.Abs(Vector3.Distance(newpos, oldpos)) < 40) {
			RectTransform room1BG = MapData.Instance.GetRoom(1).Background;
			float x = Random.Range(-room1BG.sizeDelta.x / 2 + scale, room1BG.sizeDelta.x / 2 - scale);
			float y = Random.Range(-room1BG.sizeDelta.y / 2 + scale, room1BG.sizeDelta.y / 2 - scale);
			float z = 0f;

			newpos = new Vector3(x, y, z);
		}
		return newpos;
	}

	[SerializeField]
	private int coinsCollected = 0;
	public int CoinsCollected {
		get => coinsCollected;
		set {
			coinsCollected = value;
			HUDisplay.Instance.UpdateCoinCounter(value);
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.CoinCollected);
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		Player.OnCoinPickup -= OnCoinPickup;
	}
}
