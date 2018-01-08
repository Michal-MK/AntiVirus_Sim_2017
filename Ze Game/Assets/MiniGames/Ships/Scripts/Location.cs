using UnityEngine;
using System.Collections.Generic;

namespace Igor.Minigames.Ships {
	[System.Serializable]
	public enum LocationState {
		NORMAL,
		HIT,
		MISS,
		SUNK,
		HINT,
		TARGET
	}


	public class Location {
		private int _x;
		private int _y;
		private ShipType _placedShip = ShipType.NONE;
		private LocationState _locationStatus = LocationState.NORMAL;
		private Vector2[,] neighbors;
		private LocationVisual attachedVisual;


		public Location(int x, int y) {
			_x = x;
			_y = y;

			if (x != 0 && y != 0 && x + 1 != Field.self.getDimensions.x && y + 1 != Field.self.getDimensions.y) {
				neighbors = new Vector2[3, 3] {
				{new Vector2(x-1,y-1), new Vector2(x,y-1), new Vector2(x+1,y-1) },
				{new Vector2(x-1,y),   new Vector2(x,y),   new Vector2(x+1,y),  },
				{new Vector2(x-1,y+1), new Vector2(x,y+1), new Vector2(x+1,y+1) }
				};
			}
			else {
				neighbors = new Vector2[3, 3];
				for (int i = -1; i <= 1; i++) {
					for (int j = -1; j <= 1; j++) {
						if (x + i >= 0 && x + i < Field.self.getDimensions.x && y + j >= 0 && y + j < Field.self.getDimensions.y) { //If we are in-bounds
							neighbors[i + 1, j + 1] = new Vector2(x + i, y + j);
						}
						else {
							neighbors[i + 1, j + 1] = new Vector2(-1, -1);
						}
					}
				}
			}
		}

		public Location GetNeighbor(Field field, Neighbors direction) {
			Vector2 myLocation = new Vector2(_x, _y);
			switch (direction) {
				case Neighbors.TOP_LEFT: {
					if (neighbors[0, 0] != -Vector2.one) {
						return field.GetLocation(myLocation, neighbors[0, 0]);
					}
					else {
						return null;
					}
				}
				case Neighbors.TOP_MIDDLE: {
					if (neighbors[1, 0] != -Vector2.one) {
						return field.GetLocation(myLocation, neighbors[1, 0]);
					}
					else {
						return null;
					}
				}
				case Neighbors.TOP_RIGHT: {
					if (neighbors[2, 0] != -Vector2.one) {
						return field.GetLocation(myLocation, neighbors[2, 0]);
					}
					else {
						return null;
					}
				}
				case Neighbors.MIDDLE_LEFT: {
					if (neighbors[0, 1] != -Vector2.one) {
						return field.GetLocation(myLocation, neighbors[0, 1]);
					}
					else {
						return null;
					}
				}
				case Neighbors.MIDDLE_MIDDLE: {
					return null;
				}
				case Neighbors.MIDDLE_RIGHT: {
					if (neighbors[2, 1] != -Vector2.one) {
						return field.GetLocation(myLocation, neighbors[2, 1]);
					}
					else {
						return null;
					}
				}
				case Neighbors.BOTTOM_LEFT: {
					if (neighbors[0, 2] != -Vector2.one) {
						return field.GetLocation(myLocation, neighbors[0, 2]);
					}
					else {
						return null;
					}
				}
				case Neighbors.BOTTOM_MIDDLE: {
					if (neighbors[1, 2] != -Vector2.one) {
						return field.GetLocation(myLocation, neighbors[1, 2]);
					}
					else {
						return null;
					}
				}
				case Neighbors.BOTTOM_RIGHT: {
					if (neighbors[2, 2] != -Vector2.one) {
						return field.GetLocation(myLocation, neighbors[2, 2]);
					}
					else {
						return null;
					}
				}
				default: {
					throw new System.Exception("What?");
				}
			}
		}

		public Location[] getNeighbors {
			get {
				List<Location> locations = new List<Location>();
				foreach (Vector2 vec in neighbors) {
					if (Field.self.GetLocation(vec) != null) {
						locations.Add(Field.self.GetLocation(vec));
					}
				}
				return locations.ToArray();
			}
		}

		public Location[] getNeighborsOnAxis {
			get {
				Vector2[,] axes = new Vector2[2, 2] { { Vector2.up, Vector2.right }, { Vector2.down, Vector2.left } };
				List<Location> locations = new List<Location>();
				foreach (Vector2 vec in axes) {
					if (Field.self.GetLocation(coordinates, vec) != null) {
						locations.Add(Field.self.GetLocation(coordinates, vec));
					}
				}
				return locations.ToArray();
			}
		}

		public Vector2 coordinates {
			get { return new Vector2(_x, _y); }
			set {
				_x = (int)value.x;
				_y = (int)value.y;
			}
		}

		public LocationVisual locationVisual {
			get { return attachedVisual; }
			set { attachedVisual = value; }
		}

		public bool PlaceShip(ShipType ship) {
			if (isAvailable && ship != ShipType.NONE) {
				_placedShip = ship;
				return true;
			}
			else {
				return false;
			}
		}

		public void RemoveShip() {
			if (_placedShip == ShipType.TOKEN) {
				bool staysToken = false;
				foreach (Location location in getNeighborsOnAxis) {
					if (location.placedShip != ShipType.NONE && location.placedShip != ShipType.TOKEN) {
						staysToken = true;
					}
				}
				if (!staysToken) {
					_placedShip = ShipType.NONE;
					locationVisual.Unhighlight();
				}
			}
			else {
				_placedShip = ShipType.NONE;
				locationVisual.Unhighlight();
			}
		}

		public ShipType placedShip {
			get { return _placedShip; }
		}

		public Ship getPlacedShip {
			get {
				foreach (Ship ship in Field.self.getAllShips) {
					foreach (Location location in ship.getLocation) {
						if (location == this) {
							return ship;
						}
					}
				}
				throw new System.Exception("No Ship exits at this Field");
			}
		}

		public LocationState locationState {
			get { return _locationStatus; }
			set { _locationStatus = value; }
		}

		public bool isAvailable {
			get { return _placedShip == ShipType.NONE; }
		}

		public bool isToken {
			get { return _placedShip == ShipType.TOKEN; }
		}
	}
}

