using UnityEngine;
using System.Collections;

public class LaserField : MonoBehaviour {
	[SerializeField]
	private GameObject laser_warning = null;
	[SerializeField]
	private GameObject laser = null;
	[SerializeField]
	private int maxLasers = 4;

	private int activeLasers;

	private const float PARTICLE_LIFETIME = 1f;

	private IEnumerator LaserAvoidance() {
		while (Player.Instance.GetCurrentBackground() == MapData.Instance.GetRoom(5).Background) {
			float waitTime = Random.Range(.4f, 1.1f); //Hard .6,1  //walk .4,1.1
			yield return new WaitForSeconds(waitTime);
			int amountSpawned = Control.currDifficulty < 2 ? 1 : 2;
			for (int i = 0; i < amountSpawned; i++) { //Hard 2
				StartCoroutine(SpawnLaser());
				if (activeLasers >= maxLasers) {
					yield return new WaitForSeconds(waitTime);
				}
			}
		}
		HUDisplay.Instance.DisplayInfo(null, Player.Instance.GetCurrentBackground() == MapData.Instance.GetTransition(MapData.Instance.GetRoomLink(5,6)) ?
			"That was insane! Are you alright ? You should probably save... but y know.." : "Still can't get over how insane that looks.");
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.transform.name == Player.Instance.name) {
			StartCoroutine(LaserAvoidance());
		}
	}

	private IEnumerator SpawnLaser() {
		float waitTime = Random.Range(0.1f, 0.3f); // Hard .2, .6
		yield return new WaitForSeconds(waitTime);
		RectTransform bg = MapData.Instance.GetRoom(5).Background;
		Vector3 playerPos = Player.Instance.transform.position;
		float xOff = Control.currDifficulty >= 4 ?
						 Random.Range(0, 2) == 1 ? Random.Range(-20, -2) : Random.Range(2, 25):
						 Random.Range(0, 2) == 1 ? Random.Range(-20, -4) : Random.Range(4, 30);
		bool isTop = Random.Range(0, 2) == 1;
		Vector3 BGEdge = isTop ? new Vector3(playerPos.x + xOff, bg.transform.position.y + bg.sizeDelta.y / 2)
							   : new Vector3(playerPos.x + xOff, bg.transform.position.y - bg.sizeDelta.y / 2);

		GameObject warn = Instantiate(laser_warning, BGEdge, Quaternion.identity);
		warn.transform.rotation = isTop ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
		yield return new WaitForSeconds(PARTICLE_LIFETIME);
		Destroy(warn);
		GameObject _laser = Instantiate(laser, BGEdge, Quaternion.identity);
		laser.transform.localScale = new Vector3(0.3f, 1.4f);
		_laser.transform.rotation = isTop ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
		activeLasers++;
		StartCoroutine(DestroyLaser(_laser, waitTime));
	}

	private IEnumerator DestroyLaser(GameObject laser, float waitTime) {
		SpriteRenderer sr = laser.GetComponentInChildren<SpriteRenderer>();
		BoxCollider2D col = laser.GetComponentInChildren<BoxCollider2D>();
		yield return new WaitForSeconds(waitTime * 0.2f);  //hard 0.5* wait
		for (float f = 0; f < 1; f += Time.deltaTime) {
			sr.color = new Color(1, 1, 1, 1 - f);
			if (f > .5f) {
				col.enabled = false;
			}
			yield return null;
		}
		Destroy(laser);
		activeLasers--;
	}
}
