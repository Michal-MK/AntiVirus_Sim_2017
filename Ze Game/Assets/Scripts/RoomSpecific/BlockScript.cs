using UnityEngine;

public class BlockScript : MonoBehaviour {
	public RectTransform BG;
	public GameObject player;

	Vector3 currentpos;
	Vector3 startingpos;
	private Quaternion startingrotation = Quaternion.Euler(0, 0, 0);

	float dist;

	public static bool pressurePlateTriggered = false;

	public bool showInfo = true;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		showInfo = data.shownHints.shownBlockInfo;
		transform.position = data.world.blockPos;
		transform.rotation = Quaternion.AngleAxis(data.world.blockZRotation, Vector3.back);
	}

	private void Start() {
		startingpos = gameObject.transform.position;
		startingrotation = gameObject.transform.localRotation;
	}

	private void Update() {
		if (M_Player.currentBG_name == BG.name) {
			currentpos = transform.position;
			dist = Vector3.Distance(player.transform.position, transform.position);

			if (currentpos.x < BG.position.x + -BG.sizeDelta.x / 2) {
				transform.position = startingpos;
				transform.rotation = startingrotation;
			}
			else if (currentpos.x > BG.position.x + BG.sizeDelta.x / 2) {
				transform.position = startingpos;
				transform.rotation = startingrotation;
			}
			else if (currentpos.y < BG.position.y + -BG.sizeDelta.y / 2) {
				transform.position = startingpos;
				transform.rotation = startingrotation;
			}
			else if (currentpos.y > BG.position.y + BG.sizeDelta.y / 2) {
				transform.position = startingpos;
				transform.rotation = startingrotation;
			}

			if (showInfo && dist < 10) {
				Canvas_Renderer.script.InfoRenderer("Find the activator and put the block in front of you on it.", null);
				showInfo = false;
			}
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}
