using UnityEngine;

public class Spike : MonoBehaviour {

	public Maze maze;
	public bool guideTowardsSpike = true; // NIY

	private static int _spikesCollected;
	private int stage;

	//Loading information
	private bool _shownDirAfterPickup = false;

	public static event Guide.GuideTarget OnNewTarget;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		M_Player.OnSpikePickup += M_Player_OnSpikePickup;
	}

	private void M_Player_OnSpikePickup(M_Player sender, GameObject spikeObj) {

		spikesCollected++;

		gameObject.SetActive(false);

		if (!_shownDirAfterPickup) {
			_shownDirAfterPickup = true;
			Canvas_Renderer.script.InfoRenderer("Follow the blinking arrows.\n They will guide you to your target.", "Be aware of every detail on the screen.");
		}

		if (spikesCollected >= 0 || spikesCollected <= 4) {
			Canvas_Renderer.script.UpdateCounters("Spike");
		}

		if (spikesCollected == 4) {
			maze.MazeEscape();
		}
		MapData.script.Progress(++M_Player.gameProgression);
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		spikesCollected = data.player.spikesCollected;
		gameObject.SetActive(data.world.spikeActive);
		gameObject.transform.position = data.world.spikePos;
		_shownDirAfterPickup = data.shownHints.shownDirectionsAfterSpikePickup;
	}

	public void SetPosition() {
		stage = M_Player.gameProgression;
		float Xscale = gameObject.transform.lossyScale.x / 2;
		float Yscale = gameObject.transform.lossyScale.y / 2;

		if (stage == 0) {
			RectTransform room1BG = MapData.script.GetBackground(1);
			float x = room1BG.position.x;
			float y = room1BG.position.y;
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.SetActive(true);
			if (OnNewTarget != null) {
				OnNewTarget(gameObject,true);
			}
		}
		if (stage == 1) {
			RectTransform roomIciclesBG = MapData.script.GetBackground(2);
			float x = Random.Range(roomIciclesBG.position.x - roomIciclesBG.sizeDelta.x / 2 + Xscale * 4, roomIciclesBG.position.x);
			float y = Random.Range(roomIciclesBG.position.y - roomIciclesBG.sizeDelta.y / 2 + Yscale * 4, roomIciclesBG.position.y);
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.SetActive(true);
			if (OnNewTarget != null) {
				OnNewTarget(gameObject, true);
			}
		}
		if (stage == 2) {
			RectTransform roomAvoidanceBG = MapData.script.GetBackground(3);
			float x = Random.Range(roomAvoidanceBG.position.x + (-roomAvoidanceBG.sizeDelta.x / 2) + Xscale, roomAvoidanceBG.position.x + (roomAvoidanceBG.sizeDelta.x / 2) - Xscale);
			float y = Random.Range(roomAvoidanceBG.position.y + (-roomAvoidanceBG.sizeDelta.y / 2) + Yscale, roomAvoidanceBG.position.y + (roomAvoidanceBG.sizeDelta.y / 2) - Yscale);
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.SetActive(true);
			if (OnNewTarget != null) {
				OnNewTarget(gameObject, true);
			}
		}
		if (stage == 3) {

			GameObject lastPos = maze.grid[maze.rowcollCount / 2, maze.rowcollCount / 2];

			float x = lastPos.transform.position.x;
			float y = lastPos.transform.position.y;
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.transform.localScale = Vector2.one * 3;
			gameObject.SetActive(true);
		}
		if (stage == 4) {
			RectTransform roomPreBossBG = MapData.script.GetBackground(4);
			float x = roomPreBossBG.transform.position.x - roomPreBossBG.sizeDelta.x / 2 + 20;
			float y = roomPreBossBG.transform.position.y + roomPreBossBG.sizeDelta.y / 2 - 20;
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.SetActive(true);
			if (OnNewTarget != null) {
				OnNewTarget(gameObject, true);
			}
		}
		if (stage == 5) {
			gameObject.SetActive(false);
		}
	}

	public void Hide() {
		gameObject.SetActive(false);
		if (OnNewTarget != null) {
			OnNewTarget(null, true);
		}
	}

	public static int spikesCollected {
		get { return _spikesCollected; }
		set {
			_spikesCollected = value;
			Canvas_Renderer.script.UpdateCounters("Spike");
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		M_Player.OnSpikePickup -= M_Player_OnSpikePickup;
	}
}
