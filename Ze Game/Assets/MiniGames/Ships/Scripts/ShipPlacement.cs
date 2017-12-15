using UnityEngine;
using System.Collections.Generic;
using Igor.Minigames.Ships;

public class ShipPlacement : MonoBehaviour {
	private bool _canPlace = false;
	private List<Location> _places = new List<Location>();
	public static ShipPlacement current;

	public void StartChecking() {
		current = this;
	}

	private void FixedUpdate() {
		bool temp = true;
		Raycaster[] casters = GetComponentsInChildren<Raycaster>();
		_places.Clear();
		foreach (Raycaster cast in casters) {
			if (!cast.canPlace) {
				temp = false;
			}
			if (cast.getSelected != null) {
				_places.Add(cast.getSelected);
			}
		}
		_canPlace = temp;
	}

	public bool canPlace {
		get { return _canPlace; }
	}

	public Location[] places {
		get { return _places.ToArray(); }
	}
}
