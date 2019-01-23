using UnityEngine;
using UnityEditor;

public class Select : Editor {
	[MenuItem("Igor/Selections _#P")]
	static void SelectPlayer() {
		Selection.activeGameObject = GameObject.Find("Player");
	}
}