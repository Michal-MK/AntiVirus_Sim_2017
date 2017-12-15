using UnityEngine;
using System.Collections;

namespace Igor.Minigames.Ships {
	public class ShipPrefabs : MonoBehaviour {
		public GameObject submarineObj;
		public GameObject cargoObj;
		public GameObject warObj;
		public GameObject airObj;
		public GameObject battlecruiserObj;


		public GameObject SpawnVisual(ShipType type) {
			//Vector3 mouseOffset = Vector2.one * 10;
			switch (type) {
				case ShipType.NONE: {
					return null;
				}
				case ShipType.SUBMARINE: {
					return Instantiate(submarineObj, Input.mousePosition/* + mouseOffset*/, Quaternion.identity);
				}
				case ShipType.CARGO: {
					return Instantiate(cargoObj, Input.mousePosition/* + mouseOffset*/, Quaternion.identity);
				}
				case ShipType.WAR: {
					return Instantiate(warObj, Input.mousePosition/* + mouseOffset*/, Quaternion.identity);
				}
				case ShipType.AIR: {
					return Instantiate(airObj, Input.mousePosition/* + mouseOffset*/, Quaternion.identity);
				}
				case ShipType.BATTLECRUSER: {
					return Instantiate(battlecruiserObj, Input.mousePosition/* + mouseOffset*/, Quaternion.identity);
				}
				default: {
					throw new System.Exception("Ship does not exist!");
				}
			}

		}
	}
}
