using UnityEngine;

public class Spike : MonoBehaviour {

	[SerializeField]
	private Maze maze = null;

	public static event GuideTargetStaticEventHandler OnNewTarget;

	private int spikesCollected;
	public int SpikesCollected {
		get => spikesCollected;
		set {
			spikesCollected = value;
			HUDisplay.Instance.UpdateSpikeCounter(value);
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowCollected);
		}
	}

	#region Lifecycle

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		Player.OnSpikePickup += OnSpikePickup;
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		Player.OnSpikePickup -= OnSpikePickup;
	}

	#endregion

	private void OnSpikePickup(Player sender, Spike spike) {
		SpikesCollected++;

		gameObject.SetActive(false);

		if (spikesCollected == 1) {
			HUDisplay.Instance.DisplayInfo("Follow the blinking arrows.\n They will guide you to your target.", "Be aware of every detail on the screen.");
		}
		//Finished avoidance
		if (spikesCollected == 3) {
			CameraMovement.Instance.RaycastForRooms();
		}
		//Went through maze
		if (spikesCollected == 4) {
			maze.MazeEscape();
		}
		MapData.Instance.Progress(++Player.GameProgression);
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		SpikesCollected = data.player.spikesCollected;
		gameObject.SetActive(data.world.spikeActive);
		gameObject.transform.position = data.world.spikePos;
	}

	public void SetPosition(bool guideTowards = true) {
		float Xscale = gameObject.transform.lossyScale.x / 2;
		float Yscale = gameObject.transform.lossyScale.y / 2;
		float newX;
		float newY;
		switch (Player.GameProgression) {
			case 0: {
				RectTransform room1BG = MapData.Instance.GetRoom(1).Background;
				newX = room1BG.position.x;
				newY = room1BG.position.y - 1;
				break;
			}
			case 1: {
				RectTransform roomIciclesBG = MapData.Instance.GetRoom(2).Background;
				newX = Random.Range(roomIciclesBG.position.x - roomIciclesBG.sizeDelta.x / 2 + Xscale * 4, roomIciclesBG.position.x);
				newY = Random.Range(roomIciclesBG.position.y - roomIciclesBG.sizeDelta.y / 2 + Yscale * 4, roomIciclesBG.position.y);
				break;
			}
			case 2: {
				RectTransform roomAvoidanceBG = MapData.Instance.GetRoom(3).Background;
				newX = Random.Range(roomAvoidanceBG.position.x + (-roomAvoidanceBG.sizeDelta.x / 2) + Xscale, roomAvoidanceBG.position.x + (roomAvoidanceBG.sizeDelta.x / 2) - Xscale);
				newY = Random.Range(roomAvoidanceBG.position.y, roomAvoidanceBG.position.y + (roomAvoidanceBG.sizeDelta.y / 2) - Yscale);
				break;
			}
			case 3: {
				newX = maze.middleCell.position.x;
				newY = maze.middleCell.position.y;
				transform.localScale = Vector2.one * 3;
				break;
			}
			case 4: {
				RectTransform roomPreBossBG = MapData.Instance.GetRoom(4).Background;
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
		if (guideTowards) {
			OnNewTarget?.Invoke(transform.position);
		}
	}

	public void Hide() {
		gameObject.SetActive(false);
		OnNewTarget?.Invoke(default);
	}
}
