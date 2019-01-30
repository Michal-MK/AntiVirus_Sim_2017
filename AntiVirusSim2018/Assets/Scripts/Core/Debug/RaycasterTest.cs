using UnityEngine;
using System.Collections;
using Igor.Constants.Strings;

public class RaycasterTest : MonoBehaviour {
	public LayerMask mask;
	// Use this for initialization
	void Start() {
		//print(mask.value);
		mask.value = LayerMask.GetMask("Walls"); // works
		//print(mask.value);
		mask.value = LayerMask.NameToLayer("Walls"); // does not work
		//print(mask.value);

		//RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 200, mask.value/*, Layers.layerIndexes[Layers.WALLS]*/);
		//print(hit.transform);
		Debug.DrawRay(transform.position, Vector3.right, Color.green, 1);


		Vector3 point = SpriteOffsets.GetPoint(GetComponent<SpriteRenderer>(), 12, 12);
		Vector3 point1 = SpriteOffsets.GetPoint(GetComponent<SpriteRenderer>(), 12, 88);
		Vector3 point2 = SpriteOffsets.GetPoint(GetComponent<SpriteRenderer>(), 88, 88);
		Vector3 point3 = SpriteOffsets.GetPoint(GetComponent<SpriteRenderer>(), 88, 12);

		GameObject g = new GameObject("TEst0");
		GameObject g1 = new GameObject("TEst1");
		GameObject g2 = new GameObject("TEst2");
		GameObject g3 = new GameObject("TEst3");

		g.transform.position = point;
		g1.transform.position = point1;
		g2.transform.position = point2;
		g3.transform.position = point3;
	}
}
