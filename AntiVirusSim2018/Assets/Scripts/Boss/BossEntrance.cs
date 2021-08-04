
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
		if (collision.CompareTag(Tags.PLAYER) && !lockin) {

			//print("Debug");
			//Player.Instance.pAttack.Bullets = 5;
			//Player.Instance.pAttack.Bombs = 1;

			if (Player.Instance.pAttack.Bombs > 0 && Player.Instance.pAttack.Bullets >= 5) {
				CamFadeOut.Instance.PlayTransition(CameraTransitionModes.TRANSITION_SCENES, 1);
				CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
				lockin = true;
				Zoom.CanZoom = false;
				PlayerMovement.CanMove = false;
			}
			if (Player.Instance.pAttack.Bombs <= 0 || Player.Instance.pAttack.Bullets <= 4) {
				HUDisplay.Instance.DisplayInfo("You are not a worthy opponent!\n"+
												   "Bullets: " + Player.Instance.pAttack.Bullets +"/5\n"+
												   "Bombs: "+ Player.Instance.pAttack.Bombs + "/1\n"+
												   "Return to me once you have everything... to meet your demise!\n" +
												   "HAHaHaa!!!", "Explore this location further.");
			}

		}
	}

	private void CamFadeOut_OnCamFullyFaded() {
		BossTransition();
	}

	private void BossTransition() {
		usedIndicator.SetActive(true);
		RectTransform bossBG = MapData.Instance.GetBackgroundBoss(1);
		MusicHandler.script.TransitionMusic(MusicHandler.script.room_1_boss);
		GameObject spawnedBoss = Instantiate(boss, bossBG.transform.position - new Vector3(0, bossBG.sizeDelta.y / 3), Quaternion.identity);
		spawnedBoss.name = "Boss";
		PlayerMovement.SpeedMultiplier = 5;
		GameObject health = Instantiate(bossHP, HPHolder.transform.position, Quaternion.identity, HPHolder.transform);
		health.name = "BossHealth";
		StartCoroutine(CameraMovement.Instance.LerpSize(CameraMovement.DEFAULT_CAM_SIZE, bossBG.sizeDelta.x * Screen.height / Screen.width * 0.5f, 0.15f, new Vector3(bossBG.position.x, bossBG.position.y, -10)));
		bossHP.SetActive(true);
		Control.Instance.saveManager.canSave = false;
		PlayerMovement.CanMove = false;
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
	}
}
