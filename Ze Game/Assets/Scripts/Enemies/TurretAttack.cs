using UnityEngine;
using System.Collections;


public class TurretAttack : MonoBehaviour {

	public GameObject projectile;
	private IEnumerator waitforAttack;
	public Transform enemy;
	public static float turretSpawnRate;
	private float OriginSpawnRate;
	public Vector3 playerpos;
	ObjectPooler pooler;



	private void Start(){
		pooler = GameObject.Find("EnemyProjectile Pooler").GetComponent<ObjectPooler>();
		enemy = GameObject.Find ("Enemies").transform;
		playerpos = GameObject.FindGameObjectWithTag("Player").transform.position;
		OriginSpawnRate = turretSpawnRate;
		StartCoroutine (waitForAttack (turretSpawnRate));
	}

	private IEnumerator waitForAttack(float waitTime){

		while(true){
			yield return new WaitForSeconds (waitTime);
			playerpos = GameObject.FindGameObjectWithTag("Player").transform.position;
			Projectile.spawnedByAvoidance = true;
			waitTime = turretSpawnRate;




			if (PlayerPrefs.GetInt("difficulty") <= 2) {
				for (int i = 0; i < 1; i++) {
					
					GameObject bullet = pooler.GetPool();
					Vector3 rnd = RandomVec();
					bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, ((playerpos + rnd) - gameObject.transform.position));
					//print(bullet.transform.rotation);
					bullet.transform.position = gameObject.transform.position - (bullet.transform.rotation * new Vector3(0, 1, 0)) * 2;
					bullet.transform.SetParent(enemy);
					bullet.SetActive(true);
				}
			}

			if (PlayerPrefs.GetInt("difficulty") >= 3) {
				for (int i = 0; i < 2; i++) {


					GameObject bullet = pooler.GetPool();
					Vector3 rnd = RandomVec();

					bullet.transform.rotation = Quaternion.FromToRotation(Vector3.down, ((playerpos + rnd) - (gameObject.transform.position - rnd)));

					bullet.transform.position = gameObject.transform.position - (bullet.transform.rotation * new Vector3(0, 1, 0)) * 2;
					bullet.transform.SetParent(enemy);
					bullet.SetActive(true);
				}
			}
		}
	}
	
	public Vector3 RandomVec() {
		int r = 0;
		if (PlayerPrefs.GetInt("difficulty") <= 2) {
				r = Random.Range(-20, 20);
			return Vector2.one * r;
		}
		else if (PlayerPrefs.GetInt("difficulty") >= 3) {
			while (r >= -8 && r <= 8) {
				r = Random.Range(-20, 20);
			}
			print(r);
			return Vector2.one * r;
		}
		else
			return Vector3.zero;



	}
	void OnDestroy(){
		StopAllCoroutines ();
		Projectile.spawnedByAvoidance = false;
		turretSpawnRate = OriginSpawnRate;
	}
}
