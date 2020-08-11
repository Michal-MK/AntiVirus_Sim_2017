using Igor.Constants.Strings;
using UnityEngine;

public class BasicMovement : MonoBehaviour, IPlayerMovement {

	public PlayerMovementType MovementType => PlayerMovementType.ARROW;

	private PlayerMovementModifiers modifiers;

	public PlayerMovementModifiers MovementModifier {
		get { return modifiers; }
		set {
			switch (value) {
				case PlayerMovementModifiers.INVERT: {
					print("Inverting.");
					MovementSpeed = -MovementSpeed;
					modifiers = value;
					break;
				}
			}
		}
	}

	public float MovementSpeed { get; set; } = 50;
	public float movementDrag = 30;

	private Rigidbody2D body;

	public void Setup(Rigidbody2D body) {
		this.body = body;
		body.gravityScale = 0;
		body.drag = movementDrag;
	}

	public void Move() {
		if (Input.GetAxis(InputNames.MOVEMENT_VERTICAL) != 0) {
			body.AddForce(new Vector2(0, MovementSpeed * Input.GetAxis(InputNames.MOVEMENT_VERTICAL)) * PlayerMovement.SpeedMultiplier);
		}
		if (Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL) != 0) {
			body.AddForce(new Vector2(Input.GetAxis(InputNames.MOVEMENT_HORIZONTAL) * MovementSpeed, 0) * PlayerMovement.SpeedMultiplier);
		}
	}

	public void Stop() {
		//Nothing to cleanup
	}
}
