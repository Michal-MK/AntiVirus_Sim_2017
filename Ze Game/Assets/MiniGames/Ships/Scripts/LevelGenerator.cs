using UnityEngine;
using System.Collections.Generic;
using System;

namespace Igor.Minigames.Ships {
	public class LevelGenerator {

		private enum CargoShipRotation {
			ONE_UP,
			ONE_RIGHT,
			ONE_DOWN,
			ONE_LEFT
		}

		private enum WarShipRotation {
			TWO_UP,
			TWO_RIGHT,
			TWO_DOWN,
			TWO_LEFT,
			VERTICAL,
			HORIZONTAL
		}

		private enum AirShipRotation {
			THREE_UP,
			THREE_RIGHT,
			THREE_DOWN,
			THREE_LEFT,
			TWO_UP_ONE,
			TWO_RIGHT_ONE,
			TWO_LEFT_ONE,
			TWO_DOWN_ONE
		}


		private Field field;
		private GeneratorData data;

		private int subsPlaced = 0;
		private int cargoPlaced = 0;
		private int warPlaced = 0;
		private int airPlaced = 0;
		private int cruiserPlaced = 0;


		private int counter = 0;

		public LevelGenerator(Field field, GeneratorData config) {
			this.field = field;
			data = config;
		}

		public void Generate() {
			List<Location> freeLocations = GetAvailableLocations();

			//Ship placement, from biggest (occupying th most locations) to the smallest (Submarines)

			while (data.getCruisers != cruiserPlaced) {
				int rnd = UnityEngine.Random.Range(0, freeLocations.Count);
				Location selected = freeLocations[rnd];
				if (selected.isAvailable && IsInValidCruiserLocation(selected.coordinates)) {
					bool canPlace = true;
					for (int i = 0; i < selected.getNeighborsOnAxis.Length; i++) {
						if (!selected.getNeighborsOnAxis[i].isAvailable) {
							canPlace = false;
						}
					}
					if (canPlace) {
						PlaceBattlecruier(selected, true);
						cruiserPlaced++;
						freeLocations.RemoveAll((Location l) => !l.isAvailable);
					}
				}
				counter++;
				if(counter > 100) {
					throw new Exception("Over limit! Cruiser");
				}
			}
			Debug.Log(counter);
			while (data.getAirShips != airPlaced) {
				List<AirShipRotation> rotations = new List<AirShipRotation> {
					AirShipRotation.THREE_UP, AirShipRotation.THREE_RIGHT, AirShipRotation.THREE_DOWN, AirShipRotation.THREE_LEFT,
					AirShipRotation.TWO_UP_ONE, AirShipRotation.TWO_RIGHT_ONE, AirShipRotation.TWO_DOWN_ONE, AirShipRotation.TWO_DOWN_ONE
				};
				int rnd = UnityEngine.Random.Range(0, freeLocations.Count);
				Location selected = freeLocations[rnd];
				//selected = field.GetLocation(new Vector2(1, 1));

				#region OneByOne Location checking
				if (!selected.GetNeighbor(field, Neighbors.TOP_MIDDLE).isAvailable) {
					rotations.Remove(AirShipRotation.THREE_UP);
					rotations.Remove(AirShipRotation.TWO_DOWN_ONE);
					rotations.Remove(AirShipRotation.TWO_UP_ONE);
				}
				if (!selected.GetNeighbor(field, Neighbors.MIDDLE_RIGHT).isAvailable) {
					rotations.Remove(AirShipRotation.THREE_RIGHT);
					rotations.Remove(AirShipRotation.TWO_RIGHT_ONE);
					rotations.Remove(AirShipRotation.TWO_LEFT_ONE);
				}
				if (!selected.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE).isAvailable) {
					rotations.Remove(AirShipRotation.THREE_DOWN);
					rotations.Remove(AirShipRotation.TWO_DOWN_ONE);
					rotations.Remove(AirShipRotation.TWO_UP_ONE);
				}
				if (!selected.GetNeighbor(field, Neighbors.MIDDLE_LEFT).isAvailable) {
					rotations.Remove(AirShipRotation.THREE_LEFT);
					rotations.Remove(AirShipRotation.TWO_RIGHT_ONE);
					rotations.Remove(AirShipRotation.TWO_LEFT_ONE);
				}

				if (!selected.GetNeighbor(field, Neighbors.TOP_MIDDLE).GetNeighbor(field, Neighbors.TOP_MIDDLE).isAvailable) {
					rotations.Remove(AirShipRotation.THREE_UP);
					rotations.Remove(AirShipRotation.TWO_UP_ONE);
				}
				if (!selected.GetNeighbor(field, Neighbors.MIDDLE_RIGHT).GetNeighbor(field, Neighbors.MIDDLE_RIGHT).isAvailable) {
					rotations.Remove(AirShipRotation.THREE_RIGHT);
					rotations.Remove(AirShipRotation.TWO_RIGHT_ONE);
				}
				if (!selected.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE).GetNeighbor(field, Neighbors.BOTTOM_MIDDLE).isAvailable) {
					rotations.Remove(AirShipRotation.THREE_DOWN);
					rotations.Remove(AirShipRotation.TWO_DOWN_ONE);
				}
				if (!selected.GetNeighbor(field, Neighbors.MIDDLE_LEFT).GetNeighbor(field, Neighbors.MIDDLE_LEFT).isAvailable) {
					rotations.Remove(AirShipRotation.THREE_LEFT);
					rotations.Remove(AirShipRotation.TWO_LEFT_ONE);
				}

