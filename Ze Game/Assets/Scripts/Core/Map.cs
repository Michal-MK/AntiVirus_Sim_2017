using UnityEngine;
using System.Collections;

public class Map {

	public static float MapFloat(float number, float fromRange, float toRange, float mapFrom, float mapTo) {
		//if (number < fromRange || number > toRange) {
		//	//Debug.Log("Input number: " + number + " is not from defined range: " + fromRange + " -> " + toRange + " returning " + mapFrom);
		//	return mapFrom;
		//}

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
