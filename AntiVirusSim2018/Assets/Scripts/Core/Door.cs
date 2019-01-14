using UnityEngine;

/// <summary>
/// Class representing a door between two rooms
/// </summary>
public class Door {

	/// <summary>
	/// Get the middle Door GameObject
	/// </summary>
	public GameObject getDoor { get; }

	/// <summary>
	/// The room this door is in
	/// </summary>
	public int fromRoomID { get; }

	/// <summary>
	/// The room this door is connecting, but not actually in
	/// </summary>
	public int toRoomID { get; }

	/// <summary>
	/// Toggle to Open/Close this door
	/// </summary>
	public bool isDoorOpen {
		get {
			return getDoor.gameObject.activeInHierarchy;
		}
		set {
			getDoor.SetActive(!value);
		}
	}

	/// <summary>
	/// Default constructor
	/// </summary>
	public Door(GameObject doorObj, int fromRoomID, int toRoomID) {
		getDoor = doorObj;
		this.fromRoomID = fromRoomID;
		this.toRoomID = toRoomID;
	}

	/// <summary>
	/// Function to open this door
	/// </summary>
	public void Open() {
		isDoorOpen = true;
	}

	/// <summary>
	/// Function to close this door
	/// </summary>
	public void Close() {
		isDoorOpen = false;
	}
}
