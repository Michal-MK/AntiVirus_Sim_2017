using System;
using System.Collections;
using UnityEngine;

public static class LerpFunctions {

	public static IEnumerator LerpPosition(GameObject obj, Vector3 end, float stepFreq, Action finished) {

		float sX = obj.transform.position.x;
		float sY = obj.transform.position.y;
		float eX = end.x;
		float eY = end.y;

		for (float t = stepFreq; t < 1; t += stepFreq) {
			float newX;
			float newY;

			newX = Mathf.Lerp(sX, eX, t);
			newY = Mathf.Lerp(sY, eY, t);

			obj.transform.position = new Vector3(newX, newY, obj.transform.position.z);
			yield return null;
		}
		obj.transform.position = end;
		if (finished != null) {
			finished.Invoke();
		}
	}

	public static IEnumerator SmoothStepPosition(GameObject obj, Vector3 start, Vector3 end, float stepFreq, Action finished) {

		float sX = start.x;
		float sY = start.y;
		float eX = end.x;
		float eY = end.y;

		for (float t = stepFreq; t < 1; t += stepFreq) {
			float newX = Mathf.SmoothStep(sX, eX, t);
			float newY = Mathf.SmoothStep(sY, eY, t);

			obj.transform.position = new Vector3(newX, newY, start.z);
			yield return null;
		}
		obj.transform.position = end;
		if (finished != null) {
			finished.Invoke();
		}
	}
}

