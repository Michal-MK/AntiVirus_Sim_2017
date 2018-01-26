using UnityEngine;
using UnityEngine.UI;
using AttackTypes;

public class HUDElements : UserInterface {
	public delegate void HUDAttackUpdates(AttackType type, int amount, bool state);
	public delegate void HUDCollectUpdates(Collectibles type, int amount);

	public delegate void HUDAttackVisibility(AttackType type, bool state, int amount);
	public delegate void HUDCollectibleVisibility(Collectibles type);

	public Image bombImage;
	public Text bombAmount;

	public Image bulletImage;
	public Text bulletAmount;

	public Image currentlySelectedImg;

	public Text spikesAmount;
	public Text coinsAmount;

	public Sprite spikeSpr;
	public Sprite bombSpr;

	public static HUDElements script;

	public enum Collectibles {
		COINS,
		SPIKES,
		BOMB
	}

	private AttackType currentSelectedAtkType = AttackType.NOTHING;

	private void Awake() {
		if(script == null) {
			script = this;
		}
		PlayerAttack.OnAmmoChanged += AmmoSwitch;
		PlayerAttack.OnAmmoPickup += SetVisibility;
	}

	private void AmmoSwitch(AttackType type, int ammo, bool state = true) {
		currentSelectedAtkType = type;
		switch (currentSelectedAtkType) {
			case AttackType.NOTHING: {
				Debug.Log("Doing Nothing");
				return;
			}
			case AttackType.BULLETS: {
				currentlySelectedImg.sprite = spikeSpr;
				bulletAmount.text = "x " + ammo;
				currentlySelectedImg.transform.parent.gameObject.SetActive(state);
				break;
			}
			case AttackType.BOMBS: {
				currentlySelectedImg.sprite = bombSpr;
				bombAmount.text = "x " + ammo;
				currentlySelectedImg.transform.parent.gameObject.SetActive(state);
				break;
			}
		}
	}

	public void SetVisibility(AttackType type, bool state, int amount) {
		switch (type) {
			case AttackType.BULLETS: {
				bulletImage.sprite = spikeSpr;
				bulletImage.gameObject.SetActive(state);
				bulletAmount.text = "x " + amount;
				bulletAmount.gameObject.SetActive(state);
				break;
			}
			case AttackType.BOMBS: {
				bombImage.sprite = bombSpr;
				bombImage.gameObject.SetActive(state);
				bombAmount.text = "x " + amount;
				bombAmount.gameObject.SetActive(state);
				break;
			}
		}
		currentlySelectedImg.gameObject.SetActive(state);
	}

	public void SetVisibility(Collectibles type, bool state, int amount) {
		switch (type) {
			case Collectibles.COINS: {
				coinsAmount.gameObject.SetActive(state);
				return;
			}
			case Collectibles.BOMB: {
				bombAmount.gameObject.SetActive(state);
				bombAmount.gameObject.SetActive(state);
				break;
			}
			case Collectibles.SPIKES: {
				spikesAmount.gameObject.SetActive(state);
				break;
			}
		}
	}
	private void OnDestroy() {
		script = null;
		PlayerAttack.OnAmmoChanged -= AmmoSwitch;
		PlayerAttack.OnAmmoPickup -= SetVisibility;
	}
}
