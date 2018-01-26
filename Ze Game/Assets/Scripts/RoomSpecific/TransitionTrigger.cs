using UnityEngine;
using System.Collections;

public class TransitionTrigger : MonoBehaviour {

	public GameObject reactToObject;
	public bool isPlayer = true;
	public Player_Movement.PlayerMovement modeToApply;

	public TransitionTrigger main;

	public TransitionTrigger[] exitingTriggers;

	private bool isTriggered = false;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.name == reactToObject.name) {
			if (isPlayer) {
				if (!isTriggered && main == null) {
					collision.GetComponent<M_Player>().pMovement.SetMovementMode(modeToApply);
					isTriggered = true;
					foreach (TransitionTrigger t in exitingTriggers) {
						t.gameObject.SetActive(true);
					}
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.name == reactToObject.name) {
			if (isPlayer) {
				if (!isTriggered) {
					collision.GetComponent<M_Player>().pMovement.SetMovementMode(modeToApply);
					main.isTriggered = false;
					foreach (TransitionTrigger t in main.exitingTriggers) {
						t.gameObject.SetActive(false);
					}
				}
			}
		}
	}
}
