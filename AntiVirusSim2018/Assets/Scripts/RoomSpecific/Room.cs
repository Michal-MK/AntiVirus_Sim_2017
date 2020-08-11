using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing a single room on the map
/// </summary>
public class Room : MonoBehaviour {

	[SerializeField]
	private int roomID = 0;
	/// <summary>
	/// The ID of this room, starting at <see langword="1"/>
	/// </summary>
	public int RoomID => roomID;


	[SerializeField]
	private string roomName = "";
	/// <summary>
	/// The user friendly name of this <see cref="Room"/>
	/// </summary>
	public string RoomName => roomName;


	[SerializeField]
	private List<Door> doors = new List<Door>();
	/// <summary>
	/// All <see cref="Door"/>s that are going from this <see cref="Room"/> 
	/// </summary>
	public List<Door> OutgoingDoors => doors;


	/// <summary>
	/// The background of this <see cref="Room"/>
	/// </summary>
	public RectTransform Background { get; private set; }

	private void OnEnable() {
		foreach (Transform t in GetComponentInChildren<Transform>()) {
			if (t.name.Contains("Door_")) {
				string[] split = t.name.Split('_');
				doors.Add(new Door(t.GetChild(4).gameObject, split[2].Int(), split[3].Int()));
			}
			if (t.name.Contains("Background_")) {
				Background = t.GetComponent<RectTransform>();
			}
		}
		MapData.Instance.RegisterRoom(this);
	}
}
