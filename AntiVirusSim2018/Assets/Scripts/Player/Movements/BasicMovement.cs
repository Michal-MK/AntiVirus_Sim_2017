using UnityEngine;
using static Player_Movement;

public class BasicMovement : MonoBehaviour, IPlayerMovement {

	public PlayerMovement movementType => PlayerMovement.ARROW;

	private PlayerMovementModifiers _modifiers = PlayerMovementModifiers.NONE;

	public PlayerMovementModifiers movementModifier {
		get { return _modifiers; }
		set {
			switch (value) {
				case PlayerMovementModifiers.INVERT: {
					print("Inverting.");
					movementSpeed = -movementSpeed;
					_modifiers = value;
					break;
				}
			}
		}
	}

	public float movementSpeed { get; set; } = 50;
	public float movementDrag = 30;

	private Rigidbody2D body;

	public void Setup(Rigidbody2D body) {
		this.body = body;
		body.gravityScale = 0;
		body.drag = movementDrag;
	}

	public void Move() {
		if (Input.GetAxis("VertMovement") != 0) {
			body.AddForce(new Vector2(0, movementSpeed * Input.GetAxis("VertMovement")) * playerMovementSpeedMultiplier);
		}
		if (Input.GetAxis("HorMovement") != 0) {
			body.AddForce(new Vector2(Input.GetAxis("HorMovement") * movementSpeed, 0) * playerMovementSpeedMultiplier);
		}
	}

	public void Stop() {
		//Nothing to cleanup
	}
}
