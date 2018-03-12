using UnityEngine;
using System.Collections;
using Igor.Minigames.Ships;

public class LocationVisual_Game : LocationVisual {
	public override void OnPointerClick() {
		base.OnPointerClick();
		if (ShipsMain.script.cursorMode == Igor.Minigames.Ships.CursorMode.SHIP_PLACEMENT) {
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
						placeLoc.locationVisual.SetSprite(placeLoc.placedShip);
						foreach (Location vis in placeLoc.getNeighborsOnAxis) {
							if (vis.placedShip == ShipType.NONE) {
								vis.PlaceShip(ShipType.TOKEN);
								vis.locationVisual.SetSprite(ShipType.TOKEN);
							}
						}
					}
					ShipsMain.singleplayer.getPlayerField.getAllShips.Add(new Ship(ShipPlacement.current.places, Ships_UI.selectedShip));
					ShipsMain.singleplayer.getAiField.getGenerator.Generate(Ships_UI.selectedShip);
				}
			}
		}
		else if (ShipsMain.script.cursorMode == Igor.Minigames.Ships.CursorMode.SHIP_REMOVE) {
			foreach (Ship ship in ShipsMain.singleplayer.getPlayerField.getAllShips) {
				foreach (Location location in ship.getLocation) {
					if (location == this.location) {
						ShipsMain.singleplayer.getAiField.RemoveShip(ship.getType);
						ShipsMain.singleplayer.getPlayerField.RemoveShip(ship);
						return;
					}
				}
			}
		}
		else if (ShipsMain.script.cursorMode == Igor.Minigames.Ships.CursorMode.ATTACK_MODE) {
			if (!location.isAvailable && !location.isToken) {
				overlayRenderer.sprite = OVER_Hit;
				SetSprite(location.placedShip);
				lockOverlay = true;
				bool sunk = location.getPlacedShip.Damage();
				if (sunk) {
					foreach (Location vis in location.getPlacedShip.getLocation) {
						vis.locationVisual.StartCoroutine(vis.locationVisual.FadeOverlayTo(OVER_Sunk));
					}
					//Implementation with Ai needed
					ShipsMain.singleplayer.getPlayerField.ShipSunk(location.getPlacedShip);
				}
			}
			else if (location.isToken || location.isAvailable) {
				overlayRenderer.sprite = OVER_Miss;
				lockOverlay = true;
			}
		}
	}

	public override void OnPointerEnter() {
		base.OnPointerEnter();
		if (!lockOverlay) {
			if (Ships_UI.isInAttackMode) {
				overlayRenderer.sprite = OVER_Outline;
			}
		}
	}

	public override void OnPointerExit() {
		base.OnPointerExit();
		if (!lockOverlay) {
			overlayRenderer.sprite = null;
		}
	}
}
