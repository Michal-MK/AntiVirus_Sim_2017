using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {

	public static ObjectPooler script;
	public GameObject pooledObject;
	public int max = 64;
	public bool expandable = true;

	public List<GameObject> pool;

	void Awake(){
		script = this;
	}

	void Start () {
		pool = new List<GameObject> ();
		for (int i = 0; i < max; i++) {
			GameObject obj = (GameObject)Instantiate (pooledObject);
			pooledObject.SetActive (false);
			pool.Add (obj);
		}
	}
	public GameObject GetPool(){
		for (int i = 0; i < pool.Count; i++) {
			if (!pool [i].activeInHierarchy) {
				return pool [i];
			}
		}
		if (expandable == true) {
			GameObject obj = (GameObject)Instantiate (pooledObject);
			pool.Add (obj);
			return obj;
		}
		else {
			return null;
		}
	}
}
