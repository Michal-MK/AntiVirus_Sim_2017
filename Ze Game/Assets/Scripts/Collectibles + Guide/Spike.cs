using UnityEngine;

public class Spike : MonoBehaviour {
	public CameraMovement cam;
	public RectTransform BGS;
	public RectTransform BG1;
	public RectTransform BG2a;
	public Maze Maze;
	public RectTransform BG2b;

	public GameObject player;
	public Guide guide;
	public EnemySpawner spawn;
	public GameObject teleporter;
	public Animator anim;

	public static int spikesCollected;
	public int stage;

	public bool firstSpike = false;
	public bool secondSpike = false;
	public bool thirdSpkie = false;
	public bool fourthSpike = false;
	public bool fifthSpike = false;

	private void Awake() {
		Statics.spike = this;
	}

	void Start() {
		if (PlayerPrefs.HasKey("difficulty") == false) {
			PlayerPrefs.SetInt("difficluty", 0);
		}
	}

	private void OnEnable() {
		guide.enableGuide();
		guide.Recalculate(gameObject, true);
	}

	public bool displayArrowGuideInfo = true;

	void OnTriggerEnter2D(Collider2D col) {

		if (col.tag == "Player") {

			spikesCollected++;

			gameObject.SetActive(false);
			guide.disableGuide();


			if(stage == 1) {
				Statics.pressurePlate.alreadyTriggered = true;
			}

			if (displayArrowGuideInfo == true) {
				displayArrowGuideInfo = false;
				Statics.canvasRenderer.infoRenderer("Follow the blinking arrows.", "Be aware of every detail on the screen.");
			}

			if (spikesCollected >= 0 || spikesCollected <= 4) {
				Statics.canvasRenderer.Counters("Spike");

			}
			if (spikesCollected == 4) {
				Maze.MazeEscape();
			}
			if(spikesCollected == 5) {
				Statics.canvasRenderer.infoRenderer(null, "You found all the bullets.");
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
			Statics.gameProgression.Progress();
		}
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


			float x = Random.Range(BG1.position.x - BG1.sizeDelta.x / 2 + Xscale*4, BG1.position.x);
			float y = Random.Range(BG1.position.y - BG1.sizeDelta.y / 2 + Yscale*4, BG1.position.y);
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
			GameObject lastPos = Maze.grid[Maze.rowcollCount/2, Maze.rowcollCount/2];


			float x = lastPos.transform.position.x;
			float y = lastPos.transform.position.y;
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
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
		guide.disableGuide();
	}

	private void OnDestroy() {
		Statics.spike = null;
	}
}

