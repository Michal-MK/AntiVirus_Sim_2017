using System.IO;
using System.Collections.Generic;

namespace Igor.Minigames.Ships {
	public class ShipEdit : Ship {

		private static ShipEdit current;

		public List<Location> locations = new List<Location>();


		public ShipEdit(ShipType type) : base(type) {
			current = this;
		}

		public void SaveShip(string shipName, int shipHP, bool allowRoataion) {
			if (!Directory.Exists(UnityEngine.Application.dataPath + Path.DirectorySeparatorChar + "Ships")) {
				Directory.CreateDirectory(UnityEngine.Application.dataPath + Path.DirectorySeparatorChar + "Ships");
			}
			position = locations.ToArray();
			using (StreamWriter stream = File.CreateText(UnityEngine.Application.dataPath + Path.DirectorySeparatorChar + "Ships" + Path.DirectorySeparatorChar + "Ship_" + shipName.ToLower() + ".txt")) {
				stream.WriteLine(shipName);
				stream.WriteLine(shipHP);
				if (allowRoataion) {
					stream.WriteLine("true");
				}
				else {
					stream.WriteLine("false");
				}
				for (int i = 8; i >= 0; i--) {
					for (int j = 0; j < 9; j++) {
						bool isShip = false;
						for (int k = 0; k < getLocation.Length; k++) {
							if (getLocation[k].coordinates.x == j && getLocation[k].coordinates.y == i) {
								isShip = true;
							}
						}
						if (isShip) {
							stream.Write("#");
						}
						else {
							stream.Write("_");
						}
					}
					stream.WriteLine();
				}
			}
		}

		public static ShipEdit getCurrentShip {
			get { return current; }
		}
	}
}
