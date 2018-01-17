using System.Collections.Generic;
using UnityEngine;

namespace Igor.Minigames.Ships {
	[System.Serializable]
	public class Field {
		public static Field self;
		private Location[,] _locations;
		private List<Ship> placedShips = new List<Ship>();
		private Vector2 _dimensions;

		public Field(int width, int height) {
			self = this;
			_locations = new Location[width, height];
			_dimensions = new Vector2(width, height);
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
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

		/// <summary>
		/// Spawns object that represent a field.
		/// </summary>
		public GameObject[] Visualize(GameObject representation) {
			List<GameObject> fields = new List<GameObject>();
			foreach (Location loc in _locations) {
				LocationVisual l = GameObject.Instantiate(representation, loc.coordinates, Quaternion.identity).GetComponent<LocationVisual>();
				l.name = loc.coordinates.ToString();
				loc.locationVisual = l;
				l.location = loc;
				fields.Add(l.gameObject);
			}
			Camera.main.GetComponent<CameraAdjust>().Adjust();
			return fields.ToArray();
		}

		public void UpdateVisuals() {
			foreach (Location loc in _locations) {
				loc.locationVisual.UpdateVisual();
			}
		}

		/// <summary>
		/// Gets a location based on where it is in the grid.
		/// </summary>
		public Location GetLocation(Vector2 coordinates) {
			if (coordinates.x >= 0 && coordinates.x < _dimensions.x && coordinates.y >= 0 && coordinates.y < _dimensions.y) {
				return _locations[(int)coordinates.x, (int)coordinates.y];
			}
			return null;
		}

		/// <summary>
		/// Gets a location based on known location, and offset.
		/// </summary>
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

		/// <summary>
		/// Removes a ship from the game
		/// </summary>
		public void ShipSunk(Ship attackedShip) {
			placedShips.Remove(attackedShip);
			if (placedShips.Count == 0) {
				ShipsMain.singleplayer.GameOver();
			}
		}

		/// <summary>
		/// Get all location in the field
		/// </summary>
		public Location[,] locations {
			get { return _locations; }
		}

		/// <summary>
		/// Get all ships that placed on the field
		/// </summary>
		public List<Ship> getAllShips {
			get { return placedShips; }
		}

		/// <summary>
		/// Get the X and Y dimensions of the field
		/// </summary>
		public Vector2 getDimensions {
			get { return _dimensions; }
		}
	}
}
