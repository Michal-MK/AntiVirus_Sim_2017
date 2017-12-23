using UnityEngine;
using UnityEngine.UI;
using Igor.Constants.Strings;

namespace Igor.Minigames.Ships {

	public class Ships_UI : MonoBehaviour {
		public ShipPrefabs prefabs;
		private static ShipType _selectedShip = ShipType.NONE;

		private GameObject shipVisual;

		private bool _isRotated = false;
		private static bool _isInAttackMode = false;

		private Vector3 mouseOffset = new Vector3(20, -20, 10);

		public void SetSelectedShip(int shipID) {
			Destroy(shipVisual);
			_selectedShip = (ShipType)shipID;
			_isRotated = false;
			shipVisual = prefabs.SpawnVisual(_selectedShip);
			shipVisual.GetComponent<ShipPlacement>().StartChecking();
			ShipsMain.script.cursorMode = CursorMode.SHIP_PLACEMENT;
			_isInAttackMode = false;
		}

		public void DeleteMode() {
			if (ShipsMain.script.cursorMode != CursorMode.SHIP_REMOVE) {
				ShipsMain.script.cursorMode = CursorMode.SHIP_REMOVE;
				_isInAttackMode = false;
				if (shipVisual != null) {
					shipVisual.GetComponent<ShipPlacement>().StopChecking();
					shipVisual = null;
				}
			}
		}


		public void SetAtatckMode() {
			_isInAttackMode = !_isInAttackMode;
			if (_isInAttackMode) {
				if (shipVisual != null) {
					shipVisual.GetComponent<ShipPlacement>().StopChecking();
					Destroy(shipVisual);
				}
				ShipsMain.script.cursorMode = CursorMode.ATTACK_MODE;
			}
			else {
				ShipsMain.script.cursorMode = CursorMode.NORMAL;
			}
		}

		private void Update() {
			if (shipVisual != null) {
				shipVisual.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + mouseOffset);
				if (Input.GetAxis(InputNames.MOUSEWHEEL) != 0) {
					isRotated = !isRotated;
				}
			}
		}

		public static ShipType selectedShip {
			get { return _selectedShip; }
		}

		public bool isRotated {
			get { return _isRotated; }
			set {
				_isRotated = value;
				shipVisual.GetComponent<ShipPlacement>().StopChecking();
				Destroy(shipVisual);
				shipVisual = prefabs.SpawnVisual(_selectedShip, value);
				shipVisual.GetComponent<ShipPlacement>().StartChecking();
				Field.self.ClearHighlights();
			}
		}
		public static bool isInAttackMode {
			get { return _isInAttackMode; }
		}
	}
}
