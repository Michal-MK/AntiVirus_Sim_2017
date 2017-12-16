using UnityEngine;
using UnityEngine.UI;

namespace Igor.Minigames.Ships {

	public class Ships_UI : MonoBehaviour {
		public ShipPrefabs prefabs;
		private static ShipType _selectedShip = ShipType.NONE;


		public Button submarine;
		public Button cargo;
		public Button warShip;
		public Button airShip;
		public Button battleCruiser;

		private GameObject shipVisual;

		private Vector3 mouseOffset = new Vector3(20,-20,10);

		public void SetSelectedShip(int shipID) {
			Destroy(shipVisual);
			_selectedShip = (ShipType)shipID;
			shipVisual = prefabs.SpawnVisual(_selectedShip);
			shipVisual.GetComponent<ShipPlacement>().StartChecking();
		}

		private void Update() {
			if (shipVisual != null) {
				shipVisual.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + mouseOffset);
			}
		}

		public static ShipType selectedShip {
			get { return _selectedShip; }
		}
	}
}
