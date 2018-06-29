using UnityEngine;
using System.Collections.Generic;


namespace Igor.Minigames.Ships {
	public class Ship {
		protected Location[] position;
		private int hp;
		private ShipType _type;

		public Ship(Location[] position, ShipType type) {
			this.position = position;
			hp = (int)type;
			_type = type;
		}

		/// <summary>
		/// Create a placeholder ship for generation
		/// </summary>
		public Ship(ShipType type) {
			_type = type;
		}

		/// <summary>
		/// Damages the ship, returns true if the ship sunk
		/// </summary>
		public bool Damage() {
			hp--;
			return hp == 0;
		}

		public ShipType getType {
			get { return _type; }
		}

		public Location[] getLocation {
			get { return position; }
		}

		public int hpRemaining {
			get { return hp; }
		}
	}
}
