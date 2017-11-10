using UnityEngine;

public class Spike : MonoBehaviour /*,ICollectible*/ {
	public CameraMovement cam;
	public RectTransform BGS;
	public RectTransform BG1;
	public RectTransform BG2a;
	public Maze maze;
	public RectTransform BG2b;

	public GameObject player;
	public Guide guide;
	public EnemySpawner spawn;
	public GameObject teleporter;
	public Animator anim;

	private static int _spikesCollected;
	public int stage;

	public bool firstSpike = false;
	public bool secondSpike = false;
	public bool thirdSpkie = false;
	public bool fourthSpike = false;
	public bool fifthSpike = false;

	public bool displayArrowGuideInfo = true;

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
			Canvas_Renderer.script.Counters("Spike");

		}
		if (spikesCollected == 4) {
			maze.MazeEscape();
		}


		int p = M_Player.gameProgression;
		switch (p) {
			case 0: {
				firstSpike = true;
				break;
			}
			case 1: {
				secondSpike = true;
				break;
			}
			case 2: {
				thirdSpkie = true;
				break;
			}
			case 3: {
				fourthSpike = true;
				break;
			}
			case 4: {
				fifthSpike = true;
				break;
			}
		}
		M_Player.gameProgression++;
		GameProgression.script.Progress();

	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		gameObject.SetActive(data.world.spikeActive);
		gameObject.transform.position = data.world.spikePos;
	}

	private void OnEnable() {
		guide.gameObject.SetActive(true);
		guide.Recalculate(gameObject, true);
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
			guide.Recalculate(gameObject, true);

		}
		if (stage == 1) {

			float x = Random.Range(BG1.position.x - BG1.sizeDelta.x / 2 + Xscale * 4, BG1.position.x);
			float y = Random.Range(BG1.position.y - BG1.sizeDelta.y / 2 + Yscale * 4, BG1.position.y);
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.SetActive(true);
			guide.Recalculate(gameObject, true);

		}
		if (stage == 2) {

			float x = Random.Range(BG2a.position.x + (-BG2a.sizeDelta.x / 2) + Xscale, BG2a.position.x + (BG2a.sizeDelta.x / 2) - Xscale);
			float y = Random.Range(BG2a.position.y + (-BG2a.sizeDelta.y / 2) + Yscale, BG2a.position.y + (BG2a.sizeDelta.y / 2) - Yscale);
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.SetActive(true);
			guide.Recalculate(gameObject, true);

		}
		if (stage == 3) {
			print(stage);
			GameObject lastPos = maze.grid[maze.rowcollCount / 2, maze.rowcollCount / 2];

			float x = lastPos.transform.position.x;
			float y = lastPos.transform.position.y;
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.transform.localScale = Vector2.one * 3;
			gameObject.SetActive(true);
			guide.Recalculate(gameObject, true);
		}
		if (stage == 4) {
			print(stage);
			float x = BG2b.transform.position.x - BG2b.sizeDelta.x / 2 + 20;
			float y = BG2b.transform.position.y + BG2b.sizeDelta.y / 2 - 20;
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.SetActive(true);
			guide.Recalculate(gameObject, true);

		}
		if (stage == 5) {
			print(stage);
			gameObject.SetActive(false);
		}
	}
	public void Hide() {
		gameObject.SetActive(false);
		guide.gameObject.SetActive(false);
	}

	public static int spikesCollected {
		get { return _spikesCollected; }
		set {
			_spikesCollected = value;
			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene") {
				Canvas_Renderer.script.Counters("Spike");
			}
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		M_Player.OnSpikePickup -= M_Player_OnSpikePickup;
	}
}
