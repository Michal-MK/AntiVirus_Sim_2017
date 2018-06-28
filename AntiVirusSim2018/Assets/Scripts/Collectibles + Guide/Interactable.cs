using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {

	public GameObject triggerInteractionWith;
	public GameObject interactionInfo;

	public bool allowMultipleIntaractions;
	public bool fireBySendingMessage;

	private bool awaitingInput = false;
	private Coroutine routine;
	private bool interacted;


	private void OnTriggerEnter2D(Collider2D col) {
		if (col.name == triggerInteractionWith.name) {
			awaitingInput = true;
			interactionInfo.SetActive(true);
			routine = StartCoroutine(Interact());
			if (allowMultipleIntaractions) {
				interacted = false;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D col) {
		if (col.name == triggerInteractionWith.name) {
			awaitingInput = false;
			interactionInfo.SetActive(false);
			StopCoroutine(routine);
		}
	}

	private IEnumerator Interact() {
		while (awaitingInput) {
			yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
			if (!interacted) {
				if (fireBySendingMessage) {
					SendMessageUpwards("Interact", SendMessageOptions.RequireReceiver);
					interactionInfo.SetActive(false);
					interacted = true;
				}
			}
		}
	}
}
