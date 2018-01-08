using UnityEngine;
using System.Collections;
using Igor.Minigames.Ships;

public class LocationVisual_Editor : LocationVisual {
	public override void OnPointerClick() {
		base.OnPointerClick();
		switch (ShipsCreationMain.script.mode) {
			case ShipsCreationMain.CursorMode.PART_PLACEMENT: {
				SetSprite(ShipType.TOKEN);
				ShipEdit.getCurrentShip.locations.Add(this.location);
				return;
			}
			case ShipsCreationMain.CursorMode.PART_DELETION: {
				if (location.coordinates != Vector2.one * 4) {
					SetSprite(ShipType.NONE);
					ShipEdit.getCurrentShip.locations.Remove(this.location);
				}
				return;
			}
			default: {
				return;
			}
		}
	}

	public override void OnPointerEnter() {
		base.OnPointerEnter();
		if (!lockOverlay) {
			overlayRenderer.sprite = OVER_Outline;
		}
	}

	public override void OnPointerExit() {
		base.OnPointerExit();
		if (!lockOverlay) {
			overlayRenderer.sprite = null;
		}
	}
}
