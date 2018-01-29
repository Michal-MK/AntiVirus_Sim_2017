using Igor.Constants.Strings;
using Igor.Conversions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour {

	public BossBehaviour behaviour;

	//Prefab
	public GameObject bossHealth;

	public Slider healthIndicator;

	public GameObject ShieldT;
	public GameObject ShieldR;
	public GameObject ShieldB;
	public GameObject ShieldL;

	private bool topShieldUp = false;
	private bool rightShieldUp = false;
	private bool bottomShieldUp = false;
	private bool leftShieldUp = false;
	private bool once = true;

	void Start() {
		if (GameObject.Find("BossHealth") != null) {
			healthIndicator = GameObject.Find("BossHealth").GetComponent<Slider>();
		}
		else {
			GameObject healhtObj = Instantiate(bossHealth,
				GameObject.Find(Boss.BOSS_HEALTH_PLACEHOLDER).transform.position,
				Quaternion.identity, GameObject.Find(Boss.BOSS_HEALTH_PLACEHOLDER).transform
			);
			healthIndicator = healhtObj.GetComponent<Slider>();
		}
		healthIndicator.gameObject.SetActive(true);
		healthIndicator.value = 5;
	}

	public void Collided(Collision2D it, GameObject with) {

		if (it.transform.name == ObjNames.BULLET) {
			it.gameObject.SetActive(false);
			healthIndicator.value--;
			RaiseShields(with.name.ToDirection());
			for (int i = 0; i < behaviour.spikeHitboxes.Length; i++) {
				behaviour.spikeHitboxes[i].enabled = false;
			}
			behaviour.selfRender.sprite = behaviour.Invincible;
		}
		if (healthIndicator.value == 0) {
			Destroy(with);
			StartCoroutine(Death());
		}
	}

	public void CheckShields() {
		if (topShieldUp && rightShieldUp && bottomShieldUp && leftShieldUp && once) {
			Canvas_Renderer.script.DisplayInfo("His shields are up ... but we got a bomb!\n " +
												"Switch to it in Attack mode by pressing \"Right Mouse Button\"",
												"Pressing it again will switch your ammo back to bullets");
			once = false;
		}
	}

	public void RaiseShields(Directions where) {
		switch (where) {
			case Directions.TOP:
			ShieldT.SetActive(true);
			topShieldUp = true;
			break;

			case Directions.RIGHT:
			ShieldR.SetActive(true);
			rightShieldUp = true;
			break;


			case Directions.BOTTOM:
			ShieldB.SetActive(true);
			bottomShieldUp = true;
			break;

			case Directions.LEFT:
			ShieldL.SetActive(true);
			leftShieldUp = true;
			break;
		}
	}

	public IEnumerator Death() {
		GameObject boss = behaviour.gameObject;
		boss.GetComponent<Animator>().StopPlayback();
		boss.GetComponent<Animator>().enabled = false;
		boss.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		boss.transform.position = new Vector3(0, 0, 10);
		behaviour.enabled = false;

		M_Player.gameProgression = 10;

		yield return new WaitForSeconds(3);

		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 0.8f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;


		/* Aternate Ending deprecated lul
		Canvas_Renderer.script.InfoRenderer("You did it! \n Your time has been saved to the leadreboard. \n Thank you for playing the game.", null);
		FindObjectOfType<M_Player>().FloorComplete();
		MusicHandler.script.TrasnsitionMusic(null);

		yield return new WaitForSeconds(3);
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1);
		yield return new WaitForSeconds(CamFadeOut.CAM_FULLY_FADED_NORMAL);
		SceneManager.LoadScene(SceneNames.CREDITS_SCENE);
		*/
	}

	private void CamFadeOut_OnCamFullyFaded() {
		MapData.script.Progress(M_Player.gameProgression);
		MusicHandler.script.TransitionMusic(MusicHandler.script.room1_1);

		M_Player.player.transform.position = new Vector3(302, -124, 0);
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
		CameraMovement.script.inBossRoom = false;
		Camera.main.orthographicSize = CameraMovement.defaultCamSize;
		SaveManager.canSave = true;
	}
}
