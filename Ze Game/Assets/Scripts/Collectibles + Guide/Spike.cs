using UnityEngine;

public class Spike : MonoBehaviour {

	public RectTransform BGS;
	public RectTransform BG1;
	public RectTransform BG2a;
	public Maze maze;
	public RectTransform BG2b;

	public Guide guide;

	private static int _spikesCollected;
	private int stage;

	//Loading information
	public bool displayArrowGuideInfo = true;

	public static event Guide.GuideTarget OnNewTarget;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		M_Player.OnSpikePickup += M_Player_OnSpikePickup;
	}

	private void M_Player_OnSpikePickup(M_Player sender, GameObject spikeObj) {

		spikesCollected++;

		gameObject.SetActive(false);

		if (displayArrowGuideInfo == true) {
			displayArrowGuideInfo = false;
			Canvas_Renderer.script.InfoRenderer("Follow the blinking arrows.\n They will guide you to your target.", "Be aware of every detail on the screen.");
		}

		if (spikesCollected >= 0 || spikesCollected <= 4) {
			Canvas_Renderer.script.UpdateCounters("Spike");
		}

		if (spikesCollected == 4) {
			maze.MazeEscape();
		}

		M_Player.gameProgression++;
		GameProgression.script.Progress();
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		spikesCollected = data.player.spikesCollected;
		gameObject.SetActive(data.world.spikeActive);
		gameObject.transform.position = data.world.spikePos;
	}

	public void SetPosition() {
		stage = M_Player.gameProgression;

		float Xscale = gameObject.transform.lossyScale.x / 2;
		float Yscale = gameObject.transform.lossyScale.y / 2;

		if (stage == 0) {

			float x = BGS.position.x;
			float y = BGS.position.y;
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.SetActive(true);
			if (OnNewTarget != null) {
				OnNewTarget(gameObject,true);
			}
		}
		if (stage == 1) {

			float x = Random.Range(BG1.position.x - BG1.sizeDelta.x / 2 + Xscale * 4, BG1.position.x);
			float y = Random.Range(BG1.position.y - BG1.sizeDelta.y / 2 + Yscale * 4, BG1.position.y);
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.SetActive(true);
			if (OnNewTarget != null) {
				OnNewTarget(gameObject, true);
			}
		}
		if (stage == 2) {

			float x = Random.Range(BG2a.position.x + (-BG2a.sizeDelta.x / 2) + Xscale, BG2a.position.x + (BG2a.sizeDelta.x / 2) - Xscale);
			float y = Random.Range(BG2a.position.y + (-BG2a.sizeDelta.y / 2) + Yscale, BG2a.position.y + (BG2a.sizeDelta.y / 2) - Yscale);
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

			float x = BG2b.transform.position.x - BG2b.sizeDelta.x / 2 + 20;
			float y = BG2b.transform.position.y + BG2b.sizeDelta.y / 2 - 20;
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
