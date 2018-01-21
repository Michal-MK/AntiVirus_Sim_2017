using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapData : MonoBehaviour {

	public GameObject[] doors;
	public RectTransform[] backgrounds;
	public RectTransform[] bossBackgrounds;
	public RectTransform[] transitions;

	public static MapData script;

	private List<Door> _doors = new List<Door>();


	private bool boss1Killed = false;

	private void Awake() {
		script = this;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		Start();
		GameObject.Find("_SignPost Avoidance").SetActive(!data.world.doneAvoidance);
		GameObject.Find("_Blocker3").SetActive(data.world.postMazeDoorOpen);
		for (int i = 0; i < data.world.doorsOpen.Count; i++) {
			string[] indicies = data.world.doorsOpen[i].Split(',');
			OpenDoor(new RoomLink(indicies[0], indicies[1]));
		}
	}

	private void Start() {
		for (int i = 0; i < doors.Length - 1; i += 2) {
			string[] doorName = doors[i].name.Split('_');
			string from = doorName[2];
			string to = doorName[3];
			_doors.Add(new Door(doors[i], new RoomLink(from, to)));
			_doors.Add(new Door(doors[i + 1], new RoomLink(to, from)));
		}
	}

	public void OpenDoor(RoomLink between, bool includeOpposite = true) {
		foreach (Door d in _doors) {
			if (d.getRoomIndicies == between) {
				foreach (GameObject g in d.getDoor) {
					g.SetActive(false);
				}
				d.isDoorOpen = true;
			}
			if (includeOpposite) {
				if (d.getRoomIndicies == new RoomLink(between.To, between.From)) {
					foreach (GameObject g in d.getDoor) {
						g.SetActive(false);
					}
					d.isDoorOpen = true;
				}
			}
		}
	}

	public void CloseDoor(RoomLink between, bool includeOpposite = true) {
		foreach (Door d in _doors) {
			if (d.getRoomIndicies == between) {
				foreach (GameObject g in d.getDoor) {
					g.SetActive(true);
				}
				d.isDoorOpen = false;
			}
			if (includeOpposite) {
				if (d.getRoomIndicies == new RoomLink(between.To, between.From)) {
					foreach (GameObject g in d.getDoor) {
						g.SetActive(true);
					}
					d.isDoorOpen = false;
				}
			}
		}
	}

	public void Progress(int progression, bool displayGuidance = true) {
		switch (progression) {
			case 1: {
				OpenDoor(new RoomLink("1", "2"));
				if (displayGuidance) {
					Canvas_Renderer.script.DisplayDirection(Directions.RIGHT);
				}
				break;
			}
			case 2: {
				OpenDoor(new RoomLink("2", "3"));
				if (displayGuidance) {
					Canvas_Renderer.script.DisplayDirection(Directions.TOP);
				}
				break;
			}
			case 3: {
				OpenDoor(new RoomLink("2", "4"));
				CloseDoor(new RoomLink("1", "2"));
				if (displayGuidance) {
					Canvas_Renderer.script.DisplayDirection(Directions.BOTTOM);
				}
				break;
			}
			case 10: {
				OpenDoor(new RoomLink("1", "2"));
				OpenDoor(new RoomLink("4", "5"));
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
	/// Returns a background for selected room
	/// </summary>
	public RectTransform GetBackground(int room) {
		return backgrounds[room - 1];
	}

	public RectTransform GetBackgroundBoss(int room) {
		return backgrounds[room - 1];
	}

	public RectTransform GetTransition(RoomLink link) {
		foreach (RectTransform t in transitions) {
			string[] s = t.name.Split('_');
			string fromR = s[2];
			string toR = s[3];

			if((fromR == link.From && toR == link.To) || (fromR == link.To && toR == link.From)) {
				return t;
			}
		}
		throw new System.Exception("Transition does not exist!");
	}

	private void OnDestroy() {
		script = null;
		_doors.Clear();
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}

	public bool isBoss1Killed {
		get { return boss1Killed; }
	}

	public Door[] allDoors {
		get { return _doors.ToArray(); }
	}
}