using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapData : MonoBehaviour {

	[SerializeField]
	private RectTransform[] bossBackgrounds = null;

	[SerializeField]
	private RectTransform[] transitions = null;
	public RectTransform[] Transitions => transitions;

	[SerializeField]
	private Light globalDirectionalLight = null;

	public static MapData Instance { get; private set; }

	public Dictionary<int, Room> Rooms { get; } = new Dictionary<int, Room>();
	public bool BossOneKilled { get; set; } = false;
	public MapMode CurrentMapMode { get; private set; } = MapMode.LIGHT;


	#region Lifecycle

	private void Awake() {
		Instance = this;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void OnDestroy() {
		Instance = null;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}

	#endregion

	public void RegisterRoom(Room room) {
		Rooms.Add(room.RoomID, room);
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		GameObject.Find("_SignPost Avoidance").SetActive(!data.world.doneAvoidance);
		GameObject.Find("_Blocker3").SetActive(!data.world.postMazeDoorOpen);
		for (int i = 0; i < data.world.doorsOpen.Count; i++) {
			string[] indicies = data.world.doorsOpen[i].Split(',');
			new RoomLink(NumberToRoom(int.Parse(indicies[0])), NumberToRoom(int.Parse(indicies[1]))).OpenDoor();
		}
	}

	public void SwitchMapMode(MapMode mode) {
		CurrentMapMode = mode;
		FindObjectOfType<EnemySpawner>().UpdatePrefabs(CurrentMapMode);
		foreach (TurretAttack turret in FindObjectsOfType<TurretAttack>()) {
			turret.MapStanceSwitch(mode);
		}
		foreach (SignPost sign in FindObjectsOfType<SignPost>()) {
			sign.MapStanceSwitch(mode);
		}
		foreach (ProjectileWallController wall in FindObjectsOfType<ProjectileWallController>()) {
			wall.MapStanceSwitch(mode);
		}

		switch (mode) {
			case MapMode.LIGHT: {
				globalDirectionalLight.intensity = .8f;
				return;
			}
			case MapMode.DARK: {
				globalDirectionalLight.intensity = .1f;
				return;
			}
		}
	}

	public void Progress(int progression, bool displayGuidance = true) {
		switch (progression) {
			case 1: {
				GetRoomLink(1, 2).OpenDoor();
				if (displayGuidance) {
					HUDisplay.Instance.DisplayDirection(Directions.RIGHT);
				}
				break;
			}
			case 2: {
				GetRoomLink(2, 3).OpenDoor();
				if (displayGuidance) {
					HUDisplay.Instance.DisplayDirection(Directions.TOP);
				}
				break;
			}
			case 3: {
				GetRoomLink(2, 3).OpenDoor();
				GetRoomLink(2, 4).OpenDoor();
				GetRoomLink(1, 2).CloseDoor();
				if (displayGuidance) {
					HUDisplay.Instance.DisplayDirection(Directions.BOTTOM);
				}
				break;
			}
			case 10: {
				GetRoomLink(1, 2).OpenDoor();
				GetRoomLink(4, 5).OpenDoor();
				GetRoomLink(5, 6).OpenDoor();
				if (displayGuidance) {
					HUDisplay.Instance.DisplayDirection(Directions.RIGHT);
				}
				break;
			}
		}
		if (CameraMovement.Instance != null) {
			CameraMovement.Instance.RaycastForRooms();
		}
	}

	/// <summary>
	/// Returns a room by it's identifier
	/// </summary>
	public Room GetRoom(int room) {
		return Rooms[room];
	}

	public RectTransform GetBackgroundBoss(int room) {
		return bossBackgrounds[room - 1];
	}

	public RectTransform GetTransition(RoomLink link) {
		foreach (RectTransform t in transitions) {
			if (t.name == "Background_transition_" + link.From.RoomID + "_" + link.To.RoomID) {
				return t;
			}
		}
		throw new Exception("Transition between " + link.From.RoomID + " to " + link.To.RoomID + " does not exist!");
	}

	public bool IsOneOfAdjecentLinks(RectTransform background, int roomID) {
		Room room = GetRoom(roomID);
		string[] transitionNamesSplit = transitions.Where(trans => {
			string[] split = trans.name.Split('_');
			string roomIDString = room.RoomID.ToString();
			return split[2] == roomIDString || split[3] == roomIDString;
		}).Select(t => t.name).ToArray();

		foreach (string s in transitionNamesSplit) {
			if (string.Equals(s, background.name)) {
				return true;
			}
		}
		return false;
	}

	public Door[] AllDoors {
		get {
			HashSet<Door> doors = new HashSet<Door>();
			foreach (Room r in Rooms.Values) {
				doors.AddRange(r.OutgoingDoors);
			}
			return doors.ToArray();
		}
	}

	public RoomLink GetRoomLink(int from, int to) {
		return new RoomLink(NumberToRoom(from), NumberToRoom(to));
	}

	private Room NumberToRoom(int index) {
		Room r = GameObject.Find("Room_" + index).GetComponent<Room>();
		if (r == null) {
			throw new Exception("Unable to find a room with index: " + index);
		}
		return r;
	}
}