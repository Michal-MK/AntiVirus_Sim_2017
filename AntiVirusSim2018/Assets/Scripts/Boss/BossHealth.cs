using Igor.Constants.Strings;
using Igor.Conversions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour {

	public BossBehaviour behaviour;

	//Prefab
	public GameObject bossHealth;
	public ParticleSystem deathParticles;

	private ParticleSystem instantiatedParticles;
	public Slider healthIndicator;

	public GameObject ShieldT;
	public GameObject ShieldR;
	public GameObject ShieldB;
	public GameObject ShieldL;

	public Vector3 tpLocation;

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
		//healthIndicator.value = 1;
		//print("Debug");
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
		M_Player.gameProgression = 10;

		behaviour.selfAnim.enabled = false;
		behaviour.selfRigid.velocity = Vector2.zero;
		behaviour.enabled = false;
		behaviour.StopAllCoroutines();

		instantiatedParticles = Instantiate(deathParticles.gameObject, transform.position, transform.rotation).GetComponent<ParticleSystem>();
		GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(5);
		foreach (SpriteRenderer sprR in transform.parent.GetComponentsInChildren<SpriteRenderer>()) {
			for (float f = 0; f <= 1; f += 0.1f) {
				sprR.color = new Color(1, 1, 1, 1 - f);
				yield return null;
			}
		}
		//print("Stopping emmission");
		ParticleSystem.EmissionModule e = instantiatedParticles.emission;
		e.enabled = false;
		yield return new WaitForSeconds(1);
		GetComponent<AudioSource>().loop = false;
		yield return new WaitForSeconds(1);

		//print("One s to destruction.");
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1.2f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
		Destroy(instantiatedParticles.gameObject);
	}

	private void CamFadeOut_OnCamFullyFaded() {
		MapData.script.Progress(M_Player.gameProgression);
		MusicHandler.script.TransitionMusic(MusicHandler.script.room1_1);

		M_Player.player.transform.position = tpLocation + new Vector3(0, 10);
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
		CameraMovement.script.inBossRoom = false;
		Camera.main.orthographicSize = CameraMovement.defaultCamSize;
		Control.script.saveManager.canSave = true;
		Destroy(transform.parent.gameObject);
		Destroy(healthIndicator.gameObject);
		Canvas_Renderer.script.DisplayInfoDelayed("Great job, lets perform a quick scan to see if we resolved the problem.", "Initiating...", 2);
	}
}
