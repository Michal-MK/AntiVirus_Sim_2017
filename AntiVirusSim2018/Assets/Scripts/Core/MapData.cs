using UnityEngine;
using System.Collections.Generic;

public class MapData : MonoBehaviour {

	public GameObject[] doors;
	public RectTransform[] backgrounds;
	public RectTransform[] bossBackgrounds;
	public RectTransform[] transitions;

	public Light globalDirectionalLight;

	public static MapData script;

	private readonly List<Door> _doors = new List<Door>();

	public bool isBoss1Killed { get; } = false;
	public MapMode currentMapMode { get; private set; } = MapMode.LIGHT;


	public enum MapMode {
		LIGHT,
		DARK
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
			OpenDoor(new RoomLink(int.Parse(indicies[0]), int.Parse(indicies[1])));
		}
	}

	private void Start() {
		for (int i = 0; i < doors.Length - 1; i += 2) {
			string[] doorName = doors[i].name.Split('_');
			int from = int.Parse(doorName[2]);
			int to = int.Parse(doorName[3]);
			_doors.Add(new Door(doors[i], new RoomLink(from, to)));
			_doors.Add(new Door(doors[i + 1], new RoomLink(to, from)));
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
		foreach (Door d in _doors) {
			if (d.getRoomIndicies == between) {
				foreach (GameObject g in d.getDoor) {
					g.SetActive(false);
				}
				d.isDoorOpen = true;
				if (!includeOpposite) {
					return;
				}
			}
			if (includeOpposite) {
				if (d.getRoomIndicies == new RoomLink(between.to, between.from)) {
					foreach (GameObject g in d.getDoor) {
						g.SetActive(false);
					}
					d.isDoorOpen = true;
					return;
				}
			}
		}
		throw new System.Exception("Door with indices " + between.from + " and " + between.to + " does not exist!");
	}

	public void CloseDoor(RoomLink between, bool includeOpposite = true) {
		foreach (Door d in _doors) {
			if (d.getRoomIndicies == between) {
				foreach (GameObject g in d.getDoor) {
					g.SetActive(true);
				}
				d.isDoorOpen = false;
				if (!includeOpposite) {
					return;
				}
			}
			if (includeOpposite) {
				if (d.getRoomIndicies == new RoomLink(between.to, between.from)) {
					foreach (GameObject g in d.getDoor) {
						g.SetActive(true);
					}
					d.isDoorOpen = false;
					return;
				}
			}
		}
		throw new System.Exception("Door with indices " + between.from + " and " + between.to + " does not exist!");
	}

	public void Progress(int progression, bool displayGuidance = true) {
		switch (progression) {
			case 1: {
				OpenDoor(new RoomLink(1, 2));
				if (displayGuidance) {
					Canvas_Renderer.script.DisplayDirection(Directions.RIGHT);
				}
				break;
			}
			case 2: {
				OpenDoor(new RoomLink(2, 3));
				if (displayGuidance) {
					Canvas_Renderer.script.DisplayDirection(Directions.TOP);
				}
				break;
			}
			case 3: {
				OpenDoor(new RoomLink(2, 4));
				CloseDoor(new RoomLink(1, 2));
				if (displayGuidance) {
					Canvas_Renderer.script.DisplayDirection(Directions.BOTTOM);
				}
				break;
			}
			case 10: {
				OpenDoor(new RoomLink(1, 2));
				OpenDoor(new RoomLink(4, 5));
				OpenDoor(new RoomLink(5, 6));
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
		return bossBackgrounds[room - 1];
	}

	public RectTransform GetTransition(RoomLink link) {
		foreach (RectTransform t in transitions) {
			string[] s = t.name.Split('_');
			int fromR = int.Parse(s[2]);
			int toR = int.Parse(s[3]);

			if ((fromR == link.from && toR == link.to) || (fromR == link.to && toR == link.from)) {
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

	public Door[] allDoors {
		get { return _doors.ToArray(); }
	}
}