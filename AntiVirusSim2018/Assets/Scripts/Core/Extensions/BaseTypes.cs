using System;
using System.Collections.Generic;

public static class BaseTypes {

	public static int Int(this string s) {
		return int.Parse(s);
	}

	public static void AddRange<T>(this HashSet<T> set, IList<T> collection) {
		for (int i = 0; i < collection.Count; i++) {
			set.Add(collection[i]);
		}
	}
	

	public static T SelectUnique<T>(this T[] array, Func<T, bool> func) {
		for (int i = 0; i < array.Length; i++) {
			if (func(array[i])) {
				return array[i];
			}
		}
		throw new Exception("No element in the collection satisfied the predicate!");
	}
}
