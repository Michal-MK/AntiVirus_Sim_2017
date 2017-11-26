using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {

	public GameObject spikeBullet;
	public GameObject bomb;
	public GameObject player;
	public GameObject InfoAmmo;
	public Spike spike;
	public Vector2 mousepos;
	public ObjectPooler pool;
	public Timer timer;

	public bool fireMode = false;
	public bool fireBullets = true;
	public bool visibleAlready = false;

	public static int bullets;
	public static int bombs;

	public Sprite spikeSprite;
	public Sprite bombSprite;

	public bool displayShootingInfo = true;

	Color32 visible = new Color32(255, 255, 255, 255);

	Image bombGUI;
	public Text bombCount;

	Image bulletGUI;
	public Text bulletCount;

	Image currentAmmo;

	public GameObject face;
	public GameObject hands;

	public Sprite wHands;
	public Sprite noHands;
	public Sprite attack;
	public Sprite happy;

	public float bombRechargeDelay;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		if (data.shownHints.displayShootInfo) {
			displayShootingInfo = data.shownHints.displayShootInfo;
		}
		else {
			StartCoroutine(UpdateStats());
		}
		bullets = data.player.bullets;
		bombs = data.player.bombs;

	}

	private void Start() {
		bombGUI = GameObject.Find("BombGUI").GetComponent<Image>();
		bulletGUI = GameObject.Find("BulletGUI").GetComponent<Image>();
		currentAmmo = GameObject.Find("CurrentAmmo").GetComponent<Image>();
	}

	void Update() {

		if (Input.GetButtonDown("Attack") && M_Player.gameProgression!= -1) {
			fireMode = !fireMode;

			if (fireMode) {
				face.GetComponent<SpriteRenderer>().sprite = attack;
				hands.GetComponent<SpriteRenderer>().sprite = wHands;
				Cursor.visible = true;
				Timer.StartTimer(2f);
			}
			else {
				face.GetComponent<SpriteRenderer>().sprite = happy;
				hands.GetComponent<SpriteRenderer>().sprite = noHands;
				Cursor.visible = false;
				Timer.StartTimer(1f);
			}
			if (M_Player.gameProgression != 10 && displayShootingInfo) {
				if (bullets != 0) {
					Canvas_Renderer.script.InfoRenderer("Wow, you figured out how to shoot ... ok.\n " +
														"Use your mouse to aim.\n " +
														"The bullets are limited and you HAVE to pick them up after you fire!\n" +
														"Currently you have: " + bullets + " bullets.\n " +
														"Don't lose them", null);
					displayShootingInfo = false;
				}
				else {
					Canvas_Renderer.script.InfoRenderer("Wow, you figured out how to shoot ... ok.\n" +
														"Use your mouse to aim.\n " +
														"The bullets are limited and you HAVE to pick them up after you fire!\n " +
														"Currently you have: " + bullets + " bullets.", null);
					displayShootingInfo = false;
				}
			}
			visibleAlready = true;
			InfoAmmo.GetComponent<Text>().text = "Equiped:";
			bulletGUI.color = visible;
			currentAmmo.color = visible;
			bulletCount.text = "x " + bullets;
			bombGUI.color = visible;
			bombCount.text = "x " + bombs;
		}

		if (fireMode && Input.GetButtonDown("Right Mouse Button")) {
			fireBullets = !fireBullets;
			if (fireBullets) {
				currentAmmo.sprite = spikeSprite;
			}
			else {
				currentAmmo.sprite = bombSprite;
			}
		}

		if (!PauseUnpause.isPaused) {
			if (fireMode) {
				if (Input.GetButtonDown("Left Mouse Button") && fireBullets) {
					if (bullets >= 1) {
						print("Bullets remaining: " + (bullets - 1));
						FireSpike();
					}
					else {
						print("Out of bullets!");
					}
				}
				if (Input.GetButtonDown("Left Mouse Button") && !fireBullets) {
					if (bombs > 0) {
						FireBomb();
						StartCoroutine(RefreshBombs());
					}
					else {
						print("Out of bombs!");
					}
				}
			}
		}
	}

	public IEnumerator UpdateStats() {
		yield return new WaitForEndOfFrame();
		currentAmmo.sprite = spikeSprite;
		visibleAlready = true;
		InfoAmmo.GetComponent<Text>().text = "Equiped:";
		bulletGUI.color = visible;
		currentAmmo.color = visible;
		bulletCount.text = "x " + bullets;
		bombGUI.color = visible;
		bombCount.text = "x " + bombs;
	}

	public void FireSpike() {
		print(Input.GetAxis("AimControllerX"));
		print(Input.GetAxis("AimControllerY"));
		Vector3 playpos = player.transform.position;
		GameObject bullet = pool.GetPool();
		if (Input.GetAxis("AimControllerX") == 0 && Input.GetAxis("AimControllerY") == 0) {
			mousepos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
			bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, (Vector3)mousepos - player.transform.position);
		}
		else {
			bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(Input.GetAxis("AimControllerX"), Input.GetAxis("AimControllerY")));

		}
		bullet.transform.position = player.transform.position - (bullet.transform.rotation * Vector2.down * 2);
		bullet.name = "Bullet";
		bullet.transform.parent = GameObject.Find("Collectibles").transform;
		bullet.SetActive(true);
		SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowSound);

		bullets--;
		bulletCount.text = "x " + bullets;
	}

	public void FireBomb() {
		GameObject firedBomb = Instantiate(bomb);

		bombs--;
		BombScript script = firedBomb.GetComponent<BombScript>();
		Vector3 playpos = player.transform.position;

		firedBomb.transform.position = playpos + Vector3.down * 2.5f;
		firedBomb.name = "Bomb";

		script.primed = true;
		bombCount.text = "x " + bombs;
	}

	public IEnumerator RefreshBombs() {
		Canvas_Renderer.script.InfoRenderer(null, "Wait for the bomb to regenerate!");
		yield return new WaitForSeconds(bombRechargeDelay);
		bombs++;
		bombCount.text = "x " + bombs;
		StopCoroutine(RefreshBombs());
	}

	private void OnTriggerEnter2D(Collider2D col) {
		if (col.name == "Spike1") {
			Destroy(col.gameObject);
			bullets++;
			bulletCount.text = "x " + bullets;
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowCollected);
		}
		if (col.name == "BombPickup") {
			bombGUI.color = visible;
			bombCount.text = "x " + bombs;
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}

