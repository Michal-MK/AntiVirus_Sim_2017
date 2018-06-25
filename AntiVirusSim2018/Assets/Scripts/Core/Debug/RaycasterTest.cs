using UnityEngine;
using System.Collections;
using Igor.Constants.Strings;

public class RaycasterTest : MonoBehaviour {
	public LayerMask mask;
	// Use this for initialization
	void Start() {
		print(mask.value);
		mask.value = LayerMask.GetMask("Walls"); // works
		print(mask.value);
		mask.value = LayerMask.NameToLayer("Walls"); // does not work
		print(mask.value);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 200, mask.value/*, Layers.layerIndexes[Layers.WALLS]*/);
		print(hit.transform);
		Debug.DrawRay(transform.position, Vector3.right, Color.green, 1);
	}
}