				if (!selected.GetNeighbor(field, Neighbors.TOP_MIDDLE).GetNeighbor(field, Neighbors.TOP_MIDDLE).GetNeighbor(field, Neighbors.TOP_MIDDLE).isAvailable) {
					rotations.Remove(AirShipRotation.THREE_UP);
				}
				if (!selected.GetNeighbor(field, Neighbors.MIDDLE_RIGHT).GetNeighbor(field, Neighbors.MIDDLE_RIGHT).GetNeighbor(field, Neighbors.MIDDLE_RIGHT).isAvailable) {
					rotations.Remove(AirShipRotation.THREE_RIGHT);
				}
				if (!selected.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE).GetNeighbor(field, Neighbors.BOTTOM_MIDDLE).GetNeighbor(field, Neighbors.BOTTOM_MIDDLE).isAvailable) {
					rotations.Remove(AirShipRotation.THREE_DOWN);
				}
				if (!selected.GetNeighbor(field, Neighbors.MIDDLE_LEFT).GetNeighbor(field, Neighbors.MIDDLE_LEFT).GetNeighbor(field, Neighbors.MIDDLE_LEFT).isAvailable) {
					rotations.Remove(AirShipRotation.THREE_LEFT);
				}
				#endregion

				int selectedRot = UnityEngine.Random.Range(0, rotations.Count);
				if (rotations.Count > 0) {
					PlaceAir(selected, rotations[selectedRot], true);
					airPlaced++;
					freeLocations.RemoveAll((Location l) => !l.isAvailable);
				}
				counter++;
				//Debug.Log(selected.coordinates + " " + selectedRot);
				if (counter > 100) {
					throw new Exception("Over limit! Air");
				}
			}
			Debug.Log(counter);
			while (data.getWarShips != warPlaced) {
				List<WarShipRotation> rotations = new List<WarShipRotation> {
					WarShipRotation.TWO_UP, WarShipRotation.TWO_RIGHT, WarShipRotation.TWO_DOWN, WarShipRotation.TWO_LEFT,
					WarShipRotation.VERTICAL, WarShipRotation.HORIZONTAL
				};
				int rnd = UnityEngine.Random.Range(0, freeLocations.Count);
				Location selected = freeLocations[rnd];

				#region OneByOne Location checking
				if (!selected.GetNeighbor(field, Neighbors.TOP_MIDDLE).isAvailable) {
					rotations.Remove(WarShipRotation.TWO_UP);
					rotations.Remove(WarShipRotation.VERTICAL);
				}
				if (!selected.GetNeighbor(field, Neighbors.MIDDLE_RIGHT).isAvailable) {
					rotations.Remove(WarShipRotation.TWO_RIGHT);
					rotations.Remove(WarShipRotation.HORIZONTAL);
				}
				if (!selected.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE).isAvailable) {
					rotations.Remove(WarShipRotation.TWO_DOWN);
					rotations.Remove(WarShipRotation.VERTICAL);
				}
				if (!selected.GetNeighbor(field, Neighbors.MIDDLE_LEFT).isAvailable) {
					rotations.Remove(WarShipRotation.TWO_LEFT);
					rotations.Remove(WarShipRotation.HORIZONTAL);
				}
				if (!selected.GetNeighbor(field, Neighbors.TOP_MIDDLE).GetNeighbor(field, Neighbors.TOP_MIDDLE).isAvailable) {
					rotations.Remove(WarShipRotation.TWO_UP);
				}
				if (!selected.GetNeighbor(field, Neighbors.MIDDLE_RIGHT).GetNeighbor(field, Neighbors.MIDDLE_RIGHT).isAvailable) {
					rotations.Remove(WarShipRotation.TWO_RIGHT);
				}
				if (!selected.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE).GetNeighbor(field, Neighbors.BOTTOM_MIDDLE).isAvailable) {
					rotations.Remove(WarShipRotation.TWO_DOWN);
				}
				if (!selected.GetNeighbor(field, Neighbors.MIDDLE_LEFT).GetNeighbor(field, Neighbors.MIDDLE_LEFT).isAvailable) {
					rotations.Remove(WarShipRotation.TWO_LEFT);
				}
				#endregion
				int selectedRot = UnityEngine.Random.Range(0, rotations.Count);
				if (rotations.Count > 0) {
					PlaceWar(selected, rotations[selectedRot], true);
					warPlaced++;
					freeLocations.RemoveAll((Location l) => !l.isAvailable);
				}
				counter++;
				//Debug.Log(selected.coordinates + " " + selectedRot);
				if (counter > 100) {
					throw new Exception("Over limit! War");
				}
			}
			Debug.Log(counter);
			while (data.getCargoShips != cargoPlaced) {
				List<CargoShipRotation> rotations = new List<CargoShipRotation> {
					CargoShipRotation.ONE_UP, CargoShipRotation.ONE_RIGHT, CargoShipRotation.ONE_DOWN, CargoShipRotation.ONE_LEFT
				};
				int rnd = UnityEngine.Random.Range(0, freeLocations.Count);
				Location selected = freeLocations[rnd];

				#region OneByOne Location checking
				if (!selected.GetNeighbor(field, Neighbors.TOP_MIDDLE).isAvailable) {
					rotations.Remove(CargoShipRotation.ONE_UP);
				}
				if (!selected.GetNeighbor(field, Neighbors.MIDDLE_RIGHT).isAvailable) {
					rotations.Remove(CargoShipRotation.ONE_RIGHT);
				}
				if (!selected.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE).isAvailable) {
					rotations.Remove(CargoShipRotation.ONE_DOWN);
				}
				if (!selected.GetNeighbor(field, Neighbors.MIDDLE_LEFT).isAvailable) {
					rotations.Remove(CargoShipRotation.ONE_LEFT);
				}
				#endregion
				int selectedRot = UnityEngine.Random.Range(0, rotations.Count);

				if (rotations.Count > 0) {
					PlaceCargo(selected, rotations[selectedRot], true);
					cargoPlaced++;
					freeLocations.RemoveAll((Location l) => !l.isAvailable);
				}
				counter++;
				//Debug.Log(selected.coordinates + " " + selectedRot);
				if (counter > 100) {
					throw new Exception("Over limit! Cargo");
				}
			}
			Debug.Log(counter);
			while (data.getSubmarines != subsPlaced) {
				int rnd = UnityEngine.Random.Range(0, freeLocations.Count);
				Location selected = freeLocations[rnd];
				if (selected.isAvailable) {
					PlaceSubmarine(selected, true);
					subsPlaced++;
				}
				counter++;
				if (counter > 100) {
					throw new Exception("Over limit! Subs");
				}
			}
			Debug.Log(counter);
		}

		public List<Location> GetAvailableLocations() {
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

			RegisterShip(ShipType.SUBMARINE, newShipLocations.ToArray());
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
				case CargoShipRotation.ONE_UP: {
					Location l = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					l.PlaceShip(ShipType.CARGO);
					newShipLocations.Add(l);
					break;
				}
				case CargoShipRotation.ONE_RIGHT: {
					Location l = baseLocation.GetNeighbor(field, Neighbors.MIDDLE_RIGHT);
					l.PlaceShip(ShipType.CARGO);
					newShipLocations.Add(l);
					break;
				}
				case CargoShipRotation.ONE_DOWN: {
					Location l = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					l.PlaceShip(ShipType.CARGO);
					newShipLocations.Add(l);
					break;
				}
				case CargoShipRotation.ONE_LEFT: {
					Location l = baseLocation.GetNeighbor(field, Neighbors.MIDDLE_LEFT);
					l.PlaceShip(ShipType.CARGO);
					newShipLocations.Add(l);
					break;
				}
				default: {
					throw new Exception("NIY Cargo");
				}
			}
			RegisterShip(ShipType.CARGO, newShipLocations.ToArray());
			FillTokens(newShipLocations);
			if (visualize) {
				field.UpdateVisuals();
			}
		}

		private void PlaceWar(Location baseLocation, WarShipRotation rotation, bool visualize = false) {
			List<Location> newShipLocations = new List<Location>();

			baseLocation.PlaceShip(ShipType.WAR);
			newShipLocations.Add(baseLocation);

			switch (rotation) {
				case WarShipRotation.TWO_UP: {
					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location nd_upper = upper.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					upper.PlaceShip(ShipType.WAR);
					nd_upper.PlaceShip(ShipType.WAR);
					newShipLocations.Add(upper);
					newShipLocations.Add(nd_upper);
					break;
				}
				case WarShipRotation.TWO_RIGHT: {
					Location right = baseLocation.GetNeighbor(field, Neighbors.MIDDLE_RIGHT);
					Location nd_right = right.GetNeighbor(field, Neighbors.MIDDLE_RIGHT);
					right.PlaceShip(ShipType.WAR);
					nd_right.PlaceShip(ShipType.WAR);
					newShipLocations.Add(right);
					newShipLocations.Add(nd_right);
					break;
				}
				case WarShipRotation.TWO_DOWN: {
					Location lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location nd_lower = lower.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					lower.PlaceShip(ShipType.WAR);
					nd_lower.PlaceShip(ShipType.WAR);
					newShipLocations.Add(lower);
					newShipLocations.Add(nd_lower);
					break;
				}
				case WarShipRotation.TWO_LEFT: {
					Location left = baseLocation.GetNeighbor(field, Neighbors.MIDDLE_LEFT);
					Location nd_left = left.GetNeighbor(field, Neighbors.MIDDLE_LEFT);
					left.PlaceShip(ShipType.WAR);
					nd_left.PlaceShip(ShipType.WAR);
					newShipLocations.Add(left);
					newShipLocations.Add(nd_left);
					break;
				}
				case WarShipRotation.VERTICAL: {
					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					upper.PlaceShip(ShipType.WAR);
					lower.PlaceShip(ShipType.WAR);
					newShipLocations.Add(upper);
					newShipLocations.Add(lower);
					break;
				}
				case WarShipRotation.HORIZONTAL: {
					Location right = baseLocation.GetNeighbor(field, Neighbors.MIDDLE_RIGHT);
					Location left = baseLocation.GetNeighbor(field, Neighbors.MIDDLE_LEFT);
					right.PlaceShip(ShipType.WAR);
					left.PlaceShip(ShipType.WAR);
					newShipLocations.Add(right);
					newShipLocations.Add(left);
					break;
				}
				default: {
					throw new Exception("NIY War");
				}
			}
			RegisterShip(ShipType.WAR, newShipLocations.ToArray());
			FillTokens(newShipLocations);
			if (visualize) {
				field.UpdateVisuals();
			}
		}

		private void PlaceAir(Location baseLocation, AirShipRotation rotation, bool visualize = false) {
			List<Location> newShipLocations = new List<Location>();

			baseLocation.PlaceShip(ShipType.AIR);
			newShipLocations.Add(baseLocation);
			Debug.Log(baseLocation.coordinates + " " + rotation);
			switch (rotation) {
				case AirShipRotation.THREE_UP: {
					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location nd_upper = upper.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location rd_upper = nd_upper.GetNeighbor(field, Neighbors.TOP_MIDDLE);

					upper.PlaceShip(ShipType.AIR);
					nd_upper.PlaceShip(ShipType.AIR);
					rd_upper.PlaceShip(ShipType.AIR);

					newShipLocations.Add(upper);
					newShipLocations.Add(nd_upper);
					newShipLocations.Add(rd_upper);
					break;
				}
				case AirShipRotation.THREE_RIGHT: {
					Location right = baseLocation.GetNeighbor(field, Neighbors.MIDDLE_RIGHT);
					Location nd_right = right.GetNeighbor(field, Neighbors.MIDDLE_RIGHT);
					Location rd_right = nd_right.GetNeighbor(field, Neighbors.MIDDLE_RIGHT);

					right.PlaceShip(ShipType.AIR);
					nd_right.PlaceShip(ShipType.AIR);
					rd_right.PlaceShip(ShipType.AIR);

					newShipLocations.Add(right);
					newShipLocations.Add(nd_right);
					newShipLocations.Add(rd_right);
					break;
				}
				case AirShipRotation.THREE_DOWN: {
					Location lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location nd_lower = lower.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location rd_lower = nd_lower.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);

					lower.PlaceShip(ShipType.AIR);
					nd_lower.PlaceShip(ShipType.AIR);
					rd_lower.PlaceShip(ShipType.AIR);

					newShipLocations.Add(lower);
					newShipLocations.Add(nd_lower);
					newShipLocations.Add(rd_lower);
					break;
				}
				case AirShipRotation.THREE_LEFT: {
					Location left = baseLocation.GetNeighbor(field, Neighbors.MIDDLE_LEFT);
					Location nd_left = left.GetNeighbor(field, Neighbors.MIDDLE_LEFT);
					Location rd_left = nd_left.GetNeighbor(field, Neighbors.MIDDLE_LEFT);

					left.PlaceShip(ShipType.AIR);
					nd_left.PlaceShip(ShipType.AIR);
					rd_left.PlaceShip(ShipType.AIR);

					newShipLocations.Add(left);
					newShipLocations.Add(nd_left);
					newShipLocations.Add(rd_left);
					break;
				}
				case AirShipRotation.TWO_UP_ONE: {
					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location nd_upper = upper.GetNeighbor(field, Neighbors.TOP_MIDDLE);
					Location lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);

					upper.PlaceShip(ShipType.AIR);
					nd_upper.PlaceShip(ShipType.AIR);
					lower.PlaceShip(ShipType.AIR);

					newShipLocations.Add(upper);
					newShipLocations.Add(nd_upper);
					newShipLocations.Add(lower);
					break;
				}
				case AirShipRotation.TWO_RIGHT_ONE: {
					Location right = baseLocation.GetNeighbor(field, Neighbors.MIDDLE_RIGHT);
					Location nd_right = right.GetNeighbor(field, Neighbors.MIDDLE_RIGHT);
					Location left = baseLocation.GetNeighbor(field, Neighbors.MIDDLE_LEFT);

					right.PlaceShip(ShipType.AIR);
					nd_right.PlaceShip(ShipType.AIR);
					left.PlaceShip(ShipType.AIR);

					newShipLocations.Add(right);
					newShipLocations.Add(nd_right);
					newShipLocations.Add(left);
					break;

				}
				case AirShipRotation.TWO_DOWN_ONE: {
					Location lower = baseLocation.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location nd_lower = lower.GetNeighbor(field, Neighbors.BOTTOM_MIDDLE);
					Location upper = baseLocation.GetNeighbor(field, Neighbors.TOP_MIDDLE);

					lower.PlaceShip(ShipType.AIR);
					nd_lower.PlaceShip(ShipType.AIR);
					upper.PlaceShip(ShipType.AIR);

					newShipLocations.Add(lower);
					newShipLocations.Add(nd_lower);
					newShipLocations.Add(upper);
					break;

				}
				case AirShipRotation.TWO_LEFT_ONE: {
					Location left = baseLocation.GetNeighbor(field, Neighbors.MIDDLE_LEFT);
					Location nd_left = left.GetNeighbor(field, Neighbors.MIDDLE_LEFT);
					Location right = baseLocation.GetNeighbor(field, Neighbors.MIDDLE_RIGHT);

					left.PlaceShip(ShipType.AIR);
					nd_left.PlaceShip(ShipType.AIR);
					right.PlaceShip(ShipType.AIR);

					newShipLocations.Add(left);
					newShipLocations.Add(nd_left);
					newShipLocations.Add(right);
					break;

				}
				default: {
					throw new Exception("NIY Air");
				}
			}
			RegisterShip(ShipType.AIR, newShipLocations.ToArray());
			FillTokens(newShipLocations);
			if (visualize) {
				field.UpdateVisuals();
			}
		}

		private void PlaceBattlecruier(Location baseLocation, bool visualize = false) {
			List<Location> newShipLocations = new List<Location>();

			baseLocation.PlaceShip(ShipType.BATTLECRUSER);
			Location[] neighbors = baseLocation.getNeighborsOnAxis;
			newShipLocations.Add(baseLocation);

			for (int i = 0; i < neighbors.Length; i++) {
				neighbors[i].PlaceShip(ShipType.BATTLECRUSER);
				newShipLocations.Add(neighbors[i]);
			}
			RegisterShip(ShipType.BATTLECRUSER, newShipLocations.ToArray());
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

		private void RegisterShip(ShipType type, Location[] occupiedSpace) {
			field.getAllShips.Add(new Ship(occupiedSpace, type));
		}

		private bool IsInValidCruiserLocation(Vector2 location) {
			return location.x >= 1 && location.y >= 1 && location.x < field.getDimensions.x - 1 && location.y < field.getDimensions.y - 1;
		}
	}
}