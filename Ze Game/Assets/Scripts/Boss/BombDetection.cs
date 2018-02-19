using UnityEngine;

public class BombDetection : MonoBehaviour {

	public bool deactivate;
	public bool destroy;

	public void HitByBombExplosion(Bomb bomb) {
		if (deactivate) {
			gameObject.SetActive(false);
		}
		if (destroy) {
			Destroy(gameObject);
		}
	}
}
