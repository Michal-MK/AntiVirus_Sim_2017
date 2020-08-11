using UnityEngine;

public class BlockScript : MonoBehaviour {

	private RectTransform player;
	private RectTransform room2BG;
	private Vector3 currentPos;
	private Vector3 startingPos;
	private Quaternion defaultRotation = Quaternion.Euler(0, 0, 0);

	public static bool pressurePlateTriggered = false;
	private bool preventSoftLock = false;

	public bool FirstApproachHint { get; private set; }

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		Player.OnRoomEnter += M_Player_OnRoomEnter;
	}

	private void M_Player_OnRoomEnter(Player sender, RectTransform background, RectTransform previous) {
		if (background == MapData.Instance.GetRoom(2).Background){
			preventSoftLock = true;
		}
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		FirstApproachHint = data.shownHints.shownBlockInfo;
		transform.localPosition = data.world.blockPos;
		transform.rotation = Quaternion.AngleAxis(data.world.blockZRotation, Vector3.back);
	}

	private void Start() {
		startingPos = gameObject.transform.position;
		defaultRotation = gameObject.transform.localRotation;
		room2BG = MapData.Instance.GetRoom(2).Background;
		player = Player.Instance.GetComponent<RectTransform>();
	}

	private void FixedUpdate() {
		if (preventSoftLock) {
			currentPos = transform.position;
			float dist = Vector3.Distance(player.position, transform.position);

			if (currentPos.x < room2BG.position.x + -room2BG.sizeDelta.x / 2) {
				transform.position = startingPos;
				transform.rotation = defaultRotation;
			}
			else if (currentPos.y < room2BG.position.y + -room2BG.sizeDelta.y / 2) {
				transform.position = startingPos;
				transform.rotation = defaultRotation;
			}
			else if (currentPos.y > room2BG.position.y + room2BG.sizeDelta.y / 2) {
				transform.position = startingPos;
				transform.rotation = defaultRotation;
			}

			if (!FirstApproachHint && dist < 10) {
				HUDisplay.script.DisplayInfo("Find the activator and put the block in front of you on it.", null);
				FirstApproachHint = true;
			}
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		Player.OnRoomEnter -= M_Player_OnRoomEnter;
	}
}
