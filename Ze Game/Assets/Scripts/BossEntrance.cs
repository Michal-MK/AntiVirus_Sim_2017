using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntrance : MonoBehaviour {
	public GameObject player;
	public GameObject boss;

	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.transform.tag == "Player") {
			PlayerAttack p = player.GetComponent<PlayerAttack>();
			p.enabled = true;
			//GameObject bosss = Instantiate(boss, new Vector3(-370, -70, 0), Quaternion.identity);
			//player.GetComponent<M_Player>().boss = bosss.GetComponent<BossBehaviour>();
			print('A');
		}
	}
	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			PlayerAttack p = player.AddComponent<PlayerAttack>();
			p.enabled = true;
			//GameObject bosss = Instantiate(boss, new Vector3(-370, -70, 0), Quaternion.identity);
			//player.GetComponent<M_Player>().boss = bosss.GetComponent<BossBehaviour>();
			print('B');

		}
	}
}
