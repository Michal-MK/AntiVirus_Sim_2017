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

			//Spike.spikesCollected = 5;
			//PlayerAttack.bullets = 5;
			//PlayerAttack.bombs = 1;
			//Coins.coinsCollected = 5;

			if (PlayerAttack.bombs > 0 && PlayerAttack.bullets == 5) {
				AudioHandler.script.MusicTransition(AudioHandler.script.boss);
				GameObject spawnedBoss = Instantiate(boss, new Vector3(-370, -70, 0), Quaternion.identity);
				spawnedBoss.name = "Boss";
				player.GetComponent<M_Player>().boss = spawnedBoss.GetComponent<BossBehaviour>();
				cam.bossFightCam(1);
				StartCoroutine(cam.LerpSize(cam.defaultCamSize, BossBG.sizeDelta.x * Screen.height / Screen.width * 0.5f, 0.15f, new Vector3(BossBG.position.x, BossBG.position.y, -10)));
				bossHP.SetActive(true);
			}
			if(PlayerAttack.bombs <= 0 || PlayerAttack.bullets < 5) {
				Canvas_Renderer.script.infoRenderer("You are not a worthy opponent!\n"+
													"Bullets: " + PlayerAttack.bullets +"/5\n"+
													"Bombs: "+ PlayerAttack.bombs + "/1", "Explore this location further");

			}
		}
	}
}
