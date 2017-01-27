using UnityEngine;
using System.Collections;


public class TurretAttack : MonoBehaviour {

	public GameObject projectile;
	private IEnumerator waitforAttack;
	private Transform enemy;
	public static float turretSpawnRate;
	ObjectPooler pooler;



	void Start(){
		pooler = GameObject.Find("EnemyProjectile Pooler").GetComponent<ObjectPooler>();
		enemy = GameObject.Find ("Enemies").transform;
		StartCoroutine (waitForAttack (turretSpawnRate));
	}


	private IEnumerator waitForAttack(float waitTime){

		while(true){
			yield return new WaitForSeconds (waitTime);
			waitTime = turretSpawnRate;

			for (int i = 0; i < 6; i++) {
				int angle = Random.Range (0, 360);

				gameObject.transform.rotation = Quaternion.AngleAxis (angle, Vector3.back);

				GameObject bullet = pooler.GetPool ();


				bullet.transform.rotation = Quaternion.AngleAxis (angle + 30 * (i * 2), new Vector3 (0, 0, 1));
				bullet.transform.position = transform.position - (bullet.transform.rotation * new Vector3(0,1,0))*2;
				bullet.transform.SetParent (enemy);
				bullet.SetActive (true);

				if (bullet == null) {
					break;
				}

			}
		}
	}

	void OnDestroy(){
		StopAllCoroutines ();
	}
}
