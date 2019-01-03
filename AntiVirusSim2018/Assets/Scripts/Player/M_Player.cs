using System.Collections;
using UnityEngine;
using Igor.Constants.Strings;

public delegate void BackgroundChanged(M_Player sender, RectTransform current, RectTransform previous);
public delegate void PlayerColision(M_Player sender, GameObject other);
public delegate void SimplePlayerEvent(M_Player sender);

public class M_Player : MonoBehaviour {

	#region PrefabReferences
	public Rigidbody2D rg;
	public SpriteRenderer face;
	public PlayerAttack pAttack;
	public Player_Movement pMovement;
	private Sprite previous;
	#endregion

	private static int _gameProgression;
	private string currentBG_name;

	public bool newGame = true;

	private int attempts;

	public Sprite smile;
	public Sprite happy;
	public Sprite sad;

	public static M_Player player;

	public bool isInvincible = false;

	public static event BackgroundChanged OnRoomEnter;
	public static event PlayerColision OnSpikePickup;
	public static event PlayerColision OnCoinPickup;
	public static event PlayerColision OnTargetableObjectCollision;
	public static event SimplePlayerEvent OnPlayerDeath;

	public static PlayerState playerState = PlayerState.NORMAL;

	public enum PlayerState {
		NORMAL,
		ATTACKING,
		INVERSE,
		INVERSE_ATTACKING
	}

	private void Awake() {
		if (player == null) {
			player = this;
			LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		}
		else if (player != this) {
			Destroy(gameObject);
		}
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		transform.position = data.player.playerPos;
		gameProgression = data.player.gameProgression;
		newGame = false;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}

	private IEnumerator Start() {
		Cursor.lockState = CursorLockMode.Confined;
		currentBG_name = BackgroundNames.BACKGROUND_1;
		yield return new WaitForSeconds(1);
#if UNITY_EDITOR
		if (!Control.script.allowTesting && SaveManager.current == null) {
			UnityEditor.EditorApplication.isPlaying = false;
		}
#endif

		if (newGame) {
			attempts++;
			Canvas_Renderer.script.DisplayInfo("Welcome! \n" +
												"This is your " + attempts + ". attempt to put the virus into a quarantine. \n\n" +
												"This box will appear only when I have something important to say,\n otherwise look for information in the upper left corner, so it is less disruptive. \n"
												, null);
			Control.currAttempt = attempts;
		}
		Canvas_Renderer.script.DisplayInfo(null, "Good luck & Have fun!");
		Player_Movement.canMove = true;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.name == "Block") {
			if (!BlockScript.pressurePlateTriggered && OnTargetableObjectCollision != null) {
				OnTargetableObjectCollision(this, collision.gameObject);
			}
		}
		if (collision.transform.tag == Tags.ENEMY && isInvincible == false) {
			print(collision.transform.name);
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
		if (col.tag == Tags.ENEMY && isInvincible == false) {
			if (col.gameObject.GetComponent<Rigidbody2D>() != null) {
				col.gameObject.GetComponent<Rigidbody2D>().velocity /= 10;
			}
			col.transform.SetParent(null);
			face.sprite = sad;
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ELShock);
			GameOver();

		}
		if (col.transform.tag == Tags.BACKGROUND) {
			if (OnRoomEnter != null) {
				OnRoomEnter(this, col.GetComponent<RectTransform>(), GameObject.Find(currentBG_name).GetComponent<RectTransform>());
			}
			currentBG_name = col.name;
			CameraMovement.script.RaycastForRooms();

			if (col.name == BackgroundNames.BACKGROUND_2) {
				if (gameProgression == 3) {
					Canvas_Renderer.script.DisplayInfo(null, "Go down even further.");
				}
			}
		}

		if (col.name == ObjNames.SPIKE) {
			if (OnSpikePickup != null) {
				OnSpikePickup(this, col.gameObject);
			}
			face.sprite = happy;
		}
		if (col.name == ObjNames.COIN) {
			face.sprite = happy;
			if (OnCoinPickup != null) {
				OnCoinPickup(this, col.gameObject);
			}
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

	public void GameOver() {
		if (OnPlayerDeath != null) {
			OnPlayerDeath(this);
		}

		Zoom.canZoom = false;
		Destroy(GameObject.Find("Enemies"));
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.DIM_CAMERA, 1f);
		MusicHandler.script.FadeMusic();
		gameProgression = -1;
	}

	public RectTransform GetCurrentBackground() {
		if (!string.IsNullOrEmpty(currentBG_name)) {
			return GameObject.Find(currentBG_name).GetComponent<RectTransform>();
		}
		else {
			throw new System.Exception("No background assigned to player!");
		}
	}


	private void OnDestroy() {
		player = null;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}

	public static int gameProgression {
		get { return _gameProgression; }
		set {
			_gameProgression = value;
			print("Setting progression to " + gameProgression);
		}
	}
}
