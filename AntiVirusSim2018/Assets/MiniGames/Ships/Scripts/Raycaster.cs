using UnityEngine;
using Igor.Minigames.Ships;

public class Raycaster : MonoBehaviour {

	private bool isAvailable = false;
	private Location _selected;
	private Vector3 offset = new Vector2(-0.25f, 0.25f);

	private void FixedUpdate() {
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + offset, Vector3.forward);
		foreach (RaycastHit2D hit in hits) {
			if (hit.transform.name.StartsWith("(")) {
				Location loc = hit.transform.GetComponent<LocationVisual>().location;
				this.isAvailable = loc.isAvailable;
				if (loc.isAvailable) {
					_selected = loc;
				}
				else {
					_selected = null;
				}
			}
		}
		if (_selected != null && hits.Length == 0) {
			_selected = null;
			ShipsMain.singleplayer.getPlayerField.ClearHighlights();
		}
	}

	public bool canPlace {
		get { return isAvailable; }
	}

	public Location getSelected {
		get { return _selected; }
	}
}
