using UnityEngine;
using UnityEditor;

public class Select : Editor {
	[MenuItem("Igor/Selections _#S_P")]
	static void SelectPlayer() {
		Selection.activeGameObject = GameObject.Find("Player");
	}
}