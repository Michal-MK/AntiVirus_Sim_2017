using UnityEngine;
using System.Collections;
using Igor.Constants.Strings;

public class TurretAttack : MonoBehaviour {

	public GameObject projectile;
	public GameObject target;
	public float turretSpawnRateStart;
	public float turretSpawnRateEnd;
	public float spawnRateDelta;
	private float currentSpawnRate;

	private bool _fire = true;

	public bool useDefaultTiming;
	public bool applyRandomness;
	public float randomnessMultiplier;
	public float projectileLifeSpan = -1;

	private Vector3 targetPos;
	private ObjectPool pool_EnemyProjectile;
	private Transform enemy;
	private MapMode myStance;

	private void Start() {
		pool_EnemyProjectile = new ObjectPool(Resources.Load("Enemies/" + projectile.name + (MapData.Instance.CurrentMapMode == MapMode.LIGHT ? "" : "_Dark")) as GameObject);
		myStance = MapData.Instance.CurrentMapMode;
		enemy = GameObject.Find("Enemies").transform;
		targetPos = target.transform.position;
		if (useDefaultTiming) {
			switch (Control.currDifficulty) {
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
			StartCoroutine(WaitForAttack());
		}
		else {
			StartCoroutine(WaitForAttack());
		}
	}

	private IEnumerator WaitForAttack() {
		currentSpawnRate = turretSpawnRateStart;

		while (_fire) {
			targetPos = target.transform.position;
			int diff = Control.currDifficulty;

			if (diff <= 2) {
				FireBullet();
			}
			else {
				for (int i = 0; i < 2; i++) {
					FireBullet();
				}
			}
			currentSpawnRate = Mathf.Clamp(currentSpawnRate - spawnRateDelta, turretSpawnRateEnd, turretSpawnRateStart);
			yield return new WaitForSeconds(currentSpawnRate);
		}
	}

	internal void CleanUp() {
		pool_EnemyProjectile.ClearPool();
	}

	private void FireBullet() {
		Projectile bullet = pool_EnemyProjectile.Next.GetComponent<Projectile>();
		if (applyRandomness) {
			Vector3 rnd = RandomVec(Control.currDifficulty);
			bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, ((targetPos + rnd) - gameObject.transform.position));
		}
		else {
			bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, (targetPos - gameObject.transform.position));
		}
		bullet.transform.position = gameObject.transform.position - (bullet.transform.rotation * new Vector3(0, 1, 0)) * 2;
		bullet.transform.SetParent(enemy);
		bullet.gameObject.SetActive(true);
		bullet.projectileSpeed = 20;
		bullet.IsKillable = false;
		bullet.IsPooled = true;
		if (projectileLifeSpan > 0) {
			bullet.StartCoroutine(bullet.Kill(projectileLifeSpan));
		}
		bullet.Fire();
	}

	public void MapStanceSwitch(MapMode stance) {
		switch (stance) {
			case MapMode.LIGHT: {
				if (stance != myStance) {
					GameObject g = Instantiate(Resources.Load<GameObject>(PrefabNames.ENEMY_TURRET), transform.position, transform.rotation, transform.parent);
					g.transform.localScale = transform.localScale;
					Destroy(g.GetComponent<TurretAttack>());
					g.AddComponent(this);
					Destroy(gameObject);

				}
				break;
			}
			case MapMode.DARK: {
				if (stance != myStance) {
					GameObject g = Instantiate(Resources.Load<GameObject>(PrefabNames.ENEMY_TURRET + "_Dark"), transform.position, transform.rotation, transform.parent);
					g.transform.localScale = transform.localScale;
					Destroy(g.GetComponent<TurretAttack>());
					g.AddComponent(this);
					Destroy(gameObject);

				}
				break;
			}
		}
	}

	public Vector3 RandomVec(int difficulty) {
		float r = 0;
		if (difficulty <= 2) {
			r = Random.Range(-1 * randomnessMultiplier, 1 * randomnessMultiplier);
			return Vector2.one * r;
		}
		else {
			r = Random.Range(-2 * randomnessMultiplier, 2 * randomnessMultiplier);
			return Vector2.one * r;
		}
	}

	void OnDestroy() {
		StopAllCoroutines();
	}

	public void Stop() {
		_fire = false;
	}

	public float getCurrentSpawnRate {
		get { return currentSpawnRate; }
	}
}



