using Igor.Constants.Strings;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

	public Sprite sprtActive;
	public Sprite sprtInactive;
	public AudioClip switchOn;
	public AudioClip switchOff;

	public RectTransform background;

	public Spike spike;

	//Prefabs
	public GameObject wall;

	//Save altered data
	public int attempts = 0;
	public bool alreadyTriggered = false;

	public static event GuideTargetStaticEventHandler OnNewTarget;

	private SpriteRenderer selfSprite;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	void Start() {
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
				newPos = new Vector3(pos.x, Random.Range(background.sizeDelta.y / 2 - 10, pos.y + 10), 0);
			}
			return newPos;
		}
		else {
			while (Vector3.Distance(pos, newPos) < 10) {
				newPos = new Vector3(pos.x, Random.Range(-background.sizeDelta.y / 2 + 10, pos.y - 10), 0);
			}
			return newPos;
		}

	}

	void OnTriggerEnter2D(Collider2D col) {
		if (!alreadyTriggered) {
			if (col.name == ObjNames.BLOCK) {
				selfSprite.sprite = sprtActive;
				SoundFXHandler.script.PlayFX(switchOn);
				spike.SetPosition();
				OnNewTarget?.Invoke(spike.transform.position);
				BlockScript.pressurePlateTriggered = true;
			}
		}
	}
	void OnTriggerExit2D(Collider2D col) {
		if (col.name == ObjNames.BLOCK) {
			attempts++;
			if (attempts == 1) {
				HUDisplay.script.DisplayInfo("Something pushed the block off of the activator...", "These projectiles sure are a nuisance.");
			}
			if (attempts == 2) {
				HUDisplay.script.DisplayInfo(null, "Aaand again... darn.");
			}
			if (attempts == 3) {
				HUDisplay.script.DisplayInfo(null, "Alright enough.");
				CreateBarrier();
			}
			SoundFXHandler.script.PlayFX(switchOff);
			selfSprite.sprite = sprtInactive;
			spike.Hide();
			BlockScript.pressurePlateTriggered = false;
		}
	}
	public void CreateBarrier() {
		GameObject protection = Instantiate(wall, transform.position + new Vector3(10, 0, 0), Quaternion.identity, transform.parent);
		protection.name = ObjNames.PRESSURE_PLATE_WALL;
		protection.transform.localScale = new Vector3(0.2f, 0.1f, 1);
		Rigidbody2D rb = protection.AddComponent<Rigidbody2D>();
		rb.isKinematic = true;
		rb.useFullKinematicContacts = true;
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}
