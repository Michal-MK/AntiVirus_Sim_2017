using UnityEngine;
using UnityEngine.UI;

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


		private static CursorMode mode = CursorMode.SHIP_PLACEMENT;
		private Vector2 cursorOffset = new Vector2(300, 300);

		public static ShipsMain script;
		public static GameplayManagerSP singleplayer;

		void Start() {
			script = this;
			singleplayer = new GameplayManagerSP(new Field(dimensionsPublic), new Field(dimensionsPublic));
			mode = CursorMode.NORMAL;
			Cursor.visible = true;
			Camera.main.GetComponent<CameraAdjust>().Adjust();
		}

		public void GenerateNew() {
			Vector2 dimensions;
			InputField xDimF = GameObject.Find("_XDim").GetComponent<InputField>();
			InputField yDimF = GameObject.Find("_YDim").GetComponent<InputField>();

			int xDim = 10;
			int.TryParse(xDimF.text, out xDim);
			int yDim = 10;
			int.TryParse(yDimF.text, out yDim);

			print(xDim + "  " + yDim);
			if (xDim >= 5 && yDim >= 5 && xDim <= 20 && yDim <= 20) {
				if (singleplayer.fieldObjectsPlayer != null) {
					foreach (GameObject g in singleplayer.fieldObjectsPlayer) {
						Destroy(g);
					}
					singleplayer.fieldObjectsPlayer = new GameObject[xDim * yDim];
				}
				if (singleplayer.fieldObjectsAi != null) {
					foreach (GameObject g in singleplayer.fieldObjectsAi) {
						Destroy(g);
					}
					singleplayer.fieldObjectsAi = new GameObject[xDim * yDim];
				}

				dimensions = new Vector2(xDim, yDim);
				mode = CursorMode.NORMAL;
				singleplayer.RecreateField(dimensions,locationObj);
			}
			else {
				print("Not a valid Input values between 5 and 20 inclusive");
			}
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

		public Vector2 fieldSize {
			get { return dimensionsPublic; }
			set { dimensionsPublic = value; }
		}
	}
}