using UnityEngine;

namespace Igor.Minigames.Ships {
	public class ShipPrefabs : MonoBehaviour {
		public GameObject submarineObj;
		public GameObject cargoObj;
		public GameObject warObj;
		public GameObject airObj;
		public GameObject battlecruiserObj;
		public GameObject simpleRaycastObj;

		public GameObject cargoObjRotated;
		public GameObject warObjRotated;
		public GameObject airObjRotated;

		public GameObject customsPrefab;

		private GameObject customs;

		private const int HALF_SHIP_CREATION_FIELD = 4;

		public GameObject SpawnVisual(ShipType type, bool rotated = false) {
			switch (type) {
				case ShipType.NONE: {
					return null;
				}
				case ShipType.SUBMARINE: {
					return Instantiate(submarineObj, Input.mousePosition, Quaternion.identity);
				}
				case ShipType.CARGO: {
					return Instantiate(cargoObj, Input.mousePosition, Quaternion.identity);
				}
				case ShipType.WAR: {
					return Instantiate(warObj, Input.mousePosition, Quaternion.identity);
				}
				case ShipType.AIR: {
					return Instantiate(airObj, Input.mousePosition, Quaternion.identity);
				}
				case ShipType.BATTLECRUSER: {
					return Instantiate(battlecruiserObj, Input.mousePosition, Quaternion.identity);
				}
			}
			throw new System.Exception("No Ship what ??");
		}

		public GameObject SpawnCustomVisual(string[,] parts) {
			GameObject holder = new GameObject("Holder");
			holder.AddComponent<ShipPlacement>();
			for (int i = 0; i < 81; i++) {
				int first = Mathf.FloorToInt(i / 9);
				int second = i % 9;

				string selected = parts[first, second];
				if (selected == "#") {
					Instantiate(simpleRaycastObj, new Vector3(second - HALF_SHIP_CREATION_FIELD, first - HALF_SHIP_CREATION_FIELD), Quaternion.identity, holder.transform);
				}
			}
			holder.transform.position = Input.mousePosition;
			return holder;
		}

		public void SpawnCustomShipWindow() {
			if (customs == null) {
				customs = Instantiate(customsPrefab, GameObject.Find("Canvas").transform);
				customs.name = "Customs";
			}
			else {
				ClearCustomSpawnWindow();
				SpawnCustomShipWindow();
			}
		}

		public void ClearCustomSpawnWindow() {
			Destroy(customs);
			customs = null;
		}
	}
}
