namespace Delaunay {
	public enum Side {
		Left = 0,
		Right
	}

	public static class SideHelper {
		public static Side Other(this Side source) {
			return source == Side.Left ? Side.Right : Side.Left;
		}
	}
}