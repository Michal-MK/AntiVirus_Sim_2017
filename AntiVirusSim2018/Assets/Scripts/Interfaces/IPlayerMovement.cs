using UnityEngine;

public interface IPlayerMovement {
	void Setup(Rigidbody2D body);
	void Move();
	void Stop();

	Player_Movement.PlayerMovement movementType { get; }
	Player_Movement.PlayerMovementModifiers movementModifier { get; set; }

	float movementSpeed { get; set; }
}
