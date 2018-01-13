using UnityEngine;

namespace Igor.Minigames.Ships {
	public class ShipsCreationMain : MonoBehaviour {
		public GameObject gridVisual;
		public static ShipsCreationMain script;

		public ShipsCreation_UI ui;

		private ShipEdit _currentCreatedShip = new ShipEdit(ShipType.CUSTOM);

		public enum CursorMode {
			NONE,
			PART_PLACEMENT,
			PART_DELETION
		}
		private static Field field;
		public CursorMode mode = CursorMode.NONE;

		void Start() {
			script = this;
			field = new Field(9, 9);
			field.Visualize(gridVisual);
			field.locations[4, 4].locationVisual.SetSprite(ShipType.TOKEN);
			_currentCreatedShip.locations.Add(field.locations[4, 4]);
		}

		private void OnDestroy() {
			script = null;
		}

		public void SaveShip() {
			_currentCreatedShip.SaveShip(ui.shipName, ui.shipHP, ui.allowShipRotation);
		}
	}
}
