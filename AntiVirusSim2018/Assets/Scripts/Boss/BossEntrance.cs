using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Igor.Constants.Strings;

public class BossEntrance : MonoBehaviour {
	//Prefab
	public GameObject boss;
	public GameObject bossHP;


	private GameObject HPHolder;
	private GameObject usedIndicator;
	private bool lockin = false;


	private void Start() {
		HPHolder = GameObject.Find(Boss.BOSS_HEALTH_PLACEHOLDER);
		usedIndicator = transform.parent.Find("UsedPortal").gameObject;
		usedIndicator.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player" && !lockin) {
			if (M_Player.player.pAttack.bombs > 0 && M_Player.player.pAttack.bullets == 5) {
				CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1);
				CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
				lockin = true;
				Control.script.saveManager.Save(Control.currDifficulty, true);
				//print("Saving is disabled");
				Zoom.canZoom = false;
				Player_Movement.canMove = false;
			}
			if (M_Player.player.pAttack.bombs <= 0 || M_Player.player.pAttack.bullets <= 4) {
				Canvas_Renderer.script.DisplayInfo("You are not a worthy opponent!\n"+
												   "Bullets: " + M_Player.player.pAttack.bullets +"/5\n"+
												   "Bombs: "+ M_Player.player.pAttack.bombs + "/1\n"+
												   "Return to me once you have everyting... to meet your demise!\n" +
												   "MuHAHaHaa!!!", "Explore this location further.");
			}

			print("Debug");
			M_Player.player.pAttack.bullets = 5;
			M_Player.player.pAttack.bombs = 1;
		}
	}

	private void CamFadeOut_OnCamFullyFaded() {
		BossTransition();
	}

	private void BossTransition() {
		usedIndicator.SetActive(true);
		RectTransform bossBG = MapData.script.GetBackgroundBoss(1);
		MusicHandler.script.TransitionMusic(MusicHandler.script.room_1_boss);
		GameObject spawnedBoss = Instantiate(boss, bossBG.transform.position - new Vector3(0, bossBG.sizeDelta.y / 3), Quaternion.identity);
		spawnedBoss.GetComponentInChildren<BossHealth>().tpLocation = transform.position;
		spawnedBoss.name = "Boss";
		GameObject health = Instantiate(bossHP, HPHolder.transform.position, Quaternion.identity, HPHolder.transform);
		health.name = "BossHealth";
		StartCoroutine(CameraMovement.script.LerpSize(CameraMovement.defaultCamSize, bossBG.sizeDelta.x * Screen.height / Screen.width * 0.5f, 0.15f, new Vector3(bossBG.position.x, bossBG.position.y, -10)));
		bossHP.SetActive(true);
		Control.script.saveManager.canSave = false;
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}
}
