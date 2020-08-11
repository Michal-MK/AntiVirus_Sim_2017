using System;
using System.Collections;
using UnityEngine;

public class FlappyBirdMovement : MonoBehaviour, IPlayerMovement {
	public PlayerMovementType MovementType => PlayerMovementType.FLAPPY;

	public PlayerMovementModifiers MovementModifier { get; set; }
	public float MovementSpeed { get; set; } = 0; 

	private bool canFlapAgain = true;
	private Rigidbody2D body;

	public float flappyGravity = 12;
	public float flappyForceScale = 60;

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