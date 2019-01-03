using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class MapData : MonoBehaviour {

	public RectTransform[] bossBackgrounds;

	public RectTransform[] transitions;

	public Light globalDirectionalLight;

	public static MapData script;

	public Dictionary<int, Room> rooms = new Dictionary<int, Room>();

	public bool isBoss1Killed { get; } = false;
	public MapMode currentMapMode { get; private set; } = MapMode.LIGHT;


	public enum MapMode {
		LIGHT,
		DARK
	}

	internal void RegisterRoom(Room room) {
		rooms.Add(room.roomID, room);
	}

	private void Awake() {
		script = this;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		GameObject.Find("_SignPost Avoidance").SetActive(!data.world.doneAvoidance);
		GameObject.Find("_Blocker3").SetActive(!data.world.postMazeDoorOpen);
		for (int i = 0; i < data.world.doorsOpen.Count; i++) {
			string[] indicies = data.world.doorsOpen[i].Split(',');
			OpenDoor(new RoomLink(NumberToRoom(int.Parse(indicies[0])), NumberToRoom(int.Parse(indicies[1]))));
		}
	}

	public void SwitchMapMode(MapMode mode) {
		currentMapMode = mode;
		FindObjectOfType<EnemySpawner>().UpdatePrefabs(currentMapMode);
		foreach (TurretAttack turret in FindObjectsOfType<TurretAttack>()) {
			turret.MapStanceSwitch(mode);
		}
		foreach (SignPost sign in FindObjectsOfType<SignPost>()) {
			sign.MapStanceSwitch(mode);
		}
		foreach (ProjectileWall wall in FindObjectsOfType<ProjectileWall>()) {
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

	public void OpenDoor(RoomLink between, bool includeOpposite = true) {
		between.OpenDoor(includeOpposite);
	}

	public void CloseDoor(RoomLink between, bool includeOpposite = true) {
		between.CloseDoor(includeOpposite);
	}

	public void Progress(int progression, bool displayGuidance = true) {
		switch (progression) {
			case 1: {
				OpenDoor(GetRoomLink(1, 2));
				if (displayGuidance) {
					Canvas_Renderer.script.DisplayDirection(Directions.RIGHT);
				}
				break;
			}
			case 2: {
				OpenDoor(GetRoomLink(2, 3));
				if (displayGuidance) {
					Canvas_Renderer.script.DisplayDirection(Directions.TOP);
				}
				break;
			}
			case 3: {
				OpenDoor(GetRoomLink(2, 3));
				OpenDoor(GetRoomLink(2, 4));
				CloseDoor(GetRoomLink(1, 2));
				if (displayGuidance) {
					Canvas_Renderer.script.DisplayDirection(Directions.BOTTOM);
				}
				break;
			}
			case 10: {
				OpenDoor(GetRoomLink(1, 2));
				OpenDoor(GetRoomLink(4, 5));
				OpenDoor(GetRoomLink(5, 6));
				if (displayGuidance) {
					Canvas_Renderer.script.DisplayDirection(Directions.RIGHT);
				}
				break;
			}
		}
		if (CameraMovement.script != null) {
			CameraMovement.script.RaycastForRooms();
		}
	}

	/// <summary>
	/// Returns a room
	/// </summary>
	public Room GetRoom(int room) {
		return rooms[room];
	}

	public RectTransform GetBackgroundBoss(int room) {
		return bossBackgrounds[room - 1];
	}

	public RectTransform GetTransition(RoomLink link) {
		foreach (RectTransform t in transitions) {
			if (t.name == "Background_transition_" + link.from.roomID + "_" + link.to.roomID) {
				return t;
			}
		}
		throw new System.Exception("Transition between " + link.from.roomID + " to " + link.to.roomID + " does not exist!");
	}

	private void OnDestroy() {
		script = null;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}

	public Door[] getAllDoors {
		get {
			HashSet<Door> doors = new HashSet<Door>();
			foreach (Room r in rooms.Values) {
				doors.AddRange(r.doors);
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
			throw new System.Exception("Unable to find a room with index: " + index);
		}
		return r;
	}
}