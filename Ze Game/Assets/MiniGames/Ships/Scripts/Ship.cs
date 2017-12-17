using UnityEngine;
using System.Collections.Generic;


namespace Igor.Minigames.Ships {
	public class Ship {
		private Location[] position;
		private int hp;

		public Ship(Location[] position, ShipType type) {
			this.position = position;
			hp = (int)type;
		}
		
		public void RemoveFromEditor() {
			foreach (Location location in position) {
				location.RemoveShip();
				foreach (Location neighbor in location.getNeighborsOnAxis) {
					neighbor.RemoveShip();
				}
			}
		}

		public Location[] getLocation {
			get { return position; }
		}

		/// <summary>
		/// Damages the ship, returns true if the ship sunk
		/// </summary>
		/// <returns>Is the ship dead?</returns>
		public bool Damage() {
			hp--;
			return hp == 0;
		}
	}
}
