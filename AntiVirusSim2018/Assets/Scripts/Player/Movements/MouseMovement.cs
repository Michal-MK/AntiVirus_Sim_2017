using UnityEngine;

public class MouseMovement : MonoBehaviour, IPlayerMovement {
	public PlayerMovementType MovementType => PlayerMovementType.MOUSE;

	private PlayerMovementModifiers _modifiers = PlayerMovementModifiers.NONE;

	public PlayerMovementModifiers MovementModifier {
		get { return _modifiers; }
		set {
			switch (value) {
				case PlayerMovementModifiers.INVERT: {
					print("Inverting.");
					MovementSpeed = -MovementSpeed;
					_modifiers = value;
					break;
				}
			}
		}
	}

	public float MovementSpeed { get; set; } = 0.03f;

	public Rigidbody2D body;

	public float movementDrag = 30;

	public void Setup(Rigidbody2D body) {
		this.body = body;
		body.gravityScale = 0;
		body.drag = movementDrag;
	}

	public void Move() {
		if (Input.GetAxis("Mouse X") > 0) {
			body.AddForce(new Vector2(MovementSpeed * Mathf.Abs(Input.GetAxis("Mouse X")) * 2, 0));
		}

		else if (Input.GetAxis("Mouse X") < 0) {
			body.AddForce(new Vector2(-MovementSpeed * Mathf.Abs(Input.GetAxis("Mouse X")) * 2, 0));
		}

		if (Input.GetAxis("Mouse Y") > 0) {
			body.AddForce(new Vector2(0, MovementSpeed * Mathf.Abs(Input.GetAxis("Mouse Y")) * 2));
		}

		else if (Input.GetAxis("Mouse Y") < 0) {
			body.AddForce(new Vector2(0, -MovementSpeed * Mathf.Abs(Input.GetAxis("Mouse Y")) * 2));
		}
	}

	public void Stop() {
		//Nothing to cleanup
	}
}

