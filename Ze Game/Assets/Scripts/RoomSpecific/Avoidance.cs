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

	private void Update() {
		if (Mathf.Abs(Vector3.Distance(player.position, BG.position)) <= Mathf.Abs(BG.sizeDelta.y / 2 - 10) && preformed == false) {
			start_time = timer.time;
			StartAvoidance();
			preformed = true;	
		}

		if (zero <= 30 && Projectile.spawnedByAvoidance == true) {
			zero += Time.deltaTime / 30 - Mathf.Pow(PlayerPrefs.GetInt("difficulty"),2);
			Projectile.projectileSpeed = Mathf.Lerp(10, 30, zero);
			TurretAttack.turretSpawnRate = Mathf.Lerp(1.5f, 1f, zero);
		}
	}
	public void StartAvoidance() {
		door1.SetActive(true);
		spawner.spawnAvoidance();
		StartCoroutine("hold");
		//StartCoroutine("print");
		Projectile.spawnedByAvoidance = true;
	}


	private IEnumerator hold() {
		yield return new WaitForSeconds(35);

		door1.SetActive(false);
		StopAllCoroutines();
		Projectile.projectileSpeed = 10;
		spike.SetPosition();

	}
	private IEnumerator print() {
		while (true) {
			yield return new WaitForSeconds(0.5f);
			print(Projectile.projectileSpeed);
			print(TurretAttack.turretSpawnRate);
		}
	}
}
