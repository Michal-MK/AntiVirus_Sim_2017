using System.Collections.Generic;
using UnityEngine;

public class Door {
	private List<GameObject> doorParts = new List<GameObject>();

	private RoomLink _doorBetweenXY;

	private bool _isOpen = false;


	public Door(GameObject doorObj, RoomLink connects) {
		Transform t = doorObj.transform.GetChild(4);
		doorParts.Add(t.gameObject);
		_doorBetweenXY = connects;
	}
	
	public RoomLink getRoomIndicies {
		get { return _doorBetweenXY; }
	}

	public List<GameObject> getDoor {
		get { return doorParts; }
	}

	public bool isDoorOpen {
		get { return _isOpen; }
		set { _isOpen = value; }
	}
}

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
public class RoomLink {

	private string _from;
	private string _to;

	public RoomLink(int fromRoom, int toRoom) {
		_from = fromRoom.ToString();
		_to = toRoom.ToString();
	}

	public RoomLink(string fromRoom, string toRoom) {
		_from = fromRoom;
		_to = toRoom;
	}

	public RoomLink(int fromRoom, string toRoom) {
		_from = fromRoom.ToString();
		_to = toRoom;
	}

	public RoomLink(string fromRoom, int toRoom) {
		_from = fromRoom;
		_to = toRoom.ToString();
	}

	public string From {
		get { return _from; }
	}
	public string To {
		get { return _to; }
	}

	public static bool operator == (RoomLink a, RoomLink b) {
		return a.From == b.From && a.To == b.To;
	}
	public static bool operator != (RoomLink a, RoomLink b) {
		return a.From != b.From || a.To != b.To;
	}
}
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)

