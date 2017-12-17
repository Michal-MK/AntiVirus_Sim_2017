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
	public Sprite SPR_occupied;

	private bool lockOverlay = false;

	/// <summary>
	/// Called automatically as an event, it is called on a gameobject, but it doesn't have to do anything with it, I just wanted to make clicking possible
	/// only when you are actually pointing onto someting that can potentially be a ship.
	/// </summary>
	public void OnPointerClick() {
		if (ShipsMain.cursorMode == Igor.Minigames.Ships.CursorMode.SHIP_PLACEMENT) {
			if (ShipPlacement.current != null && ShipPlacement.current.canPlace) {
				bool success = true;
				foreach (Location placeLoc in ShipPlacement.current.places) {
					bool temp = placeLoc.PlaceShip(Ships_UI.selectedShip);
					if (temp == false) {
						success = false;
					}
				}
				if (success) {
					foreach (Location placeLoc in ShipPlacement.current.places) {
						placeLoc.LocationVisual.SetSprite(placeLoc.placedShip);
						foreach (Location vis in placeLoc.getNeighborsOnAxis) {
							if (vis.placedShip == ShipType.NONE) {
								vis.PlaceShip(ShipType.TOKEN);
								vis.LocationVisual.SetSprite(ShipType.TOKEN);
							}
						}
					}
				}
				Field.self.getAllShips.Add(new Ship(ShipPlacement.current.places, Ships_UI.selectedShip));
			}
		}
		else if (ShipsMain.cursorMode == Igor.Minigames.Ships.CursorMode.SHIP_REMOVE) {
			foreach (Ship ship in Field.self.getAllShips) {
				foreach (Location location in ship.getLocation) {
					if (location == this.location) {
						ship.RemoveFromEditor();
						return;
					}
				}
			}
		}
		else if (ShipsMain.cursorMode == Igor.Minigames.Ships.CursorMode.ATTACK_MODE) {
			if (!location.isAvailable && !location.isToken) {
				overlayRenderer.sprite = OVER_Hit;
				lockOverlay = true;
				bool sunk = location.getPlacedShip.Damage();
				if (sunk) {
					foreach (Location vis in location.getPlacedShip.getLocation) {
						vis.LocationVisual.StartCoroutine(vis.LocationVisual.FadeOverlayTo(OVER_Sunk));
					}
				}
			}
			else if (location.isToken || location.isAvailable) {
				overlayRenderer.sprite = OVER_Miss;
				lockOverlay = true;
			}
		}
	}

	private IEnumerator FadeOverlayTo(Sprite newSprite) {
		for (int f = 255; f > 0; f--) {
			overlayRenderer.color = new Color32(255, 255, 255, (byte)f);
			yield return null;
		}
		overlayRenderer.sprite = newSprite;
		for (int f = 0; f <= 255; f++) {
			overlayRenderer.color = new Color32(255, 255, 255, (byte)f);
			yield return null;
		}
	}

	public void OnPointerEnter() {
		if (!lockOverlay) {
			if (Ships_UI.isInAttackMode) {
				overlayRenderer.sprite = OVER_Outline;
			}
		}
	}

	public void OnPointerExit() {
		if (!lockOverlay) {
			overlayRenderer.sprite = null;
		}
	}

	private void SetSprite(ShipType placedShip) {
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
		}
	}

	public void Highlight() {
		selfRender.sprite = Triggered;
	}

	public void Unhighlight() {
		selfRender.sprite = Normal;
	}

	public void Occupied() {
		selfRender.sprite = Unavailable;
	}
}
