using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool {

	private GameObject pooledObject;

	public int Limit { get; }
	public List<GameObject> Pool { get; } = new List<GameObject>();


	public ObjectPool(GameObject type, int upperLimit = -1) {
		pooledObject = type;
		Limit = upperLimit;
	}

	public GameObject Next {
		get {
			foreach (GameObject g in Pool) {
				if (g.activeInHierarchy == false) {
					return g;
				}
			}
			if (Limit != -1 && Pool.Count >= Limit) {
				throw new InvalidOperationException($"Attempting to get next element from the pool but the pool is exhausted. Limit: {Pool.Count}/{Limit}");
			}
			else {
				GameObject newObj = UnityEngine.Object.Instantiate(pooledObject);
				Pool.Add(newObj);
				newObj.GetComponent<IPoolable>().IsPooled = true;
				return newObj;
			}
		}
	}


	public void SwitchEnemyIllumination(MapMode mode) {
		if (mode == MapMode.DARK) {
			pooledObject = Resources.Load("Enemies/" + pooledObject.name + "_Dark") as GameObject;
		}
		else {
			pooledObject = Resources.Load("Enemies/" + pooledObject.name.Replace("_Dark", "")) as GameObject;
		}
		foreach (GameObject g in Pool) {
			g.GetComponent<Enemy>().MapModeSwitch(mode);
		}
	}

	public void ClearPool() {
		foreach (GameObject g in Pool) {
			UnityEngine.Object.Destroy(g);
		}
		Pool.Clear();
	}
}
