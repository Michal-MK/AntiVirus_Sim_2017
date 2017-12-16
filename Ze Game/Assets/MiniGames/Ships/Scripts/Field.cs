using System.Collections.Generic;
using UnityEngine;

namespace Igor.Minigames.Ships {
	[System.Serializable]
	public class Field {
		public static Field self;
		private Location[,] _locations;
		private List<Ship> placedShips = new List<Ship>();

		public Field(int x, int y) {
			_locations = new Location[x, y];
			for (int i = 0; i < x; i++) {
				for (int j = 0; j < y; j++) {
					_locations[i, j] = new Location(i, j);
				}
			}
			self = this;
		}
		public Field(Vector2 vector) {
			int x = (int)vector.x;
			int y = (int)vector.y;

			_locations = new Location[x, y];
			for (int i = 0; i < x; i++) {
				for (int j = 0; j < y; j++) {
					_locations[i, j] = new Location(i, j);
				}
			}
			self = this;
		}

		public Location GetLocation(Vector2 coordinates) {
			if (coordinates.x >= 0 && coordinates.x < ShipsMain.getDimensions.x && coordinates.y >= 0 && coordinates.y < ShipsMain.getDimensions.y) {
				return _locations[(int)coordinates.x, (int)coordinates.y];
			}
			return null;
		}

		public Location GetLocation(Vector2 coordinates, Vector2 offset) {
			Vector2 final = new Vector2(coordinates.x + offset.x, coordinates.y + offset.y);

			if (final.x >= 0 && final.x < ShipsMain.getDimensions.x && final.y >= 0 && final.y < ShipsMain.getDimensions.y) {
				return _locations[(int)final.x, (int)final.y];
			}
			return null;
		}

		public void ClearHighlights() {
			foreach (Location location in _locations) {
				if (location.placedShip == ShipType.NONE) {
					location.LocationVisual.Unhighlight();
				}
			}
		}

		public Location[,] locations {
			get { return _locations; }
		}

		public List<Ship> getAllShips {
			get { return placedShips; }
		}
	}
}
