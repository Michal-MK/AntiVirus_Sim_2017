using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntrance : MonoBehaviour {
	public GameObject player;
	public GameObject boss;
	public CameraMovement cam;
	public RectTransform BossBG;
	public GameObject bossHP;

	private GameObject HPHolder;

	private void Awake() {
		Statics.bossEntrance = this;
	}
	private void Start() {
		HPHolder = GameObject.Find("BossHealthPlaceHolder");
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {

			//Spike.spikesCollected = 5;
			//PlayerAttack.bullets = 5;
			//PlayerAttack.bombs = 1;
			//Coins.coinsCollected = 5;

			if (PlayerAttack.bombs > 0 && PlayerAttack.bullets == 5) {
				Statics.music.MusicTransition(Statics.music.boss);
				GameObject spawnedBoss = Instantiate(boss, new Vector3(-370, -70, 0), Quaternion.identity);
				spawnedBoss.name = "Boss";
				player.GetComponent<M_Player>().boss = spawnedBoss.GetComponent<BossBehaviour>();
				GameObject health = Instantiate(bossHP,HPHolder.transform.position,Quaternion.identity,GameObject.Find("BossHealthPlaceHolder").transform);
				health.name = "BossHealth";
				cam.bossFightCam(1);
				StartCoroutine(cam.LerpSize(cam.defaultCamSize, BossBG.sizeDelta.x * Screen.height / Screen.width * 0.5f, 0.15f, new Vector3(BossBG.position.x, BossBG.position.y, -10)));
				bossHP.SetActive(true);

			}
			if(PlayerAttack.bombs <= 0 || PlayerAttack.bullets < 5) {
				Statics.canvasRenderer.infoRenderer("You are not a worthy opponent!\n"+
													"Bullets: " + PlayerAttack.bullets +"/5\n"+
													"Bombs: "+ PlayerAttack.bombs + "/1", "Explore this location further");

			}
		}
	}
	public void SpawnBossOnLoad() {
		GameObject spawnedBoss = Instantiate(boss, new Vector3(-370, -70, 0), Quaternion.identity);
		spawnedBoss.name = "Boss";
		player.GetComponent<M_Player>().boss = spawnedBoss.GetComponent<BossBehaviour>();
		HPHolder = HPHolder = GameObject.Find("BossHealthPlaceHolder");
		GameObject health = Instantiate(bossHP, HPHolder.transform.position, Quaternion.identity, GameObject.Find("BossHealthPlaceHolder").transform);
		health.name = "BossHealth";
		//StartCoroutine(Statics.bossBehaviour.InterPhase());
	}

	private void OnDestroy() {
		Statics.bossEntrance = null;
	}
}
