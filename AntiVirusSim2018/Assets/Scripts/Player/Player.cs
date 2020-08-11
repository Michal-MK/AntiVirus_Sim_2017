using System;
using System.Collections;
using UnityEngine;
using Igor.Constants.Strings;

public class Player : MonoBehaviour {

	#region PrefabReferences

	public Rigidbody2D rg;
	public SpriteRenderer face;
	public PlayerAttack pAttack;
	public PlayerMovement pMovement;
	private Sprite previous;

	#endregion

	private string currentBG_name;

	public bool newGame = true;

	private int attempts;

	public Sprite smile;
	public Sprite happy;
	public Sprite sad;

	public static Player Instance { get; private set; }

	private static int gameProgression;
	public static int GameProgression {
		get => gameProgression;
		set { gameProgression = value; print($"GameProgression => {value}"); }
	}

	public static PlayerState PlayerState { get; private set; } = PlayerState.NORMAL;


	public bool isInvincible = false;

	public static event BackgroundChangedEventHandler OnRoomEnter;
	public static event PlayerColisionEventHandler OnSpikePickup;
	public static event PlayerColisionEventHandler OnCoinPickup;
	public static event PlayerColisionEventHandler OnTargetableObjectCollision;
	public static event EventHandler<PlayerDeathEventArgs> OnPlayerDeath;


	private void Awake() {
		if (Instance == null) {
			Instance = this;
			LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		}
		else if (Instance != this) {
			Destroy(gameObject);
		}
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		transform.position = data.player.playerPos;
		GameProgression = data.player.gameProgression;
		newGame = false;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}

	private IEnumerator Start() {
		currentBG_name = BackgroundNames.BACKGROUND_1;
		yield return new WaitForSeconds(1);
#if UNITY_EDITOR
		if (!Control.Instance.allowTesting && SaveManager.current == null) {
			UnityEditor.EditorApplication.isPlaying = false;
		}
#endif

		if (newGame) {
			attempts++;
			HUDisplay.script.DisplayInfo("Welcome!\n" +
											  $"This is your {attempts}. attempt to put the virus into a quarantine.\n\n" +
											   "This box will appear only when I have something important to say,\notherwise look for information in the upper left corner, so it is less disruptive."
											   , null);
			Control.currAttempt = attempts;
		}
		HUDisplay.script.DisplayInfo(null, "Good luck & Have fun!");
		PlayerMovement.CanMove = true;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.name == "Block") {
			if (!BlockScript.pressurePlateTriggered && OnTargetableObjectCollision != null) {
				OnTargetableObjectCollision(this, collision.gameObject);
			}
		}
		if (collision.transform.CompareTag(Tags.ENEMY) && !isInvincible) {
			print(collision.transform.name);
			if (collision.gameObject.GetComponent<Rigidbody2D>() != null) {
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
				collision.gameObject.GetComponent<Rigidbody2D>().velocity /= 10;
			}
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ELShock);
			GameOver();
		}
	}

	private void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag(Tags.ENEMY) && !isInvincible) {
			if (col.gameObject.GetComponent<Rigidbody2D>() != null) {
				col.gameObject.GetComponent<Rigidbody2D>().velocity /= 10;
			}
			face.sprite = sad;
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ELShock);
			GameOver();

		}
		if (col.transform.CompareTag(Tags.BACKGROUND)) {
			OnRoomEnter?.Invoke(this, col.GetComponent<RectTransform>(), GameObject.Find(currentBG_name).GetComponent<RectTransform>());
			currentBG_name = col.name;
			CameraMovement.Instance.RaycastForRooms();

			if (col.name == BackgroundNames.BACKGROUND_2) {
				if (GameProgression == 3) {
					HUDisplay.script.DisplayInfo(null, "Go down even further.");
				}
			}
		}

		if (col.CompareTag(ObjNames.SPIKE)) {
			OnSpikePickup?.Invoke(this, col.gameObject);
			face.sprite = happy;
		}
		if (col.CompareTag(ObjNames.COIN)) {
			face.sprite = happy;
			OnCoinPickup?.Invoke(this, col.gameObject);
		}

		if (col.name == EnemyNames.ENEMY_TURRET) {
			previous = face.sprite;
			face.sprite = sad;
		}
	}

	private void OnTriggerExit2D(Collider2D col) {
		if (col.name == EnemyNames.ENEMY_TURRET) {
			face.sprite = previous;
		}
	}

	public void GameOver() {
		OnPlayerDeath?.Invoke(this, new PlayerDeathEventArgs());

		Zoom.CanZoom = false;
		Destroy(GameObject.Find("Enemies"));
		CamFadeOut.Instance.PlayTransition(CameraTransitionModes.DIM_CAMERA, 1f);
		MusicHandler.script.FadeMusic();
		GameProgression = -1;
	}

	public RectTransform GetCurrentBackground() {
		if (!string.IsNullOrEmpty(currentBG_name)) {
			return GameObject.Find(currentBG_name).GetComponent<RectTransform>();
		}
		else {
			throw new Exception("No background assigned to player!");
		}
	}


	private void OnDestroy() {
		Instance = null;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}
