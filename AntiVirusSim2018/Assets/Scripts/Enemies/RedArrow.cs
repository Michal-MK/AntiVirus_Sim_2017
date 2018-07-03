using UnityEngine;

class RedArrow : Projectile {

	public GameObject darkModeLight;

	public override void MapModeSwitch(MapData.MapMode mode) {
		base.MapModeSwitch(mode);
		if (mode == MapData.MapMode.DARK) {
			GameObject g = Instantiate(darkModeLight, transform);
			g.transform.localPosition = new Vector3(0, -1.5f, -1);
		}
		else {
			Destroy(transform.GetChild(0).gameObject);
		}
	}
}
