public class ValueMapping {

	public static float MapFloat(float number, float fromRange, float toRange, float mapFrom, float mapTo) {
		if (number == fromRange) {
			return mapFrom;
		}
		else if (number == toRange) {
			return mapTo;
		}
		else {
			return ((number - fromRange) * (mapTo - mapFrom) / (toRange - fromRange)) + mapFrom;
		}
	}
}