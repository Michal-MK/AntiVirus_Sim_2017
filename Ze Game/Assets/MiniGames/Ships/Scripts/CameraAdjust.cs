using UnityEngine;
using System.Collections;
using Igor.Minigames.Ships;
using System.Linq;

public class CameraAdjust : MonoBehaviour {
	public Camera cam;

	private RectTransform buttonWidthReference;

	private bool isAdjusted = false;

	public void Adjust() {
		isAdjusted = false;

		buttonWidthReference = GameObject.Find("_Submarine").GetComponent<RectTransform>();
		cam.transform.position = new Vector3(Field.self.getDimensions.x / 2 - 0.5f, Field.self.getDimensions.y / 2 - 0.5f, -10);
		cam.orthographicSize = 2;

		Vector3 topRaycast = new Vector3(transform.position.x, cam.transform.position.y + cam.orthographicSize);
		Vector3 rightRaycast = cam.ScreenToWorldPoint(new Vector3(buttonWidthReference.position.x + buttonWidthReference.rect.width / 2, buttonWidthReference.position.y, 0));

		int i = 0;
		while (!isAdjusted) {
			RaycastHit2D[] top = Physics2D.RaycastAll(topRaycast, Vector3.forward);
			RaycastHit2D[] right = Physics2D.RaycastAll(rightRaycast, Vector3.forward);
			bool adjusted = true;
			foreach (RaycastHit2D hit in top.Concat(right)) {
				if (hit.transform.name.StartsWith("(")) {
					cam.orthographicSize += 1;
					topRaycast = RecalculateRaycastVector(Directions.TOP);
					rightRaycast = RecalculateRaycastVector(Directions.RIGHT);
					adjusted = false;
					break;
				}
			}
			if (adjusted) {
				isAdjusted = true;
			}
			i++;
			if (i > 10) {
				throw new System.Exception("It took too long to adjust");
			}
		}
	}

	private Vector3 RecalculateRaycastVector(Directions which) {
		if (which == Directions.TOP) {
			return new Vector3(transform.position.x, cam.transform.position.y + cam.orthographicSize);
		}
		else {
			return cam.ScreenToWorldPoint(new Vector3(buttonWidthReference.position.x + buttonWidthReference.rect.width / 2, buttonWidthReference.position.y, 0));
		}
	}
}
