using System;
using System.Collections;
using UnityEngine;
using AttackTypes;
using Igor.Constants.Strings;

namespace AttackTypes {
	public enum AttackType {
		NOTHING,
		BULLETS,
		BOMBS,
	}
}

public class PlayerAttack : MonoBehaviour {

	public GameObject spikeBullet;
	public GameObject bomb;

	public bool inFireMode = false;
	private AttackType ammoType = AttackType.NOTHING;

	public bool visibleAlready = false;
	public bool displayShootingInfo = true;

	public static int bullets;
	public static int bombs;

	public static event HUDElements.HUDAttackUpdates OnAmmoChanged;
	public static event HUDElements.HUDAttackVisibility OnAmmoPickup;

	public SpriteRenderer face;
	public SpriteRenderer hands;

	public Sprite handsShown;
	public Sprite handsHidden;
	public Sprite attacking;
	public Sprite happy;

	public float bombRechargeDelay = 8f;

	private void Awake() {
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		M_Player.OnSpikePickup += M_Player_OnSpikePickup;
		M_Player.OnBombPickup += M_Player_OnBombPickup;
	}

	#region EventImplementation
	private void M_Player_OnSpikePickup(M_Player sender, GameObject other) {
		bullets++;
		if (visibleAlready == true) {
			OnAmmoChanged(AttackType.BULLETS, bullets, true);
		}
		if (Spike.spikesCollected == 5) {
			string text;
			if (displayShootingInfo) {
				text = "You found all the bullets.\n You can fire them by switching into \"ShootMode\" (Space) and target using your mouse.\n The bullets are limited, don't lose them!";
				displayShootingInfo = false;
			}
			else {
				text = "You found all the bullets.\n You can fire them by... oh, you already know. Well... don't lose them!";
			}
			Canvas_Renderer.script.InfoRenderer(text, "Don't give up now.");
		}
	}

	private void M_Player_OnBombPickup(M_Player sender, GameObject other) {
		bombs++;
		Destroy(other);
		OnAmmoPickup(AttackType.BOMBS, true, bombs);
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		if (data.shownHints.displayShootInfo) {
			displayShootingInfo = true;
		}
		else {
			displayShootingInfo = false;
		}
		bullets = data.player.bullets;
		bombs = data.player.bombs;
	}
	#endregion

	void Update() {
		if (Input.GetButtonDown("Attack") && M_Player.gameProgression != -1) {
			inFireMode = !inFireMode;

			if (inFireMode) {
				face.sprite = attacking;
				hands.sprite = handsShown;
				Cursor.visible = true;
				Timer.StartTimer(2f);
			}
			else {
				face.sprite = happy;
				hands.sprite = handsHidden;
				Cursor.visible = false;
				Timer.StartTimer(1f);
			}
			if (displayShootingInfo) {
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
			if (ammoType == AttackType.NOTHING) {
				ammoType = SwitchAmmoType();
				HUDElements.SetVisibility(AttackType.BULLETS, true, Spike.spikesCollected);
				HUDElements.SetVisibility(AttackType.BOMBS, true, bombs);
				visibleAlready = true;
			}
		}
		if (inFireMode && Input.GetButtonDown("Right Mouse Button")) {
			ammoType = SwitchAmmoType();
		}

		if (!PauseUnpause.isPaused && inFireMode) {
			if (Input.GetButtonDown("Left Mouse Button") && ammoType == AttackType.BULLETS) {
				if (bullets >= 1) {
					print("Bullets remaining: " + (bullets - 1));
					FireSpike();
				}
				else {
					print("Out of bullets!");
				}
			}
			if (Input.GetButtonDown("Left Mouse Button") && ammoType == AttackType.BOMBS) {
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
			OnAmmoChanged(AttackType.BOMBS, bombs, true);
			return AttackType.BOMBS;
		}
		else if (ammoType == AttackType.BOMBS) {
			OnAmmoChanged(AttackType.BULLETS, bullets, true);
			return AttackType.BULLETS;
		}
		else {
			//Initial call will result into this
			OnAmmoChanged(AttackType.BULLETS, bullets, true);
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
		bullet.transform.position = transform.position - (bullet.transform.rotation * Vector2.down * 2);
		bullet.name = ObjNames.BULLET;
		bullet.transform.parent = GameObject.Find("Collectibles").transform;
		bullet.SetActive(true);
		SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowSound);

		bullets--;
		OnAmmoChanged(AttackType.BULLETS, bullets, true);
	}

	public void FireBomb() {
		GameObject firedBomb = Instantiate(bomb);
		firedBomb.transform.position = transform.position + Vector3.down * 2.5f;
		firedBomb.name = ObjNames.BOMB;
		firedBomb.transform.parent = GameObject.Find("Collectibles").transform;

		firedBomb.GetComponent<BombScript>().primed = true;

		bombs--;
		OnAmmoChanged(AttackType.BOMBS, bombs, true);
	}

	public IEnumerator RefreshBombs() {
		Canvas_Renderer.script.InfoRenderer(null, "Wait for the bomb to regenerate!");
		yield return new WaitForSeconds(bombRechargeDelay);
		bombs++;
		OnAmmoChanged(AttackType.BOMBS, bombs, true);
	}

	private void OnTriggerEnter2D(Collider2D col) {
		if (col.name == ObjNames.FIRED_BULLET_NAME) {
			Destroy(col.gameObject);
			bullets++;
			OnAmmoChanged(AttackType.BULLETS, bullets, true);
			SoundFXHandler.script.PlayFX(SoundFXHandler.script.ArrowCollected);
		}
	}

	private void OnDestroy() {
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
		M_Player.OnSpikePickup -= M_Player_OnSpikePickup;
		M_Player.OnBombPickup -= M_Player_OnBombPickup;
	}
}

