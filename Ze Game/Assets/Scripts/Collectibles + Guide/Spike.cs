using UnityEngine;

public class Spike : MonoBehaviour {

	public Maze maze;
	public bool guideTowardsSpike = true; // NIY

	private static int _spikesCollected;
	private int stage;

	//Loading information
	private bool _shownDirAfterPickup = false;

	public static event Guide.GuideTargetStatic OnNewTarget;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		M_Player.OnSpikePickup += M_Player_OnSpikePickup;
	}

	private void M_Player_OnSpikePickup(M_Player sender, GameObject spikeObj) {

		spikesCollected++;

		gameObject.SetActive(false);

		if (!_shownDirAfterPickup) {
			_shownDirAfterPickup = true;
			Canvas_Renderer.script.DisplayInfo("Follow the blinking arrows.\n They will guide you to your target.", "Be aware of every detail on the screen.");
		}
		//Finished avoidance
		if (_spikesCollected == 3) {
			MapData.script.OpenDoor(new RoomLink(2, 3));
			CameraMovement.script.RaycastForRooms();
		}
		//Went through maze
		if (_spikesCollected == 4) {
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

	public void SetPosition(bool guideTowards = true) {
		stage = M_Player.gameProgression;
		float Xscale = gameObject.transform.lossyScale.x / 2;
		float Yscale = gameObject.transform.lossyScale.y / 2;
		float newX;
		float newY;
		switch (stage) {
			case 0: {
				RectTransform room1BG = MapData.script.GetBackground(1);
				newX = room1BG.position.x;
				newY = room1BG.position.y - 1;
				break;
			}
			case 1: {
				RectTransform roomIciclesBG = MapData.script.GetBackground(2);
				newX = Random.Range(roomIciclesBG.position.x - roomIciclesBG.sizeDelta.x / 2 + Xscale * 4, roomIciclesBG.position.x);
				newY = Random.Range(roomIciclesBG.position.y - roomIciclesBG.sizeDelta.y / 2 + Yscale * 4, roomIciclesBG.position.y);
				break;
			}
			case 2: {
				RectTransform roomAvoidanceBG = MapData.script.GetBackground(3);
				newX = Random.Range(roomAvoidanceBG.position.x + (-roomAvoidanceBG.sizeDelta.x / 2) + Xscale, roomAvoidanceBG.position.x + (roomAvoidanceBG.sizeDelta.x / 2) - Xscale);
				newY = Random.Range(roomAvoidanceBG.position.y, roomAvoidanceBG.position.y + (roomAvoidanceBG.sizeDelta.y / 2) - Yscale);
				break;
			}
			case 3: {
				GameObject lastPos = maze.grid[maze.rowcollCount / 2, maze.rowcollCount / 2];

				newX = lastPos.transform.position.x;
				newY = lastPos.transform.position.y;
				transform.localScale = Vector2.one * 3;
				break;
			}
			case 4: {
				RectTransform roomPreBossBG = MapData.script.GetBackground(4);
				newX = roomPreBossBG.transform.position.x - roomPreBossBG.sizeDelta.x / 2 + 20;
				newY = roomPreBossBG.transform.position.y + roomPreBossBG.sizeDelta.y / 2 - 20;
				transform.localScale = new Vector2(0.4f, 0.5f);
				break;
			}
			case 5: {
				gameObject.SetActive(false);
				return;
			}
			default: {
				throw new System.Exception("NIY");
			}
		}

		gameObject.transform.position = new Vector3(newX, newY);
		gameObject.SetActive(true);
		if (OnNewTarget != null && guideTowards) {
			OnNewTarget(transform.position);

		}
	}

	public void Hide() {
		gameObject.SetActive(false);
		if (OnNewTarget != null) {
			OnNewTarget(default(Vector3));
		}
	}

	public static int spikesCollected {
		get { return _spikesCollected; }
		set {
			_spikesCollected = value;
			Canvas_Renderer.script.UpdateCounters();
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		M_Player.OnSpikePickup -= M_Player_OnSpikePickup;
	}
}
