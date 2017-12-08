using System;
using UnityEngine;

public class GameProgression : MonoBehaviour {

	public GameObject block;
	public Spike spike;

	public Vector3 playerPos;
	public Vector3 boxPos;
	public Vector3 spikePos;
	public float ZRotationBlock;

	public static GameProgression script;

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
	}

	public void GetValues() {
		playerPos = GameObject.Find("Player").transform.position;
		boxPos = block.transform.position;
		ZRotationBlock = block.transform.rotation.eulerAngles.z;
		spikePos = spike.transform.position;
	}
}
