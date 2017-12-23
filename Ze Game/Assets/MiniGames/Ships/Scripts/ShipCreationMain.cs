using UnityEngine;
using System.Collections;

namespace Igor.Minigames.Ships {
	public class ShipCreationMain : MonoBehaviour {
		private static Field field;
		void Start() {
			field = new Field(9, 9);

		}
	}
}
