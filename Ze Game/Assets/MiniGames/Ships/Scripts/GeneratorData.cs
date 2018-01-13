using System.IO;
using UnityEngine;

namespace Igor.Minigames.Ships {
	public class GeneratorData {

		private int _submarines;
		private int _cargoShips;
		private int _warShips;
		private int _airShips;
		private int _battleCruisers;

		public GeneratorData() {
			_submarines = 2;
			_cargoShips = 1;
			_warShips = 2;
			_airShips = 1;
			_battleCruisers = 20;
		}

		public GeneratorData(string filePath) {
			using (StreamReader s = File.OpenText(filePath)) {
				Debug.Log("HAha");
;			}
		}

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
