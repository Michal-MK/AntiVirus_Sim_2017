using UnityEngine;
using System.Collections;


public class TurretAttack : MonoBehaviour {

	public GameObject projectile;
	public float turretSpawnRateStart;
	public float turretSpawnRateEnd;
	private float currSpawnRate;
	private float originSpawnRate;

	public bool stop = false;

	Vector3 playerpos;
	ObjectPooler pooler;
	Transform enemy;
	Coroutine ChangeFireRate;

	private void Awake() {
		Statics.turretAttack = this;
	}

	private void Start() {
		pooler = GameObject.Find("EnemyProjectile Pooler").GetComponent<ObjectPooler>();
		enemy = GameObject.Find("Enemies").transform;
		playerpos = GameObject.FindGameObjectWithTag("Player").transform.position;

		switch (PlayerPrefs.GetInt("difficulty")) {
			case 0: {
				turretSpawnRateStart = 1.6f;
				turretSpawnRateEnd = 1.4f;
				break;
			}
			case 1: {
				turretSpawnRateStart = 1.5f;
				turretSpawnRateEnd = 1.3f;
				break;
			}
			case 2: {
				turretSpawnRateStart = 1.4f;
				turretSpawnRateEnd = 1.2f;
				break;
			}
			case 3: {
				turretSpawnRateStart = 1.4f;
				turretSpawnRateEnd = 1.2f;
				break;
			}
			case 4: {
				turretSpawnRateStart = 1.3f;
				turretSpawnRateEnd = 1.1f;
				break;
			}
		}
		originSpawnRate = turretSpawnRateStart;
		ChangeFireRate = StartCoroutine(CurrentSpawnRate(turretSpawnRateStart, turretSpawnRateEnd));
		StartCoroutine(WaitForAttack(turretSpawnRateStart));
	}

	private IEnumerator CurrentSpawnRate(float startSpeed, float endSpeed) {
		for (float f = 0; f <= 1; f += Time.deltaTime * 0.03f) {
			currSpawnRate = Mathf.Lerp(startSpeed, endSpeed, f);

			if (stop == true) {
				StopCoroutine(ChangeFireRate);
			}
			else {
				yield return null;
			}
		}
	}


	private IEnumerator WaitForAttack(float spawnRate) {
		while (true) {
			yield return new WaitForSeconds(spawnRate);
			playerpos = GameObject.FindGameObjectWithTag("Player").transform.position;

			Projectile.spawnedByAvoidance = true;
			spawnRate = currSpawnRate;

			int diff = PlayerPrefs.GetInt("difficulty");

			if (diff <= 2) {
				GameObject bullet = pooler.GetPool();
				Vector3 rnd = RandomVec(diff);
				bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, ((playerpos + rnd) - gameObject.transform.position));
				bullet.transform.position = gameObject.transform.position - (bullet.transform.rotation * new Vector3(0, 1, 0)) * 2;
				bullet.transform.SetParent(enemy);
				bullet.SetActive(true);
			}
			else {
				for (int i = 0; i < 2; i++) {

					GameObject bullet = pooler.GetPool();
					Vector3 rnd = RandomVec(diff);

					bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, ((playerpos + rnd) - (gameObject.transform.position)));

					bullet.transform.position = gameObject.transform.position - (bullet.transform.rotation * new Vector3(0, 1, 0)) * 2;
					bullet.transform.SetParent(enemy);
					bullet.SetActive(true);
				}
			}
		}
	}

	public Vector3 RandomVec(int difficulty) {
		int r = 0;
		if (difficulty <= 2) {
			r = Random.Range(-10, 10);
			return Vector2.one * r;
		}
		else if (difficulty >= 3) {
			r = Random.Range(-20, 20);
			return Vector2.one * r;
		}
		else {
			return Vector3.zero;
		}
	}

	void OnDestroy() {
		StopAllCoroutines();
		Projectile.spawnedByAvoidance = false;
		turretSpawnRateStart = originSpawnRate;
		Statics.turretAttack = null;
	}
}
