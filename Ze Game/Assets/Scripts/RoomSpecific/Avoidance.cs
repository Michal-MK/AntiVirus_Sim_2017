using System.Collections;
using UnityEngine;

public class Avoidance : MonoBehaviour {

	public RectTransform BG;
	bool preformed = false;
	public RectTransform player;
	public GameObject door1;
	public EnemySpawner spawner;
	public Spike spike;
	float avoidDuration = 60;
	public TurretAttack turr;

	private void Update() {
		if (preformed == false && Mathf.Abs(Vector3.Distance(player.position, BG.position)) <= Mathf.Abs(BG.sizeDelta.y / 2 - 10)) {
			StartAvoidance();
			preformed = true;
		}
	}

	public void StartAvoidance() {
		door1.SetActive(true);
		spawner.spawnAvoidance();
		StartCoroutine("hold");
		Projectile.spawnedByAvoidance = true;
		Projectile.spawnedByKillerWall = false;
		AudioHandler.script.MusicTransition(AudioHandler.script.avoidance);
		Camera.main.GetComponent<CameraMovement>().raycastForRooms();
		Canvas_Renderer.script.infoRenderer("Survive!\n" +
											"(I recommend zooming out using the mouse wheel.)" , "Highly experimental! Caution advised.");

	}


	private IEnumerator hold() {
		yield return new WaitForSeconds(avoidDuration);
		Projectile.spawnedByAvoidance = false;
		door1.SetActive(false);
		spike.SetPosition();
		Camera.main.GetComponent<CameraMovement>().raycastForRooms();
		Canvas_Renderer.script.infoRenderer("Uff... it's over. Get the Spike and go to the next room.", "Hint - This may be unexpected.");
		StopAllCoroutines();


	}
}
