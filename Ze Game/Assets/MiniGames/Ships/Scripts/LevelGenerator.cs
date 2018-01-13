using UnityEngine;
using System.Collections.Generic;
using System;

namespace Igor.Minigames.Ships {
	public class LevelGenerator {

		private enum Rotation {
			BASE_TO_UP,
			BASE_TO_RIGHT,
			BASE_TO_DOWN,
			BASE_TO_LEFT
		}

		private enum CargoShipRotation {
			BASE_TO_UP,
			BASE_TO_RIGHT,
			BASE_TO_DOWN,
			BASE_TO_LEFT
		}

		private enum WarShipRotation {
			BASE_TOP_VERTICAL,
			BASE_TOP_HORIZONTAL,
			BASE_CENTER_VERTICAL,
			BASE_CENTER_HORIZONTAL,
			BASE_BOTTOM_VERTICAL,
			BASE_BOTTOM_HORIZONTAL,
		}

		private enum AirShipRotation {
			BASE_TOP_THREE_DOWN,
			BASE_TOP_TWO_RIGHT_ONE_LEFT,
			BASE_TOP_TWO_LEFT_ONE_RIGHT,

			BASE_T_CENTER_TWO_UP_ONE_DOWN,
			BASE_T_CENTER_TWO_RIGHT_ONE_LEFT,
			BASE_T_CENTER_TWO_LEFT_ONE_RIGHT,

			BASE_B_CENTER_TWO_DOWN_ONE_UP,
			BASE_B_CENTER_TWO_RIGHT_ONE_LEFT,
			BASE_B_CENTER_TWO_LEFT_ONE_RIGHT,

			BASE_BOTTOM_TWO_RIGHT_ONE_LEFT,
			BASE_BOTTOM_TWO_LEFT_ONE_RIGHT,
			BASE_BOTTOM_THREE_UP
		}


		private Field field;
		private GeneratorData data;

		private int subsPlaced = 0;
		private int cargoPlaced = 0;
		private int warPlaced = 0;
		private int airPlaced = 0;
		private int cruiserPlaced = 0;

		public LevelGenerator(Field field, GeneratorData config) {
			this.field = field;
			data = config;
			//PlaceShip(field.GetLocation(Vector2.one), ShipType.BATTLECRUSER, Rotation.BASE_TO_UP, true);
		}

		public void Generate() {
			List<Location> locations = GetAvailableLocationList();
			//while (data.getCruisers != cruiserPlaced) {
			//	int rnd = UnityEngine.Random.Range(0, locations.Count);
			//	Location selected = locations[rnd];
			//	if (selected.isAvailable) {
			//		bool canPlace = true;
			//		for (int i = 0; i < selected.getNeighborsOnAxis.Length; i++) {
			//			if (!selected.getNeighborsOnAxis[i].isAvailable) {
			//				canPlace = false;
			//			}
			//		}
			//		if (canPlace) {
			//			selected.PlaceShip(ShipType.BATTLECRUSER);
			//			cruiserPlaced++;
			//			Debug.Log("Placed BATTLECRUISER at: " + selected.coordinates);
			//			for (int j = 0; j < selected.getNeighborsOnAxis.Length; j++) {
			//				for (int i = 0; i < selected.getNeighborsOnAxis[j].getNeighborsOnAxis.Length; i++) {
			//					locations.Remove(selected.getNeighborsOnAxis[j].getNeighborsOnAxis[i]);
			//				}
			//				locations.Remove(selected.getNeighborsOnAxis[j]);
			//			}
			//			locations.Remove(selected);
			//		}
			//	}
			//	Debug.Log(locations.Count);
			//}

			//while (data.getAirShips != airPlaced) {


			//}
			//while (data.getWarShips != warPlaced) {


			//}
			//while (data.getCargoShips != cargoPlaced) {


			//}
			//while (data.getSubmarines != subsPlaced) {


			//}
		}

		public List<Location> GetAvailableLocationList() {
			List<Location> l = new List<Location>();
			foreach (Location loc in field.locations) {
				if (loc.isAvailable) {
					l.Add(loc);
				}
			}
			return l;
		}

