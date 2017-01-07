using UnityEngine;
using System.Collections;


public class TurretAttack : MonoBehaviour {

	public GameObject projectile;
	private IEnumerator waitforAttack;
	private Transform enemy;



	void Start(){
		enemy = GameObject.Find ("Enemies").transform;
		StartCoroutine (waitForAttack (2.0f));
	}


	private IEnumerator waitForAttack(float waitTime){
		while(true){
			yield return new WaitForSeconds (waitTime);
			for (int i = 0; i < 6; i++) {
				GameObject bullet = ObjectPooler.script.GetPool ();


				bullet.transform.rotation = Quaternion.AngleAxis (30 * (i * 2), new Vector3 (0, 0, 1));
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
