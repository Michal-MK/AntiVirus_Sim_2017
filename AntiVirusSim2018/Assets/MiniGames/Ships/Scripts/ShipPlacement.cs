using UnityEngine;
using System.Collections.Generic;
using Igor.Minigames.Ships;

public class ShipPlacement : MonoBehaviour {
	private bool _canPlace = false;
	private List<Location> _places = new List<Location>();
	private List<Location> _prevPlaces = new List<Location>();
	public static ShipPlacement current;

	public void StartChecking() {
		current = this;
	}

	public void StopChecking() {
		current = null;
		Destroy(gameObject);
	}

	private void FixedUpdate() {
		Raycaster[] casters = GetComponentsInChildren<Raycaster>();

		_prevPlaces.Clear();
		foreach (Location place in _places) {
			_prevPlaces.Add(place);
		}
		_places.Clear();
		_canPlace = true;

		foreach (Raycaster cast in casters) {
			if (cast.canPlace == false) {
				_canPlace = false;
			}
			if (cast.getSelected != null) {
				_places.Add(cast.getSelected);
			}
			else {
				_canPlace = false;
			}
		}
		ManageHighlight(_prevPlaces, _places);
	}

	private void ManageHighlight(List<Location> previous, List<Location> current) {
		foreach (Location prevPlace in previous) {
			if (prevPlace.isAvailable) {
				prevPlace.locationVisual.Unhighlight();
			}
		}
		foreach (Location currPlace in current) {
			if (_canPlace && currPlace.placedShip == ShipType.NONE) {
				currPlace.locationVisual.Highlight();
				//print("Hoghlighted " + currPlace.placedShip);
			}
			if (!_canPlace && currPlace.placedShip == ShipType.NONE) {
				currPlace.locationVisual.Occupied();
			}
		}

	}


	public bool canPlace {
		get { return _canPlace; }
	}

	public Location[] places {
		get { return _places.ToArray(); }
	}
}
