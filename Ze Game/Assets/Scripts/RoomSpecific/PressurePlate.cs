using UnityEngine;

public class PressurePlate : MonoBehaviour {

	public Sprite Active;
	public Sprite Inactive;
	public AudioClip On;
	public AudioClip Off;

	public RectTransform BG;

	public Spike spike;

	//Prefabs
	public GameObject wall;

	//Save altered data
	public int attempts = 0;
	public bool alreadyTriggered = false;

	public static event Guide.GuideTarget OnNewTarget;

	private AudioSource sound;
	private SpriteRenderer selfSprite;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	void Start() {
		sound = GetComponent<AudioSource>();
		selfSprite = GetComponent<SpriteRenderer>();
		transform.position = GeneratePos();
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		if (data.world.blockPushAttempt == 3) {
			CreateBarrier();
		}
		else {
			attempts = data.world.blockPushAttempt;
		}
		alreadyTriggered = data.world.pressurePlateTriggered;
	}

	public Vector3 GeneratePos() {

		Vector3 pos = transform.position;
		Vector3 newPos = pos;

		float f = Random.value;

		if (f < 0.5) {
			while (Vector3.Distance(pos, newPos) < 10) {
				newPos = new Vector3(pos.x, Random.Range(BG.sizeDelta.y / 2 - 10, pos.y + 10), 0);
			}
			return newPos;
		}
		else {
			while (Vector3.Distance(pos, newPos) < 10) {
				newPos = new Vector3(pos.x, Random.Range(-BG.sizeDelta.y / 2 + 10, pos.y - 10), 0);
			}
			return newPos;
		}

	}

	void OnTriggerEnter2D(Collider2D col) {
		if (!alreadyTriggered) {
			if (col.name == "Block") {
				selfSprite.sprite = Active;
				sound.clip = On;
				sound.Play();
				spike.SetPosition();
				if (OnNewTarget != null) {
					OnNewTarget(spike.gameObject);
				}
				BlockScript.pressurePlateTriggered = true;
			}
		}
	}
	void OnTriggerExit2D(Collider2D col) {
		if (col.name == "Block") {
			attempts++;
			if (attempts == 1) {
				Canvas_Renderer.script.InfoRenderer("Something pushed the block off of the activator...", "These projectiles sure are a nuisance.");
			}
			if (attempts == 2) {
				Canvas_Renderer.script.InfoRenderer(null, "Aaand again... darn.");
			}
			if (attempts == 3) {
				CreateBarrier();
			}
			sound.clip = Off;
			sound.Play();
			selfSprite.sprite = Inactive;
			spike.Hide();
			BlockScript.pressurePlateTriggered = false;
		}
	}
	public void CreateBarrier() {
		Canvas_Renderer.script.InfoRenderer(null, "Ok, let me help you a little.");
		GameObject protection = Instantiate(wall, transform.position + new Vector3(10, 0, 0), Quaternion.identity, transform.parent);
		protection.name = "Blocker";
		protection.GetComponent<BoxCollider2D>().isTrigger = true;
		protection.transform.localScale = new Vector3(0.2f, 0.1f, 1);
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}
