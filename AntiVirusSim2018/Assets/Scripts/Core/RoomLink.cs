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
	public Room From { get; }

	/// <summary>
	/// The <see cref="Room"/> to which this link goes
	/// </summary>
	public Room To { get; }

	private Tuple<Door, Door> Connection { get; }

	public RectTransform Transition { get; }

	public RoomLink(Room fromRoom, Room toRoom) {
		From = fromRoom;
		To = toRoom;
		Transition = MapData.Instance.Transitions.SelectUnique((RectTransform t) => { return t.name.Contains(fromRoom.RoomID.ToString()) && t.name.Contains(toRoom.RoomID.ToString()); });
		Connection = new Tuple<Door, Door>(From.OutgoingDoors.Find((d) => d.FromRoomID == fromRoom.RoomID && d.ToRoomID == toRoom.RoomID),
										   To.OutgoingDoors.Find((d) => d.FromRoomID == toRoom.RoomID && d.ToRoomID == fromRoom.RoomID));
	}

	public void OpenDoor(bool both = true) {
		Connection.Item1.Open();
		if (both) Connection.Item2.Open();
	}

	public void CloseDoor(bool both = true) {
		Connection.Item1.Close();
		if (both) Connection.Item2.Close();
	}

	#region Equality comparison

	public static bool operator ==(RoomLink a, RoomLink b) {
		if (a.From == b.From && a.To == b.To) {
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
		return EqualityComparer<Room>.Default.Equals(From, link.From) &&
			   EqualityComparer<Room>.Default.Equals(To, link.To);
	}

	public override int GetHashCode() {
		int hashCode = -1951484959;
		hashCode = hashCode * -1521134295 + EqualityComparer<Room>.Default.GetHashCode(From);
		hashCode = hashCode * -1521134295 + EqualityComparer<Room>.Default.GetHashCode(To);
		return hashCode;
	}

	#endregion
}