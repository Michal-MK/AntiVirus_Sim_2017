using UnityEngine;

/// <summary>
/// Class representing a door between two rooms
/// </summary>
public class Door {

	/// <summary>
	/// Get the middle Door GameObject
	/// </summary>
	public GameObject DoorObj { get; }

	/// <summary>
	/// The room this door is in
	/// </summary>
	public int FromRoomID { get; }

	/// <summary>
	/// The room this door is connecting, but not actually in
	/// </summary>
	public int ToRoomID { get; }

	/// <summary>
	/// Toggle to Open/Close this door
	/// </summary>
	public bool IsOpen {
		get => DoorObj.activeInHierarchy;
		set { DoorObj.SetActive(!value); }
	}

	public Door(GameObject doorObj, int fromRoomID, int toRoomID) {
		DoorObj = doorObj;
		FromRoomID = fromRoomID;
		ToRoomID = toRoomID;
	}

	public void Open() {
		IsOpen = true;
	}

	public void Close() {
		IsOpen = false;
	}
}
