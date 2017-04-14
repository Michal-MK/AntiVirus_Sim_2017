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
	float avoidDuration = 60;
	public bool displayAvoidInfo = true;
	public TurretAttack turr;
	public Toggle SaveButton;
	public GameObject sign;

	private void Awake() {
		Statics.avoidance = this;
	}


	public void StartAvoidance() {
		door1.SetActive(true);
		spawner.spawnAvoidance();
		StartCoroutine(hold());
		SaveButton.interactable = false;
		Projectile.spawnedByAvoidance = true;
		Projectile.spawnedByKillerWall = false;
		Statics.music.MusicTransition(Statics.music.avoidance);
		Camera.main.GetComponent<CameraMovement>().raycastForRooms();
		StartCoroutine(TimeLeft());
	}


	private IEnumerator hold() {
		yield return new WaitForSeconds(avoidDuration);
		SaveButton.interactable = true;
		Projectile.spawnedByAvoidance = false;
		door1.SetActive(false);
		spike.SetPosition();
		Camera.main.GetComponent<CameraMovement>().raycastForRooms();
		Statics.canvasRenderer.infoRenderer("Uff... it's over. Get the Spike and go to the next room.", "Head south to face the final challenge.");
		StopAllCoroutines();


	}

	private IEnumerator TimeLeft() {
		Text SideText = Statics.canvasRenderer.info_S;
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
