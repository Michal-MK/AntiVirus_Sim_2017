using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igor.Minigames.Ships {
	[System.Serializable]
	public class Field {
		private Location[,] _locations;
		private List<Ship> placedShips = new List<Ship>();
		private Vector2 _dimensions;
		private Ships_UI.ViewingField fieldOwner;
		private Transform fieldParent;
		private bool isCurrentlyMoving = false;

		public Field(int width, int height) {
			_locations = new Location[width, height];
			_dimensions = new Vector2(width, height);
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					_locations[i, j] = new Location(i, j, this);
				}
			}
		}

		public Field(Vector2 vector) {
			int x = (int)vector.x;
			int y = (int)vector.y;

			_locations = new Location[x, y];
			_dimensions = vector;
			for (int i = 0; i < x; i++) {
				for (int j = 0; j < y; j++) {
					_locations[i, j] = new Location(i, j, this);
				}
			}
		}

		public void PopulateDefault() {
			GeneratorData g = new GeneratorData();
			LevelGenerator lg = new LevelGenerator(ShipsMain.singleplayer.getPlayerField, ShipsMain.singleplayer.getAiField, g);
			lg.Generate(this);
		}

		/// <summary>
		/// Spawns object that represent a field.
		/// </summary>
		public GameObject[] Visualize(GameObject representation, Ships_UI.ViewingField side) {
			List<GameObject> fields = new List<GameObject>();
			fieldParent = new GameObject("Field " + side.ToString()).transform;
			fieldOwner = side;
			foreach (Location loc in _locations) {
				LocationVisual l = GameObject.Instantiate(representation, loc.coordinates, Quaternion.identity, fieldParent.transform).GetComponent<LocationVisual>();
				l.name = loc.coordinates.ToString();
				loc.locationVisual = l;
				l.location = loc;
				fields.Add(l.gameObject);
			}
			if (side == Ships_UI.ViewingField.OPPONENT) {
				fieldParent.transform.position = new Vector3(-_dimensions.x * 4, 0, 0);
			}
			return fields.ToArray();
		}

		public void UpdateVisuals() {
			foreach (Location loc in _locations) {
				loc.locationVisual.UpdateVisual();
			}
		}

		public void Show(Ships_UI.ViewingField layout) {
			if (!isCurrentlyMoving) {
				GameObject.Find("Canvas").GetComponent<Ships_UI>().StartCoroutine(Move(layout));
			}
			isCurrentlyMoving = true;
			ShipsMain.ui.refHolder.currentField.text = layout == Ships_UI.ViewingField.PLAYER ? "Transitioning to Opponent field" : "Transitioning to Player field";
		}

		private IEnumerator Move(Ships_UI.ViewingField request) {
			if (request != fieldOwner) {
				for (float f = 0; f < 1; f += Time.deltaTime * 2) {
					fieldParent.transform.position = new Vector3(-_dimensions.x * 4 + _dimensions.x * 4 * f, 0, 0);
					yield return null;
				}
				fieldParent.transform.position = Vector3.zero;
			}
			else {
				for (float f = 0; f < 1; f += Time.deltaTime * 2) {
					fieldParent.transform.position = new Vector3(0 - _dimensions.x * 4 * f, 0, 0);
					yield return null;
				}
			}
			isCurrentlyMoving = false;
			if (request != Ships_UI.ViewingField.PLAYER) {
				ShipsMain.ui.refHolder.currentField.text = "Your field";
			}
			else {
				ShipsMain.ui.refHolder.currentField.text = "Opponents field";
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
		/// Gets parent GameObject of this field
		/// </summary>
		public Transform getFieldParent {
			get { return fieldParent; }
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
