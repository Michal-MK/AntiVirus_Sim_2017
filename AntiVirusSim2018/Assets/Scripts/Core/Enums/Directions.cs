public enum Directions {
	TOP = 0,
	RIGHT,
	BOTTOM,
	LEFT,
}

namespace Igor.Conversions {

	public static class DirectionHelper {
		private const string TOP = "Top";
		private const string RIGHT = "Right";
		private const string BOTTOM = "Bottom";
		private const string LEFT = "Left";

		public static Directions ToDirection(this string s) {
			switch (s) {
				case TOP: {
					return Directions.TOP;
				}
				case RIGHT: {
					return Directions.RIGHT;
				}
				case BOTTOM: {
					return Directions.BOTTOM;
				}
				case LEFT: {
					return Directions.LEFT;
				}
				default:
				throw new System.NotImplementedException("String not recognized, only correctly spelled strings are awailable");
			}
		}
	}
}