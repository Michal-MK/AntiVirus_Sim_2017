using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBody : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.name == "Player") {
			M_Player mp = GameObject.FindGameObjectWithTag("Player").GetComponent<M_Player>();
			mp.GameOver();
		}
	}
}
