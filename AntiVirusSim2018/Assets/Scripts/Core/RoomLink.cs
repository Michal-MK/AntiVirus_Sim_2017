using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Structure representing a link between two rooms
/// </summary>
public struct RoomLink {

	/// <summary>
	/// The <see cref="Room"/> from which this link goes
	/// </summary>
	public Room from { get; }

	/// <summary>
	/// The <see cref="Room"/> to which this link goes
	/// </summary>
	public Room to { get; }


	private Tuple<Door, Door> connection { get; }

	/// <summary>
	/// The <see cref="RectTransform"/> of the transition between <see cref="Room"/>s
	/// </summary>
	public RectTransform transition;

	/// <summary>
	/// Default constructor
	/// </summary>
	public RoomLink(Room fromRoom, Room toRoom) {
		from = fromRoom;
		to = toRoom;
		transition = MapData.script.transitions.SelectUnique((RectTransform t) => { return t.name.Contains(fromRoom.roomID.ToString()) && t.name.Contains(toRoom.roomID.ToString()); });
		connection = new Tuple<Door, Door>(from.doors.Find((d) => { return d.fromRoomID == fromRoom.roomID && d.toRoomID == toRoom.roomID; }),
										   to.doors.Find((d) => { return d.fromRoomID == toRoom.roomID && d.toRoomID == fromRoom.roomID; }));
	}

	/// <summary>
	/// Open the door
	/// </summary>
	public void OpenDoor(bool both = true) {
		connection.Item1.Open();
		if (both)
			connection.Item2.Open();
	}

	/// <summary>
	/// Open the door
	/// </summary>
	public void CloseDoor(bool both = true) {
		connection.Item1.Close();
		if (both)
			connection.Item2.Close();
	}

	#region Equality comparison

	public static bool operator ==(RoomLink a, RoomLink b) {
		if (a.from == b.from && a.to == b.to) {
			return true;
		}
		return false;
	}

	public static bool operator !=(RoomLink a, RoomLink b) {
		return !(a == b);
	}

	public override bool Equals(object obj) {
		if (!(obj is RoomLink)) {
			return false;
		}

		RoomLink link = (RoomLink)obj;
		return EqualityComparer<Room>.Default.Equals(from, link.from) &&
			   EqualityComparer<Room>.Default.Equals(to, link.to);
	}

	public override int GetHashCode() {
		int hashCode = -1951484959;
		hashCode = hashCode * -1521134295 + EqualityComparer<Room>.Default.GetHashCode(from);
		hashCode = hashCode * -1521134295 + EqualityComparer<Room>.Default.GetHashCode(to);
		return hashCode;
	}

	#endregion
}