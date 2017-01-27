using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {

	public GameObject pooledObject;
	public int max;
	public bool expandable = true;
	public Transform enemy;

	public List<GameObject> pool;


	void Start () {
		pool = new List<GameObject> ();
		for (int i = 0; i < max; i++) {
			GameObject obj = Instantiate (pooledObject);

			obj.SetActive (false);
			obj.transform.SetParent(enemy);
			pool.Add (obj);
			GetPool();

		}
	}
	public GameObject GetPool(){
		for (int i = 0; i < pool.Count; i++) {
			if (!pool [i].activeInHierarchy) {
				return pool [i];
			}
		}
		if (expandable == true) {
			GameObject obj = Instantiate (pooledObject);
			pool.Add (obj);
			return obj;
		}
		else {
			return null;
		}
	}
}
