using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Avoidance : MonoBehaviour {

	public bool performed = false;
	public RectTransform player;
	public GameObject door1;
	public EnemySpawner spawner;
	public Spike spike;
	public float avoidDuration = 60;
	public bool displayAvoidInfo = true;
	public Toggle saveButton;

	private void Awake() {
		Statics.avoidance = this;
	}

	public void StartAvoidance() {
		door1.SetActive(true);
		spawner.SpawnAvoidance();
		StartCoroutine(HoldAvoidance());
		saveButton.interactable = false;
		Projectile.spawnedByAvoidance = true;
		Projectile.spawnedByKillerWall = false;
		Statics.cameraMovement.RaycastForRooms();
		StartCoroutine(TimeLeft());
	}

	private IEnumerator HoldAvoidance() {
		yield return new WaitForSeconds(avoidDuration - 5);
		spawner.DespawnAvoidance();
		yield return new WaitForSeconds(5);
		saveButton.interactable = true;
		Projectile.spawnedByAvoidance = false;
		door1.SetActive(false);
		spike.SetPosition();
		Statics.cameraMovement.RaycastForRooms();
		Canvas_Renderer.script.InfoRenderer("Uff... it's over. Get the Spike and go to the next room.", "Head south to face the final challenge.");
		performed = true;
		StopAllCoroutines();
	}

	private IEnumerator TimeLeft() {
		Text SideText = Canvas_Renderer.script.info_S;
		int i = (int)avoidDuration - 1;
		bool show = false;

		while (true) {
			yield return new WaitForSeconds(1);
			i--;
			if (i <= 50) {
				show = true;
			}
			if (show) {
				SideText.text = string.Format("{0:00} seconds left!", i);
			}
			if (i <= 0) {
				StopCoroutine(TimeLeft());
				break;
			}
		}
	}

	private void OnDestroy() {
		Statics.avoidance = null;
	}
}