		private void PlaceSubmarine(Location baseLocation, bool visualize = false) {
			List<Location> newShipLocations = new List<Location>();

			baseLocation.PlaceShip(ShipType.SUBMARINE);
			newShipLocations.Add(baseLocation);
			FillTokens(newShipLocations);
			if (visualize) {
				field.UpdateVisuals();
			}
		}

		private void PlaceCargo(Location baseLocation, CargoShipRotation rotation, bool visualize = false) {
			List<Location> newShipLocations = new List<Location>();
			baseLocation.PlaceShip(ShipType.CARGO);
			newShipLocations.Add(baseLocation);
			switch (rotation) {
				case CargoShipRotation.BASE_TO_UP: {
					Location l = field.GetLocation(baseLocation.coordinates, Vector2.up);
					l.PlaceShip(ShipType.CARGO);
					newShipLocations.Add(l);
					break;
				}
				case CargoShipRotation.BASE_TO_RIGHT: {
					Location l = field.GetLocation(baseLocation.coordinates, Vector2.right);
					l.PlaceShip(ShipType.CARGO);
					newShipLocations.Add(l);
					break;
				}
				case CargoShipRotation.BASE_TO_DOWN: {
					Location l = field.GetLocation(baseLocation.coordinates, Vector2.down);
					l.PlaceShip(ShipType.CARGO);
					newShipLocations.Add(l);
					break;
				}
				case CargoShipRotation.BASE_TO_LEFT: {
					Location l = field.GetLocation(baseLocation.coordinates, Vector2.left);
					l.PlaceShip(ShipType.CARGO);
					newShipLocations.Add(l);
					break;
				}
			}
			FillTokens(newShipLocations);
			if (visualize) {
				field.UpdateVisuals();
			}
		}

		private void PlaceWar(Location baseLocation, WarShipRotation rotation, bool visualize = false) {
			List<Location> newShipLocations = new List<Location>();

			baseLocation.PlaceShip(ShipType.WAR);
			switch (rotation) {
				case WarShipRotation.BASE_CENTER_VERTICAL: {
					Location upper = field.GetLocation(baseLocation.coordinates, Vector2.up);
					Location lower = field.GetLocation(baseLocation.coordinates, Vector2.down);
					upper.PlaceShip(ShipType.WAR);
					lower.PlaceShip(ShipType.WAR);
					newShipLocations.Add(upper);
					newShipLocations.Add(lower);
					break;
				}
				case WarShipRotation.BASE_CENTER_HORIZONTAL: {
					Location right = field.GetLocation(baseLocation.coordinates, Vector2.right);
					Location left = field.GetLocation(baseLocation.coordinates, Vector2.left);
					right.PlaceShip(ShipType.WAR);
					left.PlaceShip(ShipType.WAR);
					newShipLocations.Add(right);
					newShipLocations.Add(left);
					break;
				}
				default: {
					throw new Exception("NIY");
				}
			}
			FillTokens(newShipLocations);
			if (visualize) {
				field.UpdateVisuals();
			}
		}

		private void PlaceAir(Location baseLocation, AirShipRotation rotation, bool visualize = false) {
			List<Location> newShipLocations = new List<Location>();

			baseLocation.PlaceShip(ShipType.AIR);
			switch (rotation) {
				case AirShipRotation.BASE_T_CENTER_TO_UP: {
					baseLocation.PlaceShip(ShipType.AIR);

					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location mid_lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location lower = mid_lower.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);

					upper.PlaceShip(ShipType.AIR);
					mid_lower.PlaceShip(ShipType.AIR);
					lower.PlaceShip(ShipType.AIR);

					newShipLocations.Add(upper);
					newShipLocations.Add(mid_lower);
					newShipLocations.Add(lower);
					break;
				}
				case AirShipRotation.BASE_T_CENTER_TO_RIGHT: {
					baseLocation.PlaceShip(ShipType.AIR);

					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location mid_lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location lower = mid_lower.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);

					upper.PlaceShip(ShipType.AIR);
					mid_lower.PlaceShip(ShipType.AIR);
					lower.PlaceShip(ShipType.AIR);

					newShipLocations.Add(upper);
					newShipLocations.Add(mid_lower);
					newShipLocations.Add(lower);
					break;
				}
				case AirShipRotation.BASE_T_CENTER_TO_DOWN: {
					baseLocation.PlaceShip(ShipType.AIR);

					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location mid_lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location lower = mid_lower.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);

