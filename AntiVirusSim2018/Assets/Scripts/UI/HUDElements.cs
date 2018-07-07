using UnityEngine;
using UnityEngine.UI;

public class HUDElements : UserInterface {

	public delegate void HUDAttackUpdates(AttackType type, int amount);
	public delegate void HUDAttackVisibility(AttackType type, bool state);

	public Image bombImage;
	public Text bombAmount;

	public Image bulletImage;
	public Text bulletAmount;

	public Image currentlySelectedImg;

	private Sprite spikeSprt;
	private Sprite bombSprt;

	public enum HudCollectibles {
		COINS,
		SPIKES,
		BOMB
	}

	private AttackType currentSelectedAtkType;

	protected override void Awake() {
		PlayerAttack.OnAmmoChanged += AmmoSwitch;
		PlayerAttack.OnAmmoPickup += SetVisibility;
		spikeSprt = bulletImage.sprite;
		bombSprt = bombImage.sprite;
		base.Awake();
	}

	private void AmmoSwitch(AttackType type, int ammo) {
		currentSelectedAtkType = type;
		switch (currentSelectedAtkType) {
			case AttackType.BULLETS: {
				currentlySelectedImg.sprite = spikeSprt;
				bulletAmount.text = "x " + ammo;
				break;
			}
			case AttackType.BOMBS: {
				currentlySelectedImg.sprite = bombSprt;
				bombAmount.text = "x " + ammo;
				break;
			}
		}
	}

	public void SetVisibility(AttackType type, bool state) {
		switch (type) {
			case AttackType.BULLETS: {
				bulletImage.sprite = spikeSprt;
				bulletImage.gameObject.SetActive(state);
				bulletAmount.gameObject.SetActive(state);
				break;
			}
			case AttackType.BOMBS: {
				bombImage.sprite = bombSprt;
				bombImage.gameObject.SetActive(state);
				bombAmount.gameObject.SetActive(state);
				break;
			}
		}
		currentlySelectedImg.gameObject.SetActive(state);
	}

	private void OnDestroy() {
		PlayerAttack.OnAmmoChanged -= AmmoSwitch;
		PlayerAttack.OnAmmoPickup -= SetVisibility;
	}
}
