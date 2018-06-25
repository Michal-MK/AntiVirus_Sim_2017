using UnityEngine;

public class BlockScript : MonoBehaviour {

	public int backgroundIndex = 2;

	private RectTransform player;
	private RectTransform room2BG;
	private Vector3 currentPos;
	private Vector3 startingPos;
	private Quaternion startingrotation = Quaternion.Euler(0, 0, 0);
	private float dist;
	public static bool pressurePlateTriggered = false;
	private bool shownInfo = false;
	private bool preventSoftLock = false;
	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		M_Player.OnRoomEnter += M_Player_OnRoomEnter;
	}

	private void M_Player_OnRoomEnter(RectTransform background, M_Player sender) {
		if (background == MapData.script.GetBackground(backgroundIndex)){
			preventSoftLock = true;
		}
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		shownInfo = data.shownHints.shownBlockInfo;
		transform.localPosition = data.world.blockPos;
		transform.rotation = Quaternion.AngleAxis(data.world.blockZRotation, Vector3.back);
	}

	private void Start() {
		startingPos = gameObject.transform.position;
		startingrotation = gameObject.transform.localRotation;
		room2BG = MapData.script.GetBackground(backgroundIndex);
		player = M_Player.player.GetComponent<RectTransform>();
	}

	private void FixedUpdate() {
		if (preventSoftLock) {
			currentPos = transform.position;
			dist = Vector3.Distance(player.position, transform.position);

			if (currentPos.x < room2BG.position.x + -room2BG.sizeDelta.x / 2) {
				transform.position = startingPos;
				transform.rotation = startingrotation;
			}
			else if (currentPos.y < room2BG.position.y + -room2BG.sizeDelta.y / 2) {
				transform.position = startingPos;
				transform.rotation = startingrotation;
			}
			else if (currentPos.y > room2BG.position.y + room2BG.sizeDelta.y / 2) {
				transform.position = startingPos;
				transform.rotation = startingrotation;
			}

			if (!shownInfo && dist < 10) {
				Canvas_Renderer.script.DisplayInfo("Find the activator and put the block in front of you on it.", null);
				shownInfo = true;
			}
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		M_Player.OnRoomEnter -= M_Player_OnRoomEnter;
	}

	public bool save_shownInfo {
		get { return shownInfo; }
	}
}
