using UnityEngine;

public interface IPlayerMovement {
	void Setup(Rigidbody2D body);
	void Move();
	void Stop();

	PlayerMovementType MovementType { get; }
	PlayerMovementModifiers MovementModifier { get; set; }

	float MovementSpeed { get; set; }
}
