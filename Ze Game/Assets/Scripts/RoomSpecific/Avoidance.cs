using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Avoidance : MonoBehaviour {

	public RectTransform BG;
	public bool preformed = false;
	public RectTransform player;
	public GameObject door1;
	public EnemySpawner spawner;
	public Spike spike;
	public bool displayAvoidInfo = true;
	public TurretAttack turr;
	public Toggle saveButton;
	public GameObject sign;

	private void Awake() {
		Statics.avoidance = this;
	}

	public void StartAvoidance() {
		door1.SetActive(true);
		spawner.SpawnAvoidance();
		StartCoroutine(HoldAvoidance(60f));
		saveButton.interactable = false;
		Projectile.spawnedByAvoidance = true;
		Projectile.spawnedByKillerWall = false;
		//Statics.music.PlayMusic(Statics.music.avoidance);
		Camera.main.GetComponent<CameraMovement>().RaycastForRooms();
	}

	private IEnumerator HoldAvoidance(float seconds) {
		StartCoroutine(TimeLeft(seconds));
		yield return new WaitForSeconds(seconds);
		saveButton.interactable = true;
		Projectile.spawnedByAvoidance = false;
		door1.SetActive(false);
		spike.SetPosition();
		Camera.main.GetComponent<CameraMovement>().RaycastForRooms();
		Statics.canvasRenderer.infoRenderer("Uff... it's over. Get the Spike and go to the next room.", "Head south to face the final challenge.");
		StopAllCoroutines();
	}

	private IEnumerator TimeLeft(float seconds) {
		Text SideText = Statics.canvasRenderer.info_S;
		int i = (int)seconds - 1;
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
				StopCoroutine(TimeLeft(seconds));
				break;
			}
		}
	}

	private void OnDestroy() {
		Statics.avoidance = null;
	}
}
