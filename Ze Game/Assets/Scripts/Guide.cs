using UnityEngine;
using System.Collections;


public class Guide : MonoBehaviour {

	public Spike spike;
	public M_Player player;
	private Vector3 spikepos;
	private Vector3 playerpos;
	public GameObject Arrow;


	void init () {
		spikepos = new Vector3 (spike.transform.position.x, spike.transform.position.y, 0);
		playerpos = new Vector3 (player.transform.position.x, player.transform.position.y, 0);
		float distance = Vector3.Distance (playerpos, spikepos);
		Debug.Log (distance);
	
		//float angle = Vector3.Angle (playerpos, spikepos);

		GameObject pointArrow = (GameObject)Instantiate (Arrow, new Vector3 (playerpos.x + spikepos.x / 5, playerpos.y + spikepos.y / 5, 0), Quaternion.FromToRotation (playerpos, spikepos));


	}
}
