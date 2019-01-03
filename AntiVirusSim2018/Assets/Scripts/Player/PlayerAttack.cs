using System;
using System.Collections;
using UnityEngine;
using Igor.Constants.Strings;

public enum AttackType {
	BULLETS,
	BOMBS,
}

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

	public float bombRechargeDelay { get; set; } = 8f;

	public int bullets { get; set; } = 0;
	public int bombs { get; set; } = 0;

	public bool attackModeIntro;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		M_Player.OnSpikePickup += M_Player_OnSpikePickup;
	}

	#region EventImplementation
	private void M_Player_OnSpikePickup(M_Player sender, GameObject other) {
		bullets++;
		OnAmmoChanged(AttackType.BULLETS, bullets);
		if (Spike.spikesCollected == 5) {
			string text;
			if (attackModeIntro) {
				text = "You found all the bullets.\n You can fire them by switching into \"ShootMode\" (Space) and target using your mouse.\n The bullets are limited, don't lose them!";
				attackModeIntro = false;
			}
			else {
				text = "You found all the bullets.\n You can fire them by... oh, you already know. Well... don't lose them!";
			}
			Canvas_Renderer.script.DisplayInfo(text, "Don't give up now.");
		}
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		attackModeIntro = data.shownHints.shootingIntro;
		bullets = data.player.bullets;
		bombs = data.player.bombs;
	}
	#endregion

	void Update() {
		if (M_Player.gameProgression != -1 && !PauseUnpause.isPaused && Input.GetButtonDown("Attack")) {
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
				Canvas_Renderer.script.DisplayInfo("Wow, you figured out how to shoot ... ok.\n " +
													"Use your mouse to aim.\n " +
													"The bullets are limited and you HAVE to pick them up after you fire!\n" +
													"Currently you have: " + bullets + " bullets.\n " +
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
				if (bullets >= 1) {
					FireSpike();
				}
				else {
					print("Out of bullets!");
				}
			}
			if (Input.GetButtonDown(InputNames.LMB) && ammoType == AttackType.BOMBS) {
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

	private AttackType SwitchAmmoType() {
		if (ammoType == AttackType.BULLETS) {
			OnAmmoChanged(AttackType.BOMBS, bombs);
			return AttackType.BOMBS;
		}
		else {
			OnAmmoChanged(AttackType.BULLETS, bullets);
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

		bullets--;
		OnAmmoChanged(AttackType.BULLETS, bullets);
	}

	public void FireBomb() {
		GameObject firedBomb = Instantiate(bomb);
		firedBomb.transform.position = transform.position + Vector3.down * 2.5f;
		firedBomb.name = ObjNames.BOMB;
		firedBomb.transform.parent = GameObject.Find("Collectibles").transform;
		bombs--;
		OnAmmoChanged(AttackType.BOMBS, bombs);
	}

	public IEnumerator RefreshBombs() {
		Canvas_Renderer.script.DisplayInfo(null, "Wait for the bomb to regenerate!");
		yield return new WaitForSeconds(bombRechargeDelay);
		bombs++;
		OnAmmoChanged(AttackType.BOMBS, bombs);
	}

	public void Collided(Transform collided) {
		if (collided.name == ObjNames.BOMB_PICKUP) {
			Destroy(collided.gameObject);
			bombs++;
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowCollected);
			OnAmmoPickup(AttackType.BOMBS, true);
			Canvas_Renderer.script.DisplayInfo("You found a bomb, it will be useful later on.", null);
		}

		if (collided.name == ObjNames.BULLET_PICKUP) {
			Destroy(collided.gameObject);
			bullets++;
			OnAmmoChanged(AttackType.BULLETS, bullets);
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowCollected);
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		M_Player.OnSpikePickup -= M_Player_OnSpikePickup;
	}
}

