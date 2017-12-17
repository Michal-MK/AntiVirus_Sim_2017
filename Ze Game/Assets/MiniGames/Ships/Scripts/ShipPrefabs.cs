using UnityEngine;

namespace Igor.Minigames.Ships {
	public class ShipPrefabs : MonoBehaviour {
		public GameObject submarineObj;
		public GameObject cargoObj;
		public GameObject warObj;
		public GameObject airObj;
		public GameObject battlecruiserObj;

		public GameObject cargoObjRotated;
		public GameObject warObjRotated;
		public GameObject airObjRotated;


		public GameObject SpawnVisual(ShipType type, bool rotated = false) {
			//Vector3 mouseOffset = Vector2.one * 10;
			switch (type) {
				case ShipType.NONE: {
					return null;
				}
				case ShipType.SUBMARINE: {
					return Instantiate(submarineObj, Input.mousePosition/* + mouseOffset*/, Quaternion.identity);
				}
				case ShipType.CARGO: {
					if (!rotated) {
						return Instantiate(cargoObj, Input.mousePosition/* + mouseOffset*/, Quaternion.identity);
					}
					else {
						return Instantiate(cargoObjRotated, Input.mousePosition/* + mouseOffset*/, Quaternion.identity);
					}
				}
				case ShipType.WAR: {
					if (!rotated) {
						return Instantiate(warObj, Input.mousePosition/* + mouseOffset*/, Quaternion.identity);
					}
					else {
						return Instantiate(warObjRotated, Input.mousePosition/* + mouseOffset*/, Quaternion.identity);
					}
				}
				case ShipType.AIR: {
					if (!rotated) {
						return Instantiate(airObj, Input.mousePosition/* + mouseOffset*/, Quaternion.identity);
					}
					else {
						return Instantiate(airObjRotated, Input.mousePosition/* + mouseOffset*/, Quaternion.identity);
					}
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
