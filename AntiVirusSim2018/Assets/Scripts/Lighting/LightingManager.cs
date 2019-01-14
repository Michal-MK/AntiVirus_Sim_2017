using UnityEngine;
using System.Collections;

public class LightingManager : MonoBehaviour {
	new public Light light;

	public bool oscilate;
	private float angle = 0;

	void Start() {
		if (oscilate) {
			StartCoroutine(Oscilation());
		}
	}

	private IEnumerator Oscilation() {
		float initalIntesity = light.intensity;
		while (oscilate) {
			light.intensity = initalIntesity + Mathf.Sin(angle) * initalIntesity;
			angle += Mathf.PI / 180 * 0.2f;
			angle = angle % Mathf.PI;

			yield return null;
		}
	}
}
