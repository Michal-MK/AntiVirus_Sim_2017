using System;
using System.Collections;
using UnityEngine;

public class FlappyBirdMovement : MonoBehaviour, IPlayerMovement {
	public Player_Movement.PlayerMovement movementType => Player_Movement.PlayerMovement.FLAPPY;

	public Player_Movement.PlayerMovementModifiers movementModifier { get; set; } = Player_Movement.PlayerMovementModifiers.NONE;

	private bool canFlapAgain = true;
	private Rigidbody2D body;

	public float flappyGravity = 8;
	public float flappyForceScale = 35;

	public void Setup(Rigidbody2D body) {
		this.body = body;
		body.gravityScale = flappyGravity;
		body.drag = 0;
	}

	public void Move() {
		if (Input.GetAxis("VertMovement") > 0.5f) {
			if (canFlapAgain) {
				body.velocity = new Vector2(0, flappyForceScale);
				canFlapAgain = false;
				StartCoroutine(FlapAgain());
			}
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			body.velocity = new Vector2(0, flappyForceScale);
		}
	}

	private IEnumerator FlapAgain() {
		yield return new WaitUntil(() => Input.GetAxis("VertMovement") <= 0.5f);
		canFlapAgain = true;
	}

	public void Stop() {
		body.gravityScale = 0;
		body.velocity = Vector2.zero;
	}
}