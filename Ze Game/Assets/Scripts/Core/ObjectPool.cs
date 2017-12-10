using System;
using System.Collections.Generic;
using UnityEngine;

class ObjectPool {
	private GameObject pooledObject;
	private int limit;

	private List<GameObject> instantiatedObjects = new List<GameObject>();

	public ObjectPool(GameObject type, int upperLimit = -1) {
		pooledObject = type;
		limit = upperLimit;
	}

	public GameObject getNext {
		get {
			foreach (GameObject g in instantiatedObjects) {
				if(g.activeInHierarchy == false) {
					return g;
				}
			}
			if (limit != -1 && instantiatedObjects.Count >= limit) {
				return null;
			}
			else {
				GameObject newObj = GameObject.Instantiate(pooledObject);
				instantiatedObjects.Add(newObj);
				return newObj;
			}
		}
	}

	public int getLimit {
		get { return limit; }
	}

	public List<GameObject> getAllInstantiated {
		get { return instantiatedObjects; }
	}
}
