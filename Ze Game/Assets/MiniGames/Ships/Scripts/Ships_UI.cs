using UnityEngine;
using UnityEngine.UI;
using Igor.Constants.Strings;

namespace Igor.Minigames.Ships {

	public class Ships_UI : MonoBehaviour {
		public ShipPrefabs prefabs;
		public Texture2D attackCursor;
		public Texture2D deleteCursor;


		private static ShipType _selectedShip = ShipType.NONE;

		private GameObject shipVisual;

		private bool _isRotated = false;
		private static bool _isInAttackMode = false;

		private Vector3 mouseOffset = new Vector3(20, -20, 10);
		private Vector2 cursorOffset = new Vector2(300, 300);

		public void SetSelectedShip(int shipID) {
			Destroy(shipVisual);
			_selectedShip = (ShipType)shipID;
			_isRotated = false;
			shipVisual = prefabs.SpawnVisual(_selectedShip);
			shipVisual.GetComponent<ShipPlacement>().StartChecking();
			ShipsMain.cursorMode = CursorMode.SHIP_PLACEMENT;
			Cursor.SetCursor(null, Vector2.zero, UnityEngine.CursorMode.Auto);
			_isInAttackMode = false;
		}

		public void DeleteMode() {
			if (ShipsMain.cursorMode != CursorMode.SHIP_REMOVE) {
				ShipsMain.cursorMode = CursorMode.SHIP_REMOVE;
				_isInAttackMode = false;
				Cursor.SetCursor(deleteCursor, cursorOffset, UnityEngine.CursorMode.Auto);
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
				Cursor.SetCursor(attackCursor, cursorOffset, UnityEngine.CursorMode.Auto);
				ShipsMain.cursorMode = CursorMode.ATTACK_MODE;
			}
			else {
				Cursor.SetCursor(null, Vector2.zero, UnityEngine.CursorMode.Auto);
				ShipsMain.cursorMode = CursorMode.NORMAL;
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
