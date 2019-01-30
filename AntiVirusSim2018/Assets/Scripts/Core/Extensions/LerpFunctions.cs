using System;
using System.Collections;
using UnityEngine;

public static class LerpFunctions {

	public static IEnumerator LerpPosition(GameObject obj, Vector3 end, float stepFreq, Action finished = null) {
		yield return Move(obj, end, Time.fixedDeltaTime, Mathf.Lerp);

		if (finished != null) {
			finished.Invoke();
		}
	}

	public static IEnumerator SmoothStepPosition(GameObject obj, Vector3 start, Vector3 end, float stepFreq, Action finished = null) {
		yield return Move(obj, end, Time.fixedDeltaTime, Mathf.SmoothStep);
		if (finished != null) {
			finished.Invoke();
		}
	}


	public static IEnumerator LerpPosition(GameObject obj, Vector3[] positions, float timeToCoverTransition, Action finished = null) {
		foreach (Vector3 end in positions) {
			yield return Move(obj, end, Time.deltaTime, Mathf.Lerp, timeToCoverTransition);
		}
		if (finished != null) {
			finished.Invoke();
		}
	}

	private static IEnumerator Move(GameObject who, Vector3 where, float timeStep, Func<float, float, float, float> func, float timeToCover = 1) {
		float sX = who.transform.position.x;
		float sY = who.transform.position.y;
		float eX = where.x;
		float eY = where.y;
		if (timeToCover != 1) {
			for (float t = 0; t < timeToCover; t += timeStep) {
				float f = ValueMapping.MapFloat(t, 0, timeToCover, 0, 1);
				float newX = func(sX, eX, f);
				float newY = func(sY, eY, f);
				who.transform.position = new Vector3(newX, newY);
				yield return null;
			}
		}
		else {
			for (float t = 0; t < timeToCover; t += timeStep) {
				float newX = func(sX, eX, t);
				float newY = func(sY, eY, t);
				who.transform.position = new Vector3(newX, newY);
				yield return null;
			}
		}
		who.transform.position = where;
	}
}


