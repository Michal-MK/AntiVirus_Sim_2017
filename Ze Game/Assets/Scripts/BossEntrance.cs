using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntrance : MonoBehaviour {
	public GameObject player;
	public GameObject boss;
	public CameraMovement cam;
	public RectTransform BossBG;
	public GameObject bossHP;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			PlayerAttack p = player.GetComponent<PlayerAttack>();
			p.enabled = true;
			GameObject spawnedBoss = Instantiate(boss, new Vector3(-370, -70, 0), Quaternion.identity);
			spawnedBoss.name = "Boss";
			player.GetComponent<M_Player>().boss = spawnedBoss.GetComponent<BossBehaviour>();
			cam.inBossRoom = true;
			StartCoroutine(cam.LerpSize(cam.camSize, BossBG.sizeDelta.x * Screen.height / Screen.width * 0.5f, 0.15f, new Vector3(BossBG.position.x, BossBG.position.y, -10)));
			bossHP.SetActive(true);
			Canvas_Renderer.script.infoRenderer("Here it is... Kill it! (Attack mode with \"Space\").");

		}
	}
}
