using UnityEngine;
using System.Collections;

namespace UnityEngine {
	public class Chance {
		public static bool Half() {
			return Random.value > 0.5f;
		}

		public static bool Quarter() {
			return Random.value > 0.25f;
		}

		public static bool ThreeQuarters() {
			return Random.value > 0.75f;
		}
	
		public static bool Custom(int number, int fromRange) {
			return Random.Range(0, fromRange) == number;
		}
	}
}
