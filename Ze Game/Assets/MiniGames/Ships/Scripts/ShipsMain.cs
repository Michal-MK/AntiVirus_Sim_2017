using UnityEngine;

namespace Igor.Minigames.Ships {
	public enum Neighbors {
		TOP_LEFT,
		TOP_MIDDLE,
		TOP_RIGHT,
		MIDDLE_LEFT,
		MIDDLE_MIDDLE,
		MIDDLE_RIGHT,
		BOTTOM_LEFT,
		BOTTOM_MIDDLE,
		BOTTOM_RIGHT
	}

	public enum ShipType {
		NONE = -1,
		TOKEN,
		SUBMARINE,
		CARGO,
		WAR,
		AIR,
		BATTLECRUSER,
		CUSTOM
	}

	public enum CursorMode {
		NORMAL,
		SHIP_PLACEMENT,
		SHIP_REMOVE,
		ATTACK_MODE
	}


	public class ShipsMain : MonoBehaviour {
		public Vector2 dimensionsPublic;
		public GameObject locationObj;

		public Texture2D attackCursor;
		public Texture2D deleteCursor;

		private Field field;

		private ShipType type = ShipType.NONE;
		private static CursorMode mode = CursorMode.SHIP_PLACEMENT;
		private Vector2 cursorOffset = new Vector2(300, 300);

		private bool gameStarted = false;

		public static ShipsMain script;

		private GameObject gameOver;

		void Start() {
			script = this;
			Cursor.visible = true;
			mode = CursorMode.NORMAL;
			field = new Field(dimensionsPublic);
			field.Visualize(locationObj);
			gameOver = GameObject.Find("Canvas").transform.Find("Game_Over").gameObject;
			GeneratorData g = new GeneratorData();
			LevelGenerator lg = new LevelGenerator(field, g);
			lg.Generate();
		}

		public void PrepareForGame() {
			if (!gameStarted) {
				foreach (Location location in field.locations) {
					location.locationVisual.Unhighlight();
				}
				gameStarted = true;
			}
		}

		public void GameOver() {
			gameOver.SetActive(true);
		}

		public CursorMode cursorMode {
			get { return mode; }
			set {
				mode = value;
				switch (value) {
					case CursorMode.ATTACK_MODE: {
						Cursor.SetCursor(attackCursor, cursorOffset, UnityEngine.CursorMode.Auto);
						return;
					}
					case CursorMode.SHIP_REMOVE: {
						Cursor.SetCursor(deleteCursor, cursorOffset, UnityEngine.CursorMode.Auto);
						return;
					}
					default: {
						Cursor.SetCursor(null, Vector2.zero, UnityEngine.CursorMode.Auto);
						return;
					}
				}
			}
		}

		private void OnDestroy() {
			script = null;
		}
	}
}