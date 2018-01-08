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


		private bool spawnedCustomShipWindow = false;
		private GameObject customs;

		private Vector3[] currentCustomLocations;

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
			throw new System.Exception("No Ship");
		}

		public GameObject SpawnCustomVisual(Vector3[] partLocations) {
			GameObject holder = new GameObject("Holder");
			ShipPlacement place = holder.AddComponent<ShipPlacement>();
			for (int i = 0; i < partLocations.Length; i++) {
				Instantiate(simpleRaycastObj, partLocations[i] / 50 - new Vector3(17, 6.5f), Quaternion.identity, holder.transform);
			}
			holder.transform.position = Input.mousePosition;
			currentCustomLocations = partLocations;
			return holder;
		}

		public void SpawnCustomShipWindow() {
			if (spawnedCustomShipWindow == false) {
				customs = Instantiate(customsPrefab, GameObject.Find("Canvas").transform);
				customs.name = "Customs";
				customs.GetComponent<CustomShips>().ship_UI = GameObject.Find("Canvas").GetComponent<Ships_UI>();
			}
			else {
				Destroy(customs);
				customs = null;
			}
			spawnedCustomShipWindow = !spawnedCustomShipWindow;

		}
	}
}