					upper.PlaceShip(ShipType.AIR);
					mid_lower.PlaceShip(ShipType.AIR);
					lower.PlaceShip(ShipType.AIR);

					newShipLocations.Add(upper);
					newShipLocations.Add(mid_lower);
					newShipLocations.Add(lower);
					break;
				}
				case AirShipRotation.BASE_T_CENTER_TO_LEFT: {
					baseLocation.PlaceShip(ShipType.AIR);

					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location mid_lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location lower = mid_lower.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);

					upper.PlaceShip(ShipType.AIR);
					mid_lower.PlaceShip(ShipType.AIR);
					lower.PlaceShip(ShipType.AIR);

					newShipLocations.Add(upper);
					newShipLocations.Add(mid_lower);
					newShipLocations.Add(lower);
					break;
				}
				case AirShipRotation.BASE_B_CENTER_TO_UP: {
					baseLocation.PlaceShip(ShipType.AIR);

					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location mid_lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location lower = mid_lower.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);

					upper.PlaceShip(ShipType.AIR);
					mid_lower.PlaceShip(ShipType.AIR);
					lower.PlaceShip(ShipType.AIR);

					newShipLocations.Add(upper);
					newShipLocations.Add(mid_lower);
					newShipLocations.Add(lower);
					break;
				}
				case AirShipRotation.BASE_B_CENTER_TO_RIGHT: {
					baseLocation.PlaceShip(ShipType.AIR);

					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location mid_lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location lower = mid_lower.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);

					upper.PlaceShip(ShipType.AIR);
					mid_lower.PlaceShip(ShipType.AIR);
					lower.PlaceShip(ShipType.AIR);

					newShipLocations.Add(upper);
					newShipLocations.Add(mid_lower);
					newShipLocations.Add(lower);
					break;
				}
				case AirShipRotation.BASE_B_CENTER_TO_DOWN: {
					baseLocation.PlaceShip(ShipType.AIR);

					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location mid_lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location lower = mid_lower.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);

					upper.PlaceShip(ShipType.AIR);
					mid_lower.PlaceShip(ShipType.AIR);
					lower.PlaceShip(ShipType.AIR);

					newShipLocations.Add(upper);
					newShipLocations.Add(mid_lower);
					newShipLocations.Add(lower);
					break;
				}
				case AirShipRotation.BASE_B_CENTER_TO_LEFT: {
					baseLocation.PlaceShip(ShipType.AIR);

					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location mid_lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location lower = mid_lower.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);

					upper.PlaceShip(ShipType.AIR);
					mid_lower.PlaceShip(ShipType.AIR);
					lower.PlaceShip(ShipType.AIR);

					newShipLocations.Add(upper);
					newShipLocations.Add(mid_lower);
					newShipLocations.Add(lower);
					break;
				}
				default: {
					throw new Exception("NIY");
				}
			}

			FillTokens(newShipLocations);
			if (visualize) {
				field.UpdateVisuals();
			}
		}

		private void PlaceBattlecruier(Location baseLocation, bool visualize = false) {
			List<Location> newShipLocations = new List<Location>();

			baseLocation.PlaceShip(ShipType.BATTLECRUSER);
			Location[] neighbors = baseLocation.getNeighborsOnAxis;
			for (int i = 0; i < neighbors.Length; i++) {
				neighbors[i].PlaceShip(ShipType.BATTLECRUSER);
				newShipLocations.Add(neighbors[i]);
			}
			FillTokens(newShipLocations);
			if (visualize) {
				field.UpdateVisuals();
			}
		}

		private void FillTokens(List<Location> affected) {
			for (int i = 0; i < affected.Count; i++) {
				for (int j = 0; j < affected[i].getNeighborsOnAxis.Length; j++) {
					if (!affected[i].getNeighborsOnAxis[j].isToken) {
						affected[i].getNeighborsOnAxis[j].PlaceShip(ShipType.TOKEN);
					}
				}
			}
		}

		public void Reset() {

		}
	}
}