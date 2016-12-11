using UnityEngine;
using System.Collections;


public class Guide : MonoBehaviour {

	public Spike spike;
	public M_Player player;
	private Vector3 spikepos;
	private Vector3 playerpos;
	public GameObject Arrow;


	void init () {

		//Debug.Log ("Working with: " + spike.transform.position.x +"   "+ spike.transform.position.y+"    "+ 0);


		spikepos = new Vector3 (spike.transform.position.x, spike.transform.position.y, 0);
		playerpos = new Vector3 (player.transform.position.x, player.transform.position.y, 0);
		float distance = Vector3.Distance (playerpos, spikepos);
		Debug.Log (distance);

		GameObject pointArrow = (GameObject)Instantiate (Arrow, new Vector3 (playerpos.x, playerpos.y, 0), Quaternion.FromToRotation (Vector3.up, (spikepos-playerpos)));


	}
}
