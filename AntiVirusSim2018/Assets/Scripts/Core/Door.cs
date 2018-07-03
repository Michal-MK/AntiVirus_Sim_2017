using System.Collections.Generic;
using UnityEngine;

public class Door {

	public RoomLink getRoomIndicies { get; }
	public List<GameObject> getDoor { get; } = new List<GameObject>();
	public bool isDoorOpen { get; set; } = false;

	public Door(GameObject doorObj, RoomLink connects) {
		Transform t = doorObj.transform.GetChild(4);
		getDoor.Add(t.gameObject);
		getRoomIndicies = connects;
	}
}

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
public class RoomLink {

	public int from { get; }
	public int to { get; }


	public RoomLink(int fromRoom, int toRoom) {
		from = fromRoom;
		to = toRoom;
	}

	public static bool operator == (RoomLink a, RoomLink b) {
		return a.from == b.from && a.to == b.to;
	}
	public static bool operator != (RoomLink a, RoomLink b) {
		return a.from != b.from || a.to != b.to;
	}
}
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)

