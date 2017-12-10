using UnityEngine;
using UnityEngine.UI;
using AttackTypes;

public class HUDElements : UserInterface {
	public delegate void HUDAttackUpdates(AttackType type, int amount, bool state);
	public delegate void HUDCollectUpdates(Collectibles type, int amount);

	public Image bombImage;
	public Text bombAmount;

	public Image bulletImage;
	public Text bulletAmount;

	public Image currentlySelectedImg;

	public Text spikesAmount;
	public Text coinsAmount;

	public Sprite spikeSpr;
	public Sprite bombSpr;

	private static Image s_bombImg;
	private static Text s_bombAmount;

	private static Image s_bulletImage;
	private static Text s_bulletAmount;

	private static Image s_currentlySelectedImg;

	private static Text s_spikesAmount;
	private static Text s_coinsAmount;

	private static Sprite s_spikeSpr;
	private static Sprite s_bombSpr;


	public enum Collectibles {
		COINS,
		SPIKES,
		BOMB
	}

	private AttackType currentSelectedAtkType = AttackType.NOTHING;

	private void Awake() {
		s_bombImg = bombImage;
		s_bombAmount = bombAmount;
		s_bulletImage = bulletImage;
		s_bulletAmount = bulletAmount;
		s_currentlySelectedImg = currentlySelectedImg;
		s_spikesAmount = spikesAmount;
		s_coinsAmount = coinsAmount;
		s_spikeSpr = spikeSpr;
		s_bombSpr = bombSpr;
		PlayerAttack.OnAmmoChanged += AmmoSwitch;
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

	public static void SetVisibility(AttackType type, bool state, int amount) {
		switch (type) {
			case AttackType.BULLETS: {
				s_bulletImage.sprite = s_spikeSpr;
				s_bulletImage.gameObject.SetActive(state);
				s_bulletAmount.text = "x " + amount;
				s_bulletAmount.gameObject.SetActive(state);
				break;
			}
			case AttackType.BOMBS: {
				s_bombImg.sprite = s_bombSpr;
				s_bombImg.gameObject.SetActive(state);
				s_bombAmount.text = "x " + amount;
				s_bombAmount.gameObject.SetActive(state);
				break;
			}
		}
		s_currentlySelectedImg.gameObject.SetActive(state);
	}

	public static void SetVisibility(Collectibles type, bool state, int amount) {
		switch (type) {
			case Collectibles.COINS: {
				s_coinsAmount.gameObject.SetActive(state);
				return;
			}
			case Collectibles.BOMB: {
				s_bombImg.gameObject.SetActive(state);
				s_bombAmount.gameObject.SetActive(state);
				break;
			}
			case Collectibles.SPIKES: {
				s_spikesAmount.gameObject.SetActive(state);
				break;
			}
		}
	}
}
