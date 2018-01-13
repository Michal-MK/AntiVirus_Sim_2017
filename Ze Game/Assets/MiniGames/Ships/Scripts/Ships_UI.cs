using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Igor.Constants.Strings;

namespace Igor.Minigames.Ships {

	public class Ships_UI : MonoBehaviour {
		public ShipPrefabs prefabs;

		private static ShipType _selectedShip = ShipType.NONE;
		private static bool _isInAttackMode = false;

		private GameObject shipVisual;
		private Vector3 mouseOffset = new Vector3(20, -20, 10);

		private int currentRotation = 1;
		private bool canRotateShip = true;

		private Dictionary<ShipType, bool> allowRotationForShipType = new Dictionary<ShipType, bool> {
			{ShipType.SUBMARINE,false },
			{ShipType.CARGO, true },
			{ShipType.WAR,true },
			{ShipType.AIR,true },
			{ShipType.BATTLECRUSER,false }
		};

		public void SetSelectedShip(int shipID) {
			Destroy(shipVisual);
			_selectedShip = (ShipType)shipID;
			shipVisual = prefabs.SpawnVisual(_selectedShip);
			shipVisual.GetComponent<ShipPlacement>().StartChecking();
			ShipsMain.script.cursorMode = CursorMode.SHIP_PLACEMENT;
			_isInAttackMode = false;
			currentRotation = 1;
			canRotateShip = allowRotationForShipType[_selectedShip];
		}

		public void SetSelectedShipCustom(GameObject custom, bool canRotate) {
			_selectedShip = ShipType.CUSTOM;
			shipVisual = custom;
			shipVisual.GetComponent<ShipPlacement>().StartChecking();
			ShipsMain.script.cursorMode = CursorMode.SHIP_PLACEMENT;
			_isInAttackMode = false;
			canRotateShip = canRotate;
		}

		public void UnselectVisuals() {
			Destroy(shipVisual);
			shipVisual = null;
			_selectedShip = ShipType.NONE;
			ShipsMain.script.cursorMode = CursorMode.NORMAL;
			Field.self.ClearHighlights();
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
				if (Input.GetAxis(InputNames.MOUSEWHEEL) > 0 && canRotateShip) {
					switch (currentRotation) {
						case 0: {
							shipVisual.transform.rotation = Quaternion.Euler(0, 0, 0);
							currentRotation = 1;
							return;
						}
						case 1: {
							shipVisual.transform.rotation = Quaternion.Euler(0, 0, -90);
							currentRotation = 2;
							return;
						}
						case 2: {
							shipVisual.transform.rotation = Quaternion.Euler(0, 0, -180);
							currentRotation = 3;
							return;
						}
						case 3: {
							shipVisual.transform.rotation = Quaternion.Euler(0, 0, -270);
							currentRotation = 0;
							return;
						}
					}
				}
			}
		}

		public static ShipType selectedShip {
			get { return _selectedShip; }
		}

		public static bool isInAttackMode {
			get { return _isInAttackMode; }
		}
	}
}
