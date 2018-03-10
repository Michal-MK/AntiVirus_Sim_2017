using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Igor.Constants.Strings;

namespace Igor.Minigames.Ships {

	public class Ships_UI : MonoBehaviour {

		public enum ViewingField {
			PLAYER,
			OPPONENT,
		}

		public ShipPrefabs prefabs;

		private static ViewingField _currentField  = ViewingField.PLAYER;
		private static ShipType _selectedShip = ShipType.NONE;
		private static bool _isInAttackMode = false;

		private GameObject shipVisual;
		private Vector3 mouseOffset = new Vector3(20, -20, 10);

		private int currentRotation = 1;
		private bool canRotateShip = true;

		public UI_ReferenceHolder refHolder;

		private Dictionary<ShipType, bool> allowRotationForShipType = new Dictionary<ShipType, bool> {
			{ShipType.SUBMARINE,false },
			{ShipType.CARGO, true },
			{ShipType.WAR,true },
			{ShipType.AIR,true },
			{ShipType.BATTLECRUSER,false }
		};

		private void Start() {
			refHolder = GetComponent<UI_ReferenceHolder>();
		}

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
			//Possibly wont work with ai implementation
			ShipsMain.singleplayer.getPlayerField.ClearHighlights();
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
				ShipsMain.singleplayer.PrepareForGame();
			}
			else {
				ShipsMain.script.cursorMode = CursorMode.NORMAL;
			}
		}

		public void PopulateDefaults(int side) {
			if((ViewingField)side == ViewingField.PLAYER) {
				ShipsMain.singleplayer.getPlayerField.PopulateDefault();
			}
			else {
				ShipsMain.singleplayer.getAiField.PopulateDefault();
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
			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
				ShipsMain.singleplayer.getPlayerField.Show(_currentField);
				ShipsMain.singleplayer.getAiField.Show(_currentField);
				_currentField = getCurrentView == ViewingField.PLAYER ? ViewingField.OPPONENT : ViewingField.PLAYER;
				print("Hello");
			}
		}

		public static ShipType selectedShip {
			get { return _selectedShip; }
		}

		public static bool isInAttackMode {
			get { return _isInAttackMode; }
		}

		public static ViewingField getCurrentView {
			get { return _currentField; }
		}
	}
}
