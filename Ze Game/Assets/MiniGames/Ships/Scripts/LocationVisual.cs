using UnityEngine;
using Igor.Minigames.Ships;
using System;
using System.Collections;

public class LocationVisual : MonoBehaviour {
	public Location location;

	public SpriteRenderer selfRender;
	public SpriteRenderer overlayRenderer;

	public Sprite Triggered;
	public Sprite Normal;
	public Sprite Unavailable;

	public Sprite OVER_Outline;
	public Sprite OVER_Miss;
	public Sprite OVER_Hit;
	public Sprite OVER_Sunk;
	public Sprite OVER_Hint;

	public Sprite SPR_submarine;
	public Sprite SPR_cargo;
	public Sprite SPR_war;
	public Sprite SPR_air;
	public Sprite SPR_battleCruiser;
	public Sprite SPR_custom;
	public Sprite SPR_occupied;

	protected bool lockOverlay = false;

	/// <summary>
	/// Called automatically as an event, it is called on a gameobject, but it doesn't have to do anything with it, I just wanted to make clicking possible
	/// only when you are actually pointing onto someting that can potentially be a ship.
	/// </summary>
	public virtual void OnPointerClick() {
		print("Clicked on " + gameObject.name);
	}

	public IEnumerator FadeOverlayTo(Sprite newSprite) {
		for (int i = 255; i > 0; i--) {
			overlayRenderer.color = new Color32(255, 255, 255, (byte)i);
			yield return null;
		}
		overlayRenderer.sprite = newSprite;
		for (int i = 0; i <= 255; i++) {
			overlayRenderer.color = new Color32(255, 255, 255, (byte)i);
			yield return null;
		}
	}

	public virtual void OnPointerEnter() {

	}

	public virtual void OnPointerExit() {

	}

	public void SetSprite(ShipType placedShip) {
		switch (placedShip) {
			case ShipType.SUBMARINE: {
				selfRender.sprite = SPR_submarine;
				return;
			}
			case ShipType.CARGO: {
				selfRender.sprite = SPR_cargo;
				return;
			}
			case ShipType.WAR: {
				selfRender.sprite = SPR_war;
				return;
			}
			case ShipType.AIR: {
				selfRender.sprite = SPR_air;
				return;
			}
			case ShipType.BATTLECRUSER: {
				selfRender.sprite = SPR_battleCruiser;
				return;
			}
			case ShipType.TOKEN: {
				selfRender.sprite = SPR_occupied;
				return;
			}
			case ShipType.NONE: {
				selfRender.sprite = null;
				return;
			}
			case ShipType.CUSTOM: {
				selfRender.sprite = SPR_custom;
				return;
			}
			default: {
				throw new System.Exception("Shiptype not implemented");
			}
		}
	}

	public void Highlight() {
		selfRender.sprite = Triggered;
	}

	public void Unhighlight(bool preparingForGame = false) {
		selfRender.sprite = Normal;
		if (preparingForGame) {
			//Do seomtihng special ??
		}
	}

	public void Occupied() {
		selfRender.sprite = Unavailable;
	}
}
