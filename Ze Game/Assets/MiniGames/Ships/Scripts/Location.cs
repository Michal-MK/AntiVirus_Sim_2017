using UnityEngine;
using System.Collections.Generic;

namespace Igor.Minigames.Ships {

	public enum LocationState {
		NORMAL,
		HIT,
		MISS,
		SUNK,
		HINT,
		TARGET
	}

	[System.Serializable]
	public class Location {
		private int _x;
		private int _y;
		private ShipType _placedShip = ShipType.NONE;
		private LocationState _locationStatus = LocationState.NORMAL;
		private Vector2[,] neighbors;
		private LocationVisual attachedVisual;

		private static Dictionary<Vector2, Vector2> indexToLocation = new Dictionary<Vector2, Vector2> {
			{ new Vector2(0,0), new Vector2(-1, 1) },
			{ new Vector2(1,0), new Vector2(0 , 1) },
			{ new Vector2(2,0), new Vector2(1 , 1) },
			{ new Vector2(0,1), new Vector2(-1, 0) },
			{ new Vector2(1,1), new Vector2(0 , 0) },
			{ new Vector2(2,1), new Vector2(1 , 0) },
			{ new Vector2(0,2), new Vector2(-1,-1) },
			{ new Vector2(1,2), new Vector2(0 ,-1) },
			{ new Vector2(2,2), new Vector2(1 ,-1) },
		};

		public Location(int x, int y) {
			_x = x;
			_y = y;

			if (x != 0 && y != 0 && x + 1 != Field.self.getDimensions.x && y + 1 != Field.self.getDimensions.y) {
				neighbors = new Vector2[3, 3] {
				{new Vector2(x-1,y+1), new Vector2(x,y+1), new Vector2(x+1,y+1) },
				{new Vector2(x-1,y),   new Vector2(x,y),   new Vector2(x+1,y),  },
				{new Vector2(x-1,y-1), new Vector2(x,y-1), new Vector2(x+1,y-1) }
				};
			}
			else {
				neighbors = new Vector2[3, 3];
				Vector2 dim = Field.self.getDimensions;
				for (int column = 0; column <= 2; column++) {
					for (int row = 0; row <= 2; row++) {
						Vector2 rotated = indexToLocation[new Vector2(row, column)];
						Vector2 final = rotated + new Vector2(x, y);
						if (final.x >= 0 && final.y >= 0 && final.x < dim.x && final.y < dim.y) {
							neighbors[column, row] = rotated + new Vector2(x, y);
						}
						else {
							neighbors[column, row] = -Vector2.one;
						}
					}
				}
			}
		}
		/// <summary>
		/// Primitive Token Location
		/// </summary>
		public Location() {
			_placedShip = ShipType.TOKEN;
			_x = -1;
			_y = -1;
			neighbors = new Vector2[3, 3] {
				{-Vector2.one,-Vector2.one,-Vector2.one},{-Vector2.one,-Vector2.one,-Vector2.one},{-Vector2.one,-Vector2.one,-Vector2.one}
			};
		}

		/// <summary>
		/// Gets the neighbouring cell of this cell in the spcified direction.
		/// </summary>
		public Location GetNeighbor(Field field, Neighbors direction) {
			//Vector2 myLocation = new Vector2(_x, _y);
			switch (direction) {
				case Neighbors.TOP_LEFT: {
					if (neighbors[0, 0] != -Vector2.one) {
						return field.GetLocation(neighbors[0, 0]);
					}
					else {
						Debug.Log("Primitive");
						return new Location();
					}
				}
				case Neighbors.TOP_MIDDLE: {
					if (neighbors[0, 1] != -Vector2.one) {
						return field.GetLocation(neighbors[0, 1]);
					}
					else {
						Debug.Log("Primitive");

						return new Location();
					}
				}
				case Neighbors.TOP_RIGHT: {
					if (neighbors[0, 2] != -Vector2.one) {
						return field.GetLocation(neighbors[0, 2]);
					}
					else {
						Debug.Log("Primitive");

						return new Location();
					}
				}
				case Neighbors.MIDDLE_LEFT: {
					if (neighbors[1, 0] != -Vector2.one) {
						return field.GetLocation(neighbors[1, 0]);
					}
					else {
						Debug.Log("Primitive");

						return new Location();
					}
				}
				case Neighbors.MIDDLE_MIDDLE: {
					return null;
				}
				case Neighbors.MIDDLE_RIGHT: {
					if (neighbors[1, 2] != -Vector2.one) {
						return field.GetLocation(neighbors[1, 2]);
					}
					else {
						Debug.Log("Primitive");

						return new Location();
					}
				}
				case Neighbors.BOTTOM_LEFT: {
					if (neighbors[2, 0] != -Vector2.one) {
						return field.GetLocation(neighbors[2, 0]);
					}
					else {
						Debug.Log("Primitive");

						return new Location();
					}
				}
				case Neighbors.BOTTOM_MIDDLE: {
					if (neighbors[2, 1] != -Vector2.one) {
						return field.GetLocation(neighbors[2, 1]);
					}
					else {
						Debug.Log("Primitive");

						return new Location();
					}
				}
				case Neighbors.BOTTOM_RIGHT: {
					if (neighbors[2, 2] != -Vector2.one) {
						return field.GetLocation(neighbors[2, 2]);
					}
					else {
						Debug.Log("Primitive");

						return new Location();
					}
				}
				default: {
					throw new System.Exception("What?");
				}
			}
		}

		/// <summary>
		/// Gets all neighbouring cell on X/Y axes from this cell
		/// </summary>
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
		/// <summary>
		/// Cells coordinates in Field's 2D array
		/// </summary>
		public Vector2 coordinates {
			get { return new Vector2(_x, _y); }
			set {
				_x = (int)value.x;
				_y = (int)value.y;
			}
		}

		/// <summary>
		/// Visual representation of this cell
		/// </summary>
		public LocationVisual locationVisual {
			get { return attachedVisual; }
			set { attachedVisual = value; }
		}

		/// <summary>
		/// Place a ship on this location, boolean to represent success
		/// </summary>
		public bool PlaceShip(ShipType ship) {
			if (isAvailable && ship != ShipType.NONE) {
				_placedShip = ship;
				return true;
			}
			else {
				return false;
			}
		}

		/// <summary>
		/// Remove a ship from this location according to its neighours
		/// </summary>
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

		/// <summary>
		/// The ship  type at this location
		/// </summary>
		public ShipType placedShip {
			get { return _placedShip; }
		}

		/// <summary>
		/// The actual ship object that registres this location as one if its "HP"
		/// </summary>
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
		
		/// <summary>
		/// Can a ship be placed here
		/// </summary>
		public bool isAvailable {
			get { return _placedShip == ShipType.NONE; }
		}
		/// <summary>
		/// Is this location unavailable due to neighbouring ship
		/// </summary>
		public bool isToken {
			get { return _placedShip == ShipType.TOKEN; }
		}
	}
}

