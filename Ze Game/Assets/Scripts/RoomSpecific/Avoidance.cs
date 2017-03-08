using System.Collections;
using UnityEngine;

public class Avoidance : MonoBehaviour {

	public RectTransform BG;
	bool preformed = false;
	public RectTransform player;
	public GameObject door1;
	public EnemySpawner spawner;
	public Spike spike;
	float avoidDuration = 35;
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
		Canvas_Renderer.script.infoRenderer("Survive! (I recommend zooming out by scrolling the mouse.)");

	}


	private IEnumerator hold() {
		yield return new WaitForSeconds(avoidDuration);
		Projectile.spawnedByAvoidance = false;
		door1.SetActive(false);
		spike.SetPosition();
		Canvas_Renderer.script.infoRenderer("Uff... it's over. Get the Spike and go to the next room.");
		StopAllCoroutines();


	}
}
