using System.Collections.Generic;
using UnityEngine;

public class Door {
	private static List<Door> doors = new List<Door>();
	private List<GameObject> doorParts = new List<GameObject>();

	private int openOrder_;

	public Door(GameObject doorObj, int order) {
		Transform t = doorObj.transform.GetChild(4);
		doorParts.Add(t.gameObject);
		openOrder_ = order;
		doors.Add(this);
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += delegate { doors.Clear(); };
	}

	~Door() {
		UnityEngine.SceneManagement.SceneManager.sceneLoaded -= delegate { doors.Clear(); };
	}

	public Door GetDoor(int atOrder) {
		for (int i = 0; i < doors.Count; i++) {
			if (doors[i].openOrder_ == atOrder) {
				return doors[i];
			}
		}
		throw new System.Exception("Door does not exist!");
	}

	public int getDoorOrder {
		get { return openOrder_; }
	}

	public List<GameObject> getDoor {
		get { return doorParts; }
	}

	public static Door[] getDoors {
		get { return doors.ToArray(); }
	}
}

