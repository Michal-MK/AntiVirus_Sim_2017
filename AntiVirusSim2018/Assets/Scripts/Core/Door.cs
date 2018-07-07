using UnityEngine;

public class Door {

	public RoomLink getRoomIndicies { get; }
	public GameObject getDoor { get; }
	public bool isDoorOpen { get; set; }

	public Door(GameObject doorObj, RoomLink connects) {
		Transform t = doorObj.transform.GetChild(4);
		getDoor = t.gameObject;
		getRoomIndicies = connects;
		isDoorOpen = t.gameObject.activeInHierarchy;
	}

	public void Open() {
		getDoor.SetActive(false);
		isDoorOpen = true;
	}

	public void Close() {
		getDoor.SetActive(true);
		isDoorOpen = false;
	}
}

public class RoomLink {

	public int from { get; }
	public int to { get; }

	public RoomLink(int fromRoom, int toRoom) {
		from = fromRoom;
		to = toRoom;
	}
}