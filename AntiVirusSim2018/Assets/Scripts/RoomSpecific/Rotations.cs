using System.Collections;
using UnityEngine;

public class Rotations : MonoBehaviour {

	public float speed;
	public bool isCouterCloclwise;
	public float radius;
	public bool useCustomSpacing = false;
	public float customIncrement;
	public float angleOffset;
	public bool useRandomSpeed = false;
	public bool useRandomRotationDirection = false;

	private float angleObjectOffset = 0;
	private float angle = 0;

	private Transform[] affectedObjects;

	void Start() {
		angle += angleOffset;
		Marker[] affected = GetComponentsInChildren<Marker>();
		affectedObjects = new Transform[affected.Length];
		for (int i = 0; i < affected.Length; i++) {
			affectedObjects[i] = affected[i].transform;
		}
		angleObjectOffset = 360 / affectedObjects.Length;
		if (useCustomSpacing) {
			angleObjectOffset = customIncrement;
		}
		if (useRandomSpeed) {
			StartCoroutine(AlterSpeed());
		}
		if (useRandomRotationDirection) {
			StartCoroutine(AlterRotationDirection());
		}
	}

	void FixedUpdate() {
		foreach (Transform t in affectedObjects) {
			float newX = radius * Mathf.Cos(Mathf.Deg2Rad * angle) + transform.position.x;
			float newY = radius * Mathf.Sin(Mathf.Deg2Rad * angle) + transform.position.y;

			t.position = new Vector3(newX, newY);

			angle += angleObjectOffset;
		}
		if (isCouterCloclwise) {
			angle = (angle - speed) % 360;
		}
		else {
			angle = (angle + speed) % 360;
		}
	}

	private IEnumerator AlterSpeed() {
		while (gameObject.activeSelf) {
			yield return new WaitForSeconds(Random.Range(1, 5));
			speed = Mathf.Clamp(Random.Range(speed / 4, speed * 2), 0.5f, 8);
		}
	}

	private IEnumerator AlterRotationDirection() {
		while (gameObject.activeSelf) {
			yield return new WaitForSeconds(Random.Range(8, 15));
			isCouterCloclwise = !isCouterCloclwise;
		}
	}
}
