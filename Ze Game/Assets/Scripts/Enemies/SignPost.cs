using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPost : MonoBehaviour {
	public SpriteRenderer sign;
	public GameObject InteractInfo;

	private bool awaitingInput = false;
	private bool interact = true;

	private void OnTriggerEnter2D(Collider2D col) {
		if(col.tag == "Player") {
			awaitingInput = true;
			InteractInfo.SetActive(true);
			
		}
	}
	private void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "Player") {
			awaitingInput = false;
			InteractInfo.SetActive(false);

		}
	}
	private IEnumerator Fade() {
		for (float f = 255; f > 0; f -= 1) {
			sign.color = new Color32(255, 255, 255, (byte)f);
			if(f > 0) {
				yield return null;
			}
			else {
				sign.color = new Color(1, 1, 1, 0);
				Destroy(gameObject);
				break;
			}
		}
	}

	private void Update() {
		if (awaitingInput) {
			if (Input.GetButtonDown("Interact")) {
				if (interact) {
					Statics.avoidance.StartAvoidance();
					Statics.avoidance.preformed = true;
					StartCoroutine(Fade());
					interact = false;
					if (Statics.avoidance.displayAvoidInfo) {
						Statics.canvasRenderer.infoRenderer("MuHAhAHAHAHAHAHAHAHAHAHAAAAA!\n" +
															"You fell for my genious trap, now... DIE!", "Survive, You can zoom out using the Mousewheel");
						Statics.avoidance.displayAvoidInfo = false;
					}
				}
			}
		}
	}
}
