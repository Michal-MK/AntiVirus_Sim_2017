using UnityEngine;
using System.Collections.Generic;


namespace Igor.Minigames.Ships {
	public class Ship {
		private Vector2 inArrayPosition;
		private ShipType type;

		public Ship(Vector2 position, ShipType type) {
			inArrayPosition = position;
			this.type = type;
			Shape(type);
		}

		public Location[] Shape(ShipType type) {
			switch (type) {
				case ShipType.SUBMARINE: {
					if (isAvailable) {
						return new Location[1] { Field.self.GetLocation(inArrayPosition) };
					}
					return null;
				}
				case ShipType.CARGO: {
					if (isAvailable) {
						
					}
					return null;
				}
				case ShipType.WAR: {

					return null;
				}
				case ShipType.AIR: {

					return null;
				}
				case ShipType.BATTLECRUSER: {

					return null;
				}
			}
			return null;
		}

		private bool isAvailable {
			get {
				Location myLocation = Field.self.GetLocation(inArrayPosition);
				if (myLocation.placedShip == ShipType.NONE) {
					bool yes = true;
					foreach (Location neighborLocation in myLocation.getNeighborsOnAxis) {
						if (neighborLocation.placedShip != ShipType.NONE && neighborLocation.placedShip != ShipType.TOKEN) {
							yes = false;
						}
					}
					return yes;
				}
				return false;
			}
		}
	}
}
