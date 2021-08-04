using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody2D body;
	public PlayerMovementType overrideMovement;
	public PlayerMovementModifiers overrideModifier;

	public static bool CanMove { get; set; } = true;
	public static float SpeedMultiplier { get; set; } = 1;

	public PlayerMovementType CurrentMovementMode { get; private set; }
	public PlayerMovementModifiers CurrentMovementModifier { get; private set; }

	public IPlayerMovement movementMethod;

	public bool overrideMovementMethod = false;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		PauseUnpause.OnPaused += OnPaused;
		SetMovementMode(CurrentMovementMode);
		SetMovementModifier(CurrentMovementModifier);
	}

	private void OnPaused(object sender, PauseEventArgs e) {
		CanMove = e.IsPlaying;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		CanMove = true;
		Time.timeScale = 1;
	}

	private void FixedUpdate() {
		if (CurrentMovementMode != PlayerMovementType.FLAPPY && CanMove) {
			movementMethod.Move();
		}
		if (overrideMovementMethod) {
			if (overrideModifier != CurrentMovementModifier) {
				SetMovementModifier(overrideModifier);
				CurrentMovementModifier = overrideModifier;
			}
			if (overrideMovement != CurrentMovementMode) {
				SetMovementMode(overrideMovement);
				CurrentMovementMode = overrideMovement;
			}
		}
	}

	private void Update() {
		if (CurrentMovementMode == PlayerMovementType.FLAPPY && CanMove) {
			movementMethod.Move();
		}
		if (Input.GetKeyDown(KeyCode.I)) {
			Player.Instance.isInvincible ^= true;
			HUDisplay.Instance.DisplayInfo(null, "Invincibility " + (Player.Instance.isInvincible ? "Enabled" : "Disabled"));
		}
	}

	public void SetMovementMode(PlayerMovementType type) {
		if (movementMethod != null) {
			movementMethod.Stop();
			Destroy(movementMethod as Component);
		}
		switch (type) {
			case PlayerMovementType.FLAPPY: {
				movementMethod = gameObject.AddComponent<FlappyBirdMovement>();
				break;
			}
			case PlayerMovementType.ARROW: {
				movementMethod = gameObject.AddComponent<BasicMovement>();
				break;
			}
			case PlayerMovementType.MOUSE: {
				movementMethod = gameObject.AddComponent<MouseMovement>();
				break;
			}
			case PlayerMovementType.TELEPORT: {
				movementMethod = gameObject.AddComponent<TeleportationMovement>();
				break;
			}
		}
		movementMethod.Setup(body);
		CurrentMovementMode = type;
		SetMovementModifier(CurrentMovementModifier);
	}

	public void SetMovementModifier(PlayerMovementModifiers type) {
		movementMethod.MovementModifier = type;
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		PauseUnpause.OnPaused -= OnPaused;
	}
}

