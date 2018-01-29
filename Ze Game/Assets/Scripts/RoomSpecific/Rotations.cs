using UnityEngine;
using System.Collections.Generic;

public class Rotations : MonoBehaviour {

	public float speed;
	public bool isCouterCloclwise;
	public float radius;
	public bool useCustomSpacing = false;

	public float customIncrement;
	private float increment = 0;
	private float angle = 0;

	private Transform[] affectedObjects;

	// Use this for initialization
	void Start() {
		Marker[] affected = GetComponentsInChildren<Marker>();
		affectedObjects = new Transform[affected.Length];
		for (int i = 0; i < affected.Length; i++) {
			affectedObjects[i] = affected[i].transform;
		}
		increment = 360 / affectedObjects.Length;
		if (useCustomSpacing) {
			increment = customIncrement;
		}
	}

	// Update is called once per frame
	void FixedUpdate() {
		foreach (Transform t in affectedObjects) {
			float newX = radius * Mathf.Cos(Mathf.Deg2Rad * angle) + transform.position.x;
			float newY = radius * Mathf.Sin(Mathf.Deg2Rad * angle) + transform.position.y;

			t.position = new Vector3(newX, newY);

			angle += increment;
		}
		if (isCouterCloclwise) {
			angle = (angle - speed) % 360;
		}
		else {
			angle = (angle + speed) % 360;
		}
	}
}
