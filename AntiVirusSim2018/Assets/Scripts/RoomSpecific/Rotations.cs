using System;
using System.Collections;
using UnityEngine;

public class Rotations : MonoBehaviour {

	public enum RotationType {
		DEFAULT,
		ROUND_X,
		VERTICAL_WALL,
		HORIZONTAL_WALL,
		Exp,
	}


	public RotationType type = RotationType.DEFAULT;
	public float speed;
	public bool isCounterClockwise;
	public float radius;
	public float angleOffset;
	public bool useRandomSpeed = false;
	public bool useRandomRotationDirection = false;

	private float angle = 0;

	private Transform[] affectedObjects;

	private Func<int, Vector3> positionFunc;

	void Start() {
		affectedObjects = GetObjectsToControl();
		switch (type) {
			case RotationType.DEFAULT: {
				positionFunc = Default;
				break;
			}
			case RotationType.ROUND_X: {
				positionFunc = Round_X;
				break;
			}
			case RotationType.HORIZONTAL_WALL: {
				positionFunc = HorizontalWall;
				break;
			}
			case RotationType.VERTICAL_WALL: {
				positionFunc = VerticalWall;
				break;
			}
			default: {
				positionFunc = Exp;
				break;
			}
		}
		if (useRandomSpeed) {
			StartCoroutine(AlterSpeed());
		}
		if (useRandomRotationDirection) {
			StartCoroutine(AlterRotationDirection());
		}
	}

	private Transform[] GetObjectsToControl() {
		Marker[] affected = GetComponentsInChildren<Marker>();
		Transform[] trans = new Transform[affected.Length];
		for (int i = 0; i < affected.Length; i++) {
			trans[i] = affected[i].transform;
		}
		return trans;
	}

	void FixedUpdate() {
		for (int i = 0; i < affectedObjects.Length; i++) {
			affectedObjects[i].position = positionFunc.Invoke(i);
		}
		if (isCounterClockwise) {
			angle = (angle - speed) % 360;
		}
		else {
			angle = (angle + speed) % 360;
		}
	}

	private IEnumerator AlterSpeed() {
		while (gameObject.activeSelf) {
			yield return new WaitForSeconds(UnityEngine.Random.Range(1, 5));
			speed = Mathf.Clamp(UnityEngine.Random.Range(speed / 4, speed * 2), 0.5f, 8);
		}
	}

	private IEnumerator AlterRotationDirection() {
		while (gameObject.activeSelf) {
			yield return new WaitForSeconds(UnityEngine.Random.Range(8, 15));
			isCounterClockwise = !isCounterClockwise;
		}
	}

	private Vector3 Default(int position) {
		float newX = radius * Mathf.Cos(Mathf.Deg2Rad * (angle + angleOffset * position)) + transform.position.x;
		float newY = radius * Mathf.Sin(Mathf.Deg2Rad * (angle + angleOffset * position)) + transform.position.y;
		return new Vector3(newX, newY);
	}

	private Vector3 Round_X(int position) {
		float newX = radius * Mathf.Cos(Mathf.Deg2Rad * (angle + 45 + angleOffset * position)) + transform.position.x;
		float newY = radius * Mathf.Cos(Mathf.Deg2Rad * (angle + angleOffset * position * 2)) + transform.position.y;
		return new Vector3(newX, newY);
	}

	private Vector3 HorizontalWall(int position) {
		float newX = radius * Mathf.Cos(Mathf.Deg2Rad * (angle + angleOffset * position)) + transform.position.x;
		return new Vector3(newX, 0);
	}

	private Vector3 VerticalWall(int position) {
		float newY = radius * Mathf.Cos(Mathf.Deg2Rad * (angle + angleOffset * position)) + transform.position.y;
		return new Vector3(0, newY);
	}


	public float XRadiusMod = 0;
	public float YRadiusMod = 0;

	public float XAngleMod = 0;
	public float YAngleMod = 0;

	public float XPosOff = 0;
	public float YPosOff = 0;

	public float XEntireFunc = 0;
	public float YEntireFunc = 0;

	private Vector3 Exp(int position) {
		float newX = radius * XRadiusMod * Mathf.Tan(Mathf.Deg2Rad * (angle + XAngleMod + angleOffset * position + 1) + XEntireFunc) + transform.position.x + XPosOff;
		float newY = radius * YRadiusMod * Mathf.Cos(Mathf.Deg2Rad * (angle + YAngleMod + angleOffset * position) + YEntireFunc) + transform.position.y + YPosOff;
		return new Vector3(newX, newY);
	}
}
