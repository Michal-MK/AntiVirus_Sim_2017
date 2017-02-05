using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoidance : MonoBehaviour {

	public RectTransform BG;
	bool preformed = false;
	public RectTransform player;
	public float start_time;
	public GameObject door1;
	public EnemySpawner spawner;
	float zero = 0;
	public Spike spike;
	float avoidDuration = 35;

	private void Update() {
		if (Mathf.Abs(Vector3.Distance(player.position, BG.position)) <= Mathf.Abs(BG.sizeDelta.y / 2 - 10) && preformed == false) {
			start_time = timer.time;
			StartAvoidance();
			preformed = true;	
		}

		if (zero <= 30 && Projectile.spawnedByAvoidance == true) {
			zero += Time.deltaTime / avoidDuration;
			switch (PlayerPrefs.GetInt("difficulty")) {
				case 0:
				TurretAttack.turretSpawnRate = Mathf.Lerp(1.6f, 1.2f, zero);
				break;
				case 1:
				TurretAttack.turretSpawnRate = Mathf.Lerp(1.5f, 1.2f, zero);
				break;
				case 2:
				TurretAttack.turretSpawnRate = Mathf.Lerp(1.4f, 1.1f, zero);
				break;
				case 3:
				TurretAttack.turretSpawnRate = Mathf.Lerp(1.4f, 1f, zero);
				break;
				case 4:
				TurretAttack.turretSpawnRate = Mathf.Lerp(1.3f, 0.9f, zero);
				break;

			}
		}
	}
	public void StartAvoidance() {
		door1.SetActive(true);
		spawner.spawnAvoidance();
		StartCoroutine("hold");
		//StartCoroutine("print");
		Projectile.spawnedByAvoidance = true;
		Canvas_Renderer.script.infoRenderer("!SURVIVE!");

	}


	private IEnumerator hold() {
		yield return new WaitForSeconds(35);
		Projectile.spawnedByAvoidance = false;
		door1.SetActive(false);
		Projectile.projectileSpeed = 10;
		spike.SetPosition();
		Canvas_Renderer.script.infoRenderer("Uff... it's over. Get the Spike and go to the next room.");
		StopAllCoroutines();
		

	}
	private IEnumerator print() {
		while (true) {
			yield return new WaitForSeconds(0.5f);
			print(TurretAttack.turretSpawnRate);
			print(Time.deltaTime / avoidDuration);
		}
	}
}
