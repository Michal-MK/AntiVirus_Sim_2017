using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour {
	public RectTransform BG;

	Vector3 currentpos;
	Vector3 startingpos;
	Quaternion startingrotation;

	private void Start() {
		startingpos = gameObject.transform.position;
		startingrotation = gameObject.transform.localRotation;

	}

	private void Update() {
		currentpos = gameObject.transform.position;

		if (currentpos.x < BG.position.x + -BG.sizeDelta.x / 2) {
			gameObject.transform.position = startingpos;
			gameObject.transform.rotation = startingrotation;
		}
		else if(currentpos.x > BG.position.x + BG.sizeDelta.x / 2) {
			gameObject.transform.position = startingpos;
			gameObject.transform.rotation = startingrotation;
		}
		else if (currentpos.y < BG.position.y + -BG.sizeDelta.y / 2) {
			gameObject.transform.position = startingpos;
			gameObject.transform.rotation = startingrotation;
		}
		else if (currentpos.y > BG.position.y + BG.sizeDelta.y / 2) {
			gameObject.transform.position = startingpos;
			gameObject.transform.rotation = startingrotation;
		}
	}
}
