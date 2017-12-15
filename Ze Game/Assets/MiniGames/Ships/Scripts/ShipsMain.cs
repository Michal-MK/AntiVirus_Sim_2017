using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Igor.Minigames.Ships {
	public enum Neighbors {
		TOP_LEFT,
		TOP_MIDDLE,
		TOP_RIGHT,
		MIDDLE_LEFT,
		MIDDLE_MIDDLE,
		MIDDLE_RIGHT,
		BOTTOM_LEFT,
		BOTTOM_MIDDLE,
		BOTTOM_RIGHT
	}

	public enum ShipType {
		NONE = -1,
		TOKEN,
		SUBMARINE,
		CARGO,
		WAR,
		AIR,
		BATTLECRUSER
	}

	public enum CursorMode {
		SHIP_PLACEMENT,
		SHIP_REMOVE
	}


	public class ShipsMain : MonoBehaviour {
		public Vector2 dimensionsPublic;
		public GameObject locationObj;

		private static Vector2 dimensions;
		private Field field;

		private ShipType type = ShipType.NONE;

		void Start() {
			Cursor.visible = true;
			dimensions = dimensionsPublic;
			field = new Field(dimensionsPublic);
			Visualize(field);
		}

		private void Visualize(Field field) {
			Location[,] locations = field.locations;
			foreach (Location loc in locations) {
				GameObject l = Instantiate(locationObj, loc.coordinates, Quaternion.identity);
				l.name = loc.coordinates.ToString();
				loc.LocationVisual = l.GetComponent<LocationVisual>();
				l.GetComponent<LocationVisual>().location = loc;
			}
			Camera.main.transform.position = new Vector3(dimensions.x / 2 - 0.5f, dimensions.y / 2 - 0.5f, -10);
		}

		public static Vector2 getDimensions {
			get { return dimensions; }
		}
	}
}