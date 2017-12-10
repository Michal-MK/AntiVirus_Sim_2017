using System.Collections;
using UnityEngine;
using Igor.Constants.Strings;

public delegate void BackgroundChanged(RectTransform background, M_Player sender);
public delegate void CoinEvents(M_Player sender);
public delegate void SpikeEvents(M_Player sender);
public delegate void PlayerColision(M_Player sender, GameObject other);
public delegate void PlayerDeath(M_Player sender);

public class M_Player : MonoBehaviour {
	#region PrefabReferences
	public Rigidbody2D rg;
	public SpriteRenderer face;
	public PlayerAttack pAttack;
	public Player_Movement pMovement;
	private Sprite previous;
	#endregion

	public static int gameProgression;
	public static string currentBG_name;

	public bool newGame = true;
	public bool gameOver = false;

	private int attempts;

	public Sprite smile;
	public Sprite happy;
	public Sprite sad;

	public static M_Player player;

	public static event BackgroundChanged OnRoomEnter;
	public static event PlayerColision OnSpikePickup;
	public static event PlayerColision OnCoinPickup;
	public static event PlayerColision OnTargetableObjectCollision;
	public static event Zoom.Zooming OnZoomModeSwitch;
	public static event PlayerDeath OnPlayerDeath;

	public static PlayerState playerState = PlayerState.NORMAL;

	public enum PlayerState {
		NORMAL,
		ATTACKING
	}

	private void Awake() {
		if (player == null) {
			player = this;
		}
		else if (player != this) {
			Destroy(gameObject);
		}
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		transform.position = data.player.playerPos;
		gameProgression = data.player.spikesCollected;
		attempts = data.core.localAttempt;
	}

	void Start() {
		Cursor.lockState = CursorLockMode.Confined;

#if !UNITY_EDITOR
		string name = Control.currProfile.getProfileName;
#endif
		StartCoroutine(DelayIntro());
	}

	private IEnumerator DelayIntro() {
		yield return new WaitForSeconds(1);
		MapData.script.Progress();
		if (newGame) {
			attempts++;
			Canvas_Renderer.script.InfoRenderer("Welcome! \n" +
												"This is your " + attempts + ". attempt to put the virus into a quaratine. \n\n" +
												"This box will appear only when I have something important to say,\n otherwise look for information in the upper left corner, so it is less disruptive. \n"
												, null);

			Control.currAttempt = attempts;
		}

		Canvas_Renderer.script.InfoRenderer(null, "Good luck & Have fun!");
		Player_Movement.canMove = true;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.name == "killerblock") {
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ELShock);
		}
		if (collision.transform.name == "Block") {
			if (!BlockScript.pressurePlateTriggered && OnTargetableObjectCollision != null) {
				OnTargetableObjectCollision(this, collision.gameObject);
			}
		}
		if (collision.transform.tag == "Enemy") {
			if (collision.gameObject.GetComponent<Rigidbody2D>() != null) {
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
				collision.gameObject.GetComponent<Rigidbody2D>().velocity /= 10;
			}
			collision.transform.parent = GameObject.Find("Collectibles").transform;
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ELShock);
			GameOver();
		}
	}

	private void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Enemy") {
			print(col.gameObject.name);
			if (col.gameObject.GetComponent<Rigidbody2D>() != null) {
				col.gameObject.GetComponent<Rigidbody2D>().velocity /= 10;
			}
			col.transform.SetParent(GameObject.Find("Collectibles").transform, false);
			face.sprite = sad;
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ELShock);
			GameOver();

		}
		if (col.transform.tag == "BG") {
			if (OnRoomEnter != null) {
				OnRoomEnter(col.GetComponent<RectTransform>(), this);
			}
			currentBG_name = col.name;
			CameraMovement.script.RaycastForRooms();

			if (col.name == BackgroundNames.BACKGROUND1_2) {
				if (gameProgression == 3) {
					Canvas_Renderer.script.InfoRenderer(null, "Go down even further.");
				}
			}
			if (col.name == BackgroundNames.BACKGROUND_BOSS_ + "1") {
				gameProgression = 10;
			}
		}

		if (col.tag == ObjNames.SPIKE) {
			if (OnSpikePickup != null) {
				OnSpikePickup(this, col.gameObject);
			}
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowCollected);
			MapData.script.Progress();
			face.sprite = happy;
		}
		if (col.name == ObjNames.COIN) {
			face.sprite = happy;
			if (OnCoinPickup != null) {
				OnCoinPickup(this, col.gameObject);
			}
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.CoinCollected);
			Canvas_Renderer.script.UpdateCounters(ObjNames.COIN);
		}

		if (col.name == ObjNames.BOMB_PICKUP) {
			PlayerAttack.bombs++;
			Destroy(col.gameObject);
			Canvas_Renderer.script.InfoRenderer("You found a bomb, it will be useful later on.", null);
		}

		if (col.tag == EnemyNames.ENEMY_TURRET) {
			previous = face.sprite;
			face.sprite = sad;
		}
	}

	private void OnTriggerExit2D(Collider2D col) {
		if (col.tag == EnemyNames.ENEMY_TURRET) {
			face.sprite = previous;
		}
	}

	public void FloorComplete() {
		Player_Movement.canMove = false;
		Cursor.visible = true;
		Timer.PauseTimer();
#if !UNITY_EDITOR
		UploadScore score = new UploadScore();
#endif
	}

	public void GameOver() {
		if (OnPlayerDeath != null) {
			OnPlayerDeath(this);
		}

		if (OnZoomModeSwitch != null) {
			OnZoomModeSwitch(false);
		}

		Player_Movement.canMove = false;
		Cursor.visible = true;
		Timer.PauseTimer();
		Time.timeScale = 0;
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.DIM_CAMERA, 1f);
		MusicHandler.script.FadeMusic();
		gameProgression = -1;
		gameOver = true;

		Destroy(GameObject.Find("Enemies"));
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}
