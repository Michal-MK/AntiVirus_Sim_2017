using UnityEngine;

public class TransitionTrigger : MonoBehaviour {

	public GameObject reactToObject;
	public bool isPlayer = true;
	public PlayerMovementType modeToApply;

	private PlayerMovementType modeToRestore;

	public bool setupPosition;
	public Transform[] setupPositionLocations;


	private void Start() {
		if (isPlayer) {
			reactToObject = Player.Instance.gameObject;
		}
	}

	private void OnTriggerEnter2D(Collider2D c) {
		if (c.name == reactToObject.name) {
			if (isPlayer) {
				Player player = c.GetComponent<Player>();
				modeToRestore = player.pMovement.CurrentMovementMode;
				SetMode(player, setupPosition, modeToApply);
				if (setupPosition) {
					data = c;
				}
			}
		}
	}


	private void OnTriggerExit2D(Collider2D c) {
		if (c.name == reactToObject.name) {
			if (isPlayer) {
				Player player = c.GetComponent<Player>();
				SetMode(player, false, modeToRestore);
				if (setupPosition) {
					data = c;
				}
			}
		}
	}

	private Collider2D data;
	private void SetMode(Player player, bool setPosition, PlayerMovementType mode) {
		if (setPosition) {
			float dist = Mathf.Infinity;
			int index = -1;
			for (int i = 0; i < setupPositionLocations.Length; i++) {

				float newD = Vector3.Distance(player.transform.position, setupPositionLocations[i].position);
				if (newD < dist) {
					dist = newD;
					index = i;
				}
			}
			PlayerMovement.CanMove = false;
			StartCoroutine(LerpFunctions.LerpPosition(player.gameObject, setupPositionLocations[index].position, Time.deltaTime,FinializeSwitch));
			return;
		}
		player.pMovement.SetMovementMode(mode);
	}

	private void FinializeSwitch() {
		data.GetComponent<Player>().pMovement.SetMovementMode(modeToApply);
		PlayerMovement.CanMove = true;
	}
}

