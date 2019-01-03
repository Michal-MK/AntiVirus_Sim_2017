using UnityEngine;

/// <summary>
/// Class representing a door between two rooms
/// </summary>
public class Door {

	/// <summary>
	/// The <see cref="RoomLink"/> between the Rooms, indicated where the door is
	/// </summary>
	public RoomLink connecting { get; }

	/// <summary>
	/// Get the middle Door GameObject
	/// </summary>
	public GameObject getDoor { get; }

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
	public Door(GameObject doorObj, RoomLink connects) {
		getDoor = doorObj;
		connecting = connects;
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
