using System;
using System.Collections.Generic;
using UnityEngine;

class ObjectPool {
	private GameObject pooledObject;

	public int getLimit { get; }
	public List<GameObject> getAllInstantiated { get; } = new List<GameObject>();


	public ObjectPool(GameObject type, int upperLimit = -1) {
		pooledObject = type;
		getLimit = upperLimit;
	}

	public GameObject getNext {
		get {
			foreach (GameObject g in getAllInstantiated) {
				if(g.activeInHierarchy == false) {
					return g;
				}
			}
			if (getLimit != -1 && getAllInstantiated.Count >= getLimit) {
				return null;
			}
			else {
				GameObject newObj = GameObject.Instantiate(pooledObject);
				getAllInstantiated.Add(newObj);
				return newObj;
			}
		}
	}


	public void SwitchEnemyIllumination(MapData.MapMode mode) {
		if (mode == MapData.MapMode.DARK) {
			pooledObject = Resources.Load("Enemies/" + pooledObject.name + "_Dark") as GameObject;
		}
		else {
			pooledObject = Resources.Load("Enemies/" + pooledObject.name.Replace("_Dark", "")) as GameObject;
		}
		foreach (GameObject g in getAllInstantiated) {
			g.GetComponent<Projectile>().MapModeSwitch(mode);
		}
	}
}
