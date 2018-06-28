using System;
using System.Collections;
using UnityEngine;

public class TransitionTrigger : MonoBehaviour {

	public GameObject reactToObject;
	public bool isPlayer = true;
	public Player_Movement.PlayerMovement modeToApply;

	private Player_Movement.PlayerMovement modeToRestore;

	public bool setupPosition;
	public Transform[] setupPositionLocations;


	private void Start() {
		if (isPlayer) {
			reactToObject = M_Player.player.gameObject;
		}
	}

	private void OnTriggerEnter2D(Collider2D c) {
		if (c.name == reactToObject.name) {
			if (isPlayer) {
				M_Player player = c.GetComponent<M_Player>();
				modeToRestore = player.pMovement.getCurrentMovementMode;
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
				M_Player player = c.GetComponent<M_Player>();
				SetMode(player, false, modeToRestore);
				if (setupPosition) {
					data = c;
				}
			}
		}
	}

	private Collider2D data;
	private void SetMode(M_Player player, bool setPosition, Player_Movement.PlayerMovement mode) {
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
			Player_Movement.canMove = false;
			StartCoroutine(LerpFunctions.LerpPosition(player.gameObject, setupPositionLocations[index].position, Time.deltaTime,FinializeSwitch));
			return;
		}
		player.pMovement.SetMovementMode(mode);
	}

	private void FinializeSwitch() {
		data.GetComponent<M_Player>().pMovement.SetMovementMode(modeToApply);
		Player_Movement.canMove = true;
	}
}

