using System.IO;
using UnityEngine;

namespace Igor.Minigames.Ships {
	public class GeneratorData {

		private int _submarines;
		private int _cargoShips;
		private int _warShips;
		private int _airShips;
		private int _battleCruisers;

		/// <summary>
		/// Default generator, makes 2x Sub, 2x Cargo, 2x War, 1x Air and 2x Cruiser
		/// </summary>
		public GeneratorData() {
			_submarines = 2;
			_cargoShips = 2;
			_warShips = 2;
			_airShips = 1;
			_battleCruisers = 2;
		}

		/// <summary>
		/// Custom generator specify how many of shiptype you want
		/// </summary>
		public GeneratorData(int submarines, int cargos, int wars, int airs, int cruisers) {
			_submarines = submarines;
			_cargoShips = cargos;
			_warShips = wars;
			_airShips = airs;
			_battleCruisers = cruisers;
		}

		public int getSubmarines { get { return _submarines; } }
		public int getCargoShips { get { return _cargoShips; } }
		public int getWarShips { get { return _warShips; } }
		public int getAirShips { get { return _airShips; } }
		public int getCruisers { get { return _battleCruisers; } }
	}
}
