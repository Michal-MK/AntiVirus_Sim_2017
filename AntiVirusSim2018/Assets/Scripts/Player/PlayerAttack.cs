using System;
using System.Collections;
using UnityEngine;
using Igor.Constants.Strings;

public class PlayerAttack : MonoBehaviour {

	public GameObject spikeBullet;
	public GameObject bomb;

	public bool inFireMode = false;
	private AttackType ammoType;

	public static event HUDElements.HUDAttackUpdates OnAmmoChanged;
	public static event HUDElements.HUDAttackVisibility OnAmmoPickup;

	public SpriteRenderer face;
	public GameObject hands;

	public Sprite attacking;
	public Sprite happy;

	public float BombRechargeDelay { get; set; } = 8f;

	public int Bullets { get; set; } = 0;
	public int Bombs { get; set; } = 0;

	public bool attackModeIntro;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		Player.OnSpikePickup += M_Player_OnSpikePickup;
	}

	#region EventImplementation
	private void M_Player_OnSpikePickup(Player sender, GameObject other) {
		Bullets++;
		OnAmmoChanged(AttackType.BULLETS, Bullets);
		if (other.GetComponent<Spike>().SpikesCollected == 5) {
			string text;
			if (attackModeIntro) {
				text = "You found all the bullets.\nYou can fire them by switching into \"ShootMode\" (Space) and target using your mouse.\nThe bullets are limited, don't lose them!";
				attackModeIntro = false;
			}
			else {
				text = "You found all the bullets.\n You can fire them by... oh, you already know. Well... don't lose them!";
			}
			HUDisplay.script.DisplayInfo(text, "Don't give up now.");
		}
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		attackModeIntro = data.shownHints.shootingIntro;
		Bullets = data.player.bullets;
		Bombs = data.player.bombs;
	}
	#endregion

	void Update() {
		if (Player.GameProgression != -1 && !PauseUnpause.isPaused && Input.GetButtonDown("Attack")) {
			inFireMode = !inFireMode;
			if (inFireMode) {
				face.sprite = attacking;
				hands.SetActive(true);
				Cursor.visible = true;
				Timer.StartTimer(2f);
			}
			else {
				face.sprite = happy;
				hands.SetActive(false);
				Cursor.visible = false;
				Timer.StartTimer(1f);
			}

			if (attackModeIntro) {
				HUDisplay.script.DisplayInfo("Wow, you figured out how to shoot ... ok.\n " +
													"Use your mouse to aim.\n " +
													"The bullets are limited and you HAVE to pick them up after you fire!\n" +
													"Currently you have: " + Bullets + " bullets.\n " +
													"Don't lose them", null);
				attackModeIntro = false;
				ammoType = SwitchAmmoType();
				HUDElements elem = GameObject.Find("_Time and Collectibles").GetComponent<HUDElements>();
				elem.SetVisibility(AttackType.BULLETS, true);
				elem.SetVisibility(AttackType.BOMBS, true);
			}
		}
		if (inFireMode && !PauseUnpause.isPaused) {
			if (Input.GetButtonDown(InputNames.RMB)) {
				ammoType = SwitchAmmoType();
			}
			if (Input.GetButtonDown(InputNames.LMB) && ammoType == AttackType.BULLETS) {
				if (Bullets >= 1) {
					FireSpike();
				}
				else {
					print("Out of bullets!");
				}
			}
			if (Input.GetButtonDown(InputNames.LMB) && ammoType == AttackType.BOMBS) {
				if (Bombs > 0) {
					FireBomb();
					StartCoroutine(RefreshBombs());
				}
				else {
					print("Out of bombs!");
				}
			}
		}
	}

	private AttackType SwitchAmmoType() {
		if (ammoType == AttackType.BULLETS) {
			OnAmmoChanged(AttackType.BOMBS, Bombs);
			return AttackType.BOMBS;
		}
		else {
			OnAmmoChanged(AttackType.BULLETS, Bullets);
			return AttackType.BULLETS;
		}
	}

	public void FireSpike() {
		GameObject bullet = Instantiate(spikeBullet);
		if (Input.GetAxis("AimControllerX") == 0 && Input.GetAxis("AimControllerY") == 0) {
			Vector2 mousepos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
			bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, (Vector3)mousepos - transform.position);
		}
		else {
			bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(Input.GetAxis("AimControllerX"), Input.GetAxis("AimControllerY")));
		}
		bullet.transform.position = transform.position - (bullet.transform.rotation * Vector2.down * 2.5f);
		bullet.name = ObjNames.BULLET;
		bullet.transform.parent = GameObject.Find("Collectibles").transform;
		bullet.SetActive(true);
		bullet.GetComponent<SpikeBullet>().player = this;
		SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowSound);

		Bullets--;
		OnAmmoChanged(AttackType.BULLETS, Bullets);
	}

	public void FireBomb() {
		GameObject firedBomb = Instantiate(bomb);
		firedBomb.transform.position = transform.position + Vector3.down * 2.5f;
		firedBomb.name = ObjNames.BOMB;
		firedBomb.transform.parent = GameObject.Find("Collectibles").transform;
		Bombs--;
		OnAmmoChanged(AttackType.BOMBS, Bombs);
	}

	public IEnumerator RefreshBombs() {
		HUDisplay.script.DisplayInfo(null, "Wait for the bomb to regenerate!");
		yield return new WaitForSeconds(BombRechargeDelay);
		Bombs++;
		OnAmmoChanged(AttackType.BOMBS, Bombs);
	}

	public void Collided(Transform collided) {
		if (collided.name == ObjNames.BOMB_PICKUP) {
			Destroy(collided.gameObject);
			Bombs++;
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowCollected);
			OnAmmoPickup(AttackType.BOMBS, true);
			HUDisplay.script.DisplayInfo("You found a bomb, it will be useful later on.", null);
		}

		if (collided.name == ObjNames.BULLET_PICKUP) {
			Destroy(collided.gameObject);
			Bullets++;
			OnAmmoChanged(AttackType.BULLETS, Bullets);
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowCollected);
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		Player.OnSpikePickup -= M_Player_OnSpikePickup;
	}
}

