using Igor.Constants.Strings;
using Igor.Conversions;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
	public bool stopEverything = false;

	private bool t = false;
	private bool r = false;
	private bool b = false;
	private bool l = false;
	private bool once = true;

	private bool stop = false;

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

		if (it.transform.name == "Bullet") {
			//print("With " + with.name);
			it.gameObject.SetActive(false);
			healthIndicator.value--;
			RaiseShields(with.name.ToDirection());
			for (int i = 0; i < behaviour.spikeHitboxes.Length; i++) {
				behaviour.spikeHitboxes[i].enabled = false;
			}
			behaviour.selfRender.sprite = behaviour.Invincible;
		}
		if (healthIndicator.value == 0 && !stop) {
			StartCoroutine(Death());
		}

	}
	public void CheckShields() {
		if (t && r && b && l && once) {
			Canvas_Renderer.script.InfoRenderer("His shields are up ... but we got a bomb!\n " +
												"Switch to it in Attack mode by pressing \"Right Mouse Button\"",
												"Pressing it again will switch your ammo back to bullets");
			once = false;
		}
	}

	public void RaiseShields(Directions where) {
		switch (where) {
			case Directions.TOP:
			ShieldT.SetActive(true);
			t = true;
			break;

			case Directions.RIGHT:
			ShieldR.SetActive(true);
			r = true;
			break;


			case Directions.BOTTOM:
			ShieldB.SetActive(true);
			b = true;
			break;

			case Directions.LEFT:
			ShieldL.SetActive(true);
			l = true;
			break;
		}
	}




	public IEnumerator Death() {

		stopEverything = true;
		stop = true;
		GameObject boss = behaviour.gameObject;


		boss.GetComponent<Animator>().StopPlayback();
		boss.GetComponent<Animator>().enabled = false;
		boss.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		boss.transform.position = new Vector3(0, 0, 10);
		behaviour.enabled = false;

		Canvas_Renderer.script.InfoRenderer("You did it! \n Your time has been saved to the leadreboard. \n Thank you for playing the game.", null);
		FindObjectOfType<M_Player>().FloorComplete();
		MusicHandler.script.TrasnsitionMusic(null);

		yield return new WaitForSeconds(3);
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1);
		yield return new WaitForSeconds(CamFadeOut.CAM_FULLY_FADED_NORMAL);
		SceneManager.LoadScene(SceneNames.CREDITS_SCENE);
	}
}
