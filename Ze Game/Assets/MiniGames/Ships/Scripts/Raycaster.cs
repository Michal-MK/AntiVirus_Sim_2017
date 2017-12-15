using UnityEngine;
using System.Collections;
using Igor.Minigames.Ships;

public class Raycaster : MonoBehaviour {

	private bool isAvailable = false;
	private Location _selected;
	private Vector3 offset = new Vector2(-0.25f, 0.25f);

	private Transform highlight;
	private bool isDirty = false;

	private void FixedUpdate() {
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + offset, Vector3.forward);
		foreach (RaycastHit2D hit in hits) {
			if (hit.transform.name.StartsWith("(")) {
				isDirty = true;
				Location loc = hit.transform.GetComponent<LocationVisual>().location;
				this.isAvailable = loc.isAvailable;
				if (loc.isAvailable) {
					_selected = loc;
				}
				ManageHighlight(hit.transform);

			}
		}
		if (isDirty && hits.Length == 0) {
			isDirty = false;
			foreach (Location location in Field.self.locations) {
				if(location.placedShip == ShipType.NONE) {
					location.LocationVisual.Unhighlight();
				}
			}
		}
	}


	private void ManageHighlight(Transform location) {
		LocationVisual currLocation = location.GetComponent<LocationVisual>();
		if (highlight != null && highlight != location) {
			LocationVisual prevLocation = highlight.GetComponent<LocationVisual>();
			if (prevLocation.location.isAvailable) {
				prevLocation.Unhighlight();
			}
			if (currLocation.location.isAvailable) {
				currLocation.Highlight();
			}
			highlight = location;
		}
		else {
			highlight = location;
			if (currLocation.location.isAvailable) {
				currLocation.Highlight();
			}
		}
	}

	public bool canPlace {
		get { return isAvailable; }
	}

	public Location getSelected {
		get { return _selected; }
	}
}
