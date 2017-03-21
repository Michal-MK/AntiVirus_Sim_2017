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

	private void Awake() {
		Statics.avoidance = this;
	}

	private void Update() {
		if (!preformed && Mathf.Abs(Vector3.Distance(player.position, BG.position)) <= Mathf.Abs(BG.sizeDelta.y / 2 - 10)) {
			StartAvoidance();
			preformed = true;
		}
	}

	public void StartAvoidance() {
		door1.SetActive(true);
		spawner.spawnAvoidance();
		StartCoroutine("hold");
		SaveButton.interactable = false;
		Projectile.spawnedByAvoidance = true;
		Projectile.spawnedByKillerWall = false;
		Statics.music.MusicTransition(Statics.music.avoidance);
		Camera.main.GetComponent<CameraMovement>().raycastForRooms();
		if (displayAvoidInfo) {
			Statics.canvasRenderer.infoRenderer("Survive!\n" +
												"(I recommend zooming out using the mouse wheel.)", "Highly experimental! Caution advised.");
			displayAvoidInfo = false;
		}
	}


	private IEnumerator hold() {
		yield return new WaitForSeconds(avoidDuration);
		SaveButton.interactable = true;
		Projectile.spawnedByAvoidance = false;
		door1.SetActive(false);
		spike.SetPosition();
		Camera.main.GetComponent<CameraMovement>().raycastForRooms();
		Statics.canvasRenderer.infoRenderer("Uff... it's over. Get the Spike and go to the next room.", "Hint - This may be unexpected.");
		StopAllCoroutines();


	}
	private void OnDestroy() {
		Statics.avoidance = null;
	}
}
