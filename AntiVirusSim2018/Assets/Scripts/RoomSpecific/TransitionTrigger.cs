using UnityEngine;

public class TransitionTrigger : MonoBehaviour {

	public GameObject reactToObject;
	public bool isPlayer = true;
	public Player_Movement.PlayerMovement modeToApply;

	private Player_Movement.PlayerMovement modeToRestore;

	private void Start() {
		if (isPlayer) {
			reactToObject = M_Player.player.gameObject;
		}
	}

	private void OnTriggerEnter2D(Collider2D c) {
		if (c.name == reactToObject.name) {
			if (isPlayer) {
				modeToRestore = c.GetComponent<M_Player>().pMovement.getCurrentMovementMode;
				c.GetComponent<M_Player>().pMovement.SetMovementMode(modeToApply);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D c) {
		if (c.name == reactToObject.name) {
			if (isPlayer) {
				c.GetComponent<M_Player>().pMovement.SetMovementMode(modeToRestore);
			}
		}
	}
}

