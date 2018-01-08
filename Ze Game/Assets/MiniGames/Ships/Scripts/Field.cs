using System;
using System.Collections.Generic;
using UnityEngine;

namespace Igor.Minigames.Ships {
	[System.Serializable]
	public class Field {
		public static Field self;
		private Location[,] _locations;
		private List<Ship> placedShips = new List<Ship>();
		private Vector2 _dimensions;

		public Field(int x, int y) {
			self = this;
			_locations = new Location[x, y];
			_dimensions = new Vector2(x, y);
			for (int i = 0; i < x; i++) {
				for (int j = 0; j < y; j++) {
					_locations[i, j] = new Location(i, j);
				}
			}
		}

		public Field(Vector2 vector) {
			self = this;
			int x = (int)vector.x;
			int y = (int)vector.y;

			_locations = new Location[x, y];
			_dimensions = vector;
			for (int i = 0; i < x; i++) {
				for (int j = 0; j < y; j++) {
					_locations[i, j] = new Location(i, j);
				}
			}
		}

		public void Visualize(GameObject representation) {
			foreach (Location loc in _locations) {
				LocationVisual l = GameObject.Instantiate(representation, loc.coordinates, Quaternion.identity).GetComponent<LocationVisual>();
				l.name = loc.coordinates.ToString();
				loc.locationVisual = l;
				l.location = loc;
			}
			Camera.main.transform.position = new Vector3(_dimensions.x / 2 - 0.5f, _dimensions.y / 2 - 0.5f, -10);
		}

		public Location GetLocation(Vector2 coordinates) {
			if (coordinates.x >= 0 && coordinates.x < _dimensions.x && coordinates.y >= 0 && coordinates.y < _dimensions.y) {
				return _locations[(int)coordinates.x, (int)coordinates.y];
			}
			return null;
		}

		public Location GetLocation(Vector2 coordinates, Vector2 offset) {
			Vector2 final = new Vector2(coordinates.x + offset.x, coordinates.y + offset.y);

			if (final.x >= 0 && final.x < _dimensions.x && final.y >= 0 && final.y < _dimensions.y) {
				return _locations[(int)final.x, (int)final.y];
			}
			return null;
		}

		public void ClearHighlights() {
			foreach (Location location in _locations) {
				if (location.placedShip == ShipType.NONE) {
					location.locationVisual.Unhighlight();
				}
			}
		}

		public Location[,] locations {
			get { return _locations; }
		}

		public List<Ship> getAllShips {
			get { return placedShips; }
		}

		public void ShipSunk(Ship getPlacedShip) {
			placedShips.Remove(getPlacedShip);
			if (placedShips.Count == 0) {
				ShipsMain.script.GameOver();
			}
		}

		public Vector2 getDimensions {
			get { return _dimensions; }
		}
	}
}
