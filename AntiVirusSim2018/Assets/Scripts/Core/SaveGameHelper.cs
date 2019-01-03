using Igor.Constants.Strings;
using System;
using UnityEngine;

public class SaveGameHelper : MonoBehaviour {

	public GameObject block;
	public Spike spike;

	public Vector3 playerPos;
	public Vector3 boxPos;
	public Vector3 spikePos;
	public float ZRotationBlock;

	public static SaveGameHelper script;

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	public void GetValues() {
		playerPos = GameObject.Find(Tags.PLAYER).transform.position;
		boxPos = block.transform.position;
		ZRotationBlock = block.transform.rotation.eulerAngles.z;
		spikePos = spike.transform.position;
	}
}
