using System;
using UnityEngine;

public class Player_Movement : MonoBehaviour {

	public enum PlayerMovement {
		ARROW,
		FLAPPY,
		TELEPORT,
		MOUSE,
	}

	[Flags]
	public enum PlayerMovementModifiers {
		NONE,
		INVERT,
	}

	public Rigidbody2D body;
	public PlayerMovement overrideMovement;
	public PlayerMovementModifiers overrideModifier;

	public static bool canMove { get; set; } = true;

	public PlayerMovement getCurrentMovementMode { get; private set; } = PlayerMovement.MOUSE;
	public PlayerMovementModifiers getCurrentMovementModifier { get; private set; } = PlayerMovementModifiers.NONE;

	public IPlayerMovement movementMethod;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		PauseUnpause.OnPaused += OnPaused;
		SetMovementMode(getCurrentMovementMode);
		SetMovementModifier(getCurrentMovementModifier);

		overrideMovement = getCurrentMovementMode;
		overrideModifier = getCurrentMovementModifier;
	}

	private void OnPaused(object sender, PauseEventArgs e) {
		canMove = e.isPlaying;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		canMove = true;
		Time.timeScale = 1;
	}

	private void FixedUpdate() {
		if (getCurrentMovementMode != PlayerMovement.FLAPPY && canMove) {
			movementMethod.Move();
		}
		if (overrideModifier != getCurrentMovementModifier) {
			SetMovementModifier(overrideModifier);
			getCurrentMovementModifier = overrideModifier;
		}
		if (overrideMovement != getCurrentMovementMode) {
			SetMovementMode(overrideMovement);
			getCurrentMovementMode = overrideMovement;
		}
	}

	private void Update() {
		if (getCurrentMovementMode == PlayerMovement.FLAPPY && canMove) {
			movementMethod.Move();
		}
		if (Input.GetKeyDown(KeyCode.I)) {
			M_Player.player.isInvincible ^= true;
			Canvas_Renderer.script.DisplayInfo(null, "Invincibility " + (M_Player.player.isInvincible ? "Enabled" : "Disabled"));
		}
	}

	public void SetMovementMode(PlayerMovement type) {
		if (movementMethod != null) {
			movementMethod.Stop();
			Destroy(movementMethod as Component);
		}
		switch (type) {
			case PlayerMovement.FLAPPY: {
				movementMethod = gameObject.AddComponent<FlappyBirdMovement>();
				break;
			}
			case PlayerMovement.ARROW: {
				movementMethod = gameObject.AddComponent<BasicMovement>();
				break;
			}
			case PlayerMovement.MOUSE: {
				movementMethod = gameObject.AddComponent<MouseMovement>();
				break;
			}
			case PlayerMovement.TELEPORT: {
				movementMethod = gameObject.AddComponent<TeleportationMovement>();
				break;
			}
		}
		movementMethod.Setup(body);
		getCurrentMovementMode = type;
		SetMovementModifier(getCurrentMovementModifier);
	}

	public void SetMovementModifier(PlayerMovementModifiers type) {
		movementMethod.movementModifier = type;
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}

