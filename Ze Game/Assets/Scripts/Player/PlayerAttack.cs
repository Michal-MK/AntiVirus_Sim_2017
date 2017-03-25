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
	public timer timer;

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

	private void Awake() {
		Statics.playerAttack = this;
	}
	private void Start() {
		bombGUI = GameObject.Find("BombGUI").GetComponent<Image>();
		bulletGUI = GameObject.Find("BulletGUI").GetComponent<Image>();
		currentAmmo = GameObject.Find("CurrentAmmo").GetComponent<Image>();
		if (Statics.mPlayer.newGame) {
			bullets = 0;
			bombs = 0;
		}
	}

	void Update() {

		if (Input.GetButtonDown("Attack")) {
			Cursor.visible = !Cursor.visible;
			fireMode = !fireMode;
			timer.attacking = !timer.attacking;

			if (M_Player.gameProgression != 10 && displayShootingInfo) {
				if (bullets != 0) {
					Statics.canvasRenderer.infoRenderer("Wow, you figured out how to shoot ... ok.\n " +
														"Use your mouse to aim.\n "+
														"The bullets are reusable and you have to pick them up after you fire!\n" +
														"Currently you have: " + bullets + " bullets.\n "+
														"Don't lose them", null);
					displayShootingInfo = false;
				}else {
					Statics.canvasRenderer.infoRenderer("Wow, you figured out how to shoot ... ok.\n" +
														"Use your mouse to aim.\n "+
														"The bullets are reusable and you have to pick them up after you fire!\n " +
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


		if (Input.GetButtonDown("Right Mouse Button")) {
			fireBullets = !fireBullets;
		}
		if (!Statics.pauseUnpause.isPaused) {
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
		if (fireBullets) {
			currentAmmo.sprite = spikeSprite;
		}
		else {
			currentAmmo.sprite = bombSprite;

		}
	}

	public void UpdateStats() {
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
		Statics.sound.PlayFX(Statics.sound.ArrowSound);

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
		Statics.canvasRenderer.infoRenderer(null, "Wait for the bomb to regenerate!");
		yield return new WaitForSecondsRealtime(8);
		bombs++;
		bombCount.text = "x " + bombs;
		StopCoroutine(RefreshBombs());
	}

	private void OnTriggerEnter2D(Collider2D col) {
		if (col.name == "Spike1") {
			Destroy(col.gameObject);
			bullets++;
			bulletCount.text = "x " + bullets;
		}
		if (col.name == "BombPickup") {
			bombGUI.color = visible;
			bombCount.text = "x " + bombs;
		}
	}
	private void OnDestroy() {
		Statics.playerAttack = null;
	}
}

