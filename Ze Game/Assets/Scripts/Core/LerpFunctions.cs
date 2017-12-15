using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LerpFunctions {

	public IEnumerator LerpPosition(GameObject obj, Vector3 start, Vector3 end, float stepFreq) {

		float sX = start.x;
		float sY = start.y;
		float eX = end.x;
		float eY = end.y;

		for (float t = 0; t < 1; t += stepFreq) {
			float newX;
			float newY;

			newX = Mathf.Lerp(sX, eX, t);
			newY = Mathf.Lerp(sY, eY, t);

			obj.transform.position = new Vector3(newX, newY, start.z);
			yield return null;
		}
	}
	public IEnumerator SmoothStepPosition(GameObject obj, Vector3 start, Vector3 end, float stepFreq) {

		float sX = start.x;
		float sY = start.y;
		float eX = end.x;
		float eY = end.y;

		for (float t = 0; t < 1; t += stepFreq) {
			float newX = Mathf.SmoothStep(sX, eX, t);
			float newY = Mathf.SmoothStep(sY, eY, t);

			obj.transform.position = new Vector3(newX, newY, start.z);
			yield return null;
		}
	}
}

