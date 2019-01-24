using Igor.Constants.Strings;
using Igor.Conversions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour {

	//Prefabs
	public GameObject bossHealth;
	public ParticleSystem deathParticles;

	public BossBehaviour behaviour;

	private ParticleSystem instantiatedParticles;

	public Slider healthIndicator;

	public GameObject ShieldT;
	public GameObject ShieldR;
	public GameObject ShieldB;
	public GameObject ShieldL;

	public Vector3 tpLocation;

	public AudioClip bossDeathExplosions;

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

	public void Collided(GameObject it, GameObject with) {

		if (it.name == ObjNames.BULLET) {
			it.SetActive(false);
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
		if (healthIndicator.value == 1 && once) {
			Canvas_Renderer.script.DisplayInfo("His shields are up ... but we got a bomb!\n " +
												"Switch to it in Attack mode by pressing \"Right Mouse Button\"", //TODO
												"Pressing it again will switch your ammo back to bullets");
			once = false;
		}
	}

	public void RaiseShields(Directions where) {
		switch (where) {
			case Directions.TOP: {
				ShieldT.SetActive(true);
				break;
			}
			case Directions.RIGHT: {
				ShieldR.SetActive(true);
				break;
			}
			case Directions.BOTTOM: {
				ShieldB.SetActive(true);
				break;
			}
			case Directions.LEFT: {
				ShieldL.SetActive(true);
				break;
			}
		}
	}

	public IEnumerator Death() {
		behaviour.selfAnim.enabled = false;
		behaviour.selfRigid.velocity = Vector2.zero;
		behaviour.StopAllCoroutines();
		behaviour.enabled = false;

		instantiatedParticles = Instantiate(deathParticles.gameObject, transform.position, transform.rotation).GetComponent<ParticleSystem>();

		SoundFXHandler.script.PlayFXLoop(bossDeathExplosions, 4);
		yield return new WaitForSeconds(4);
		foreach (SpriteRenderer sprR in transform.parent.GetComponentsInChildren<SpriteRenderer>()) {
			for (float f = 0; f <= 1; f += 0.1f) {
				sprR.color = new Color(1, 1, 1, 1 - f);
				yield return null;
			}
		}
		Destroy(instantiatedParticles.gameObject);

		yield return new WaitForSeconds(1);
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES, 1.2f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
	}

	private void CamFadeOut_OnCamFullyFaded() {
		MapData.script.Progress(M_Player.gameProgression = 10);

		MusicHandler.script.TransitionMusic(MusicHandler.script.room1_1);

		M_Player.player.transform.position = tpLocation + new Vector3(0, 10);
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
		CameraMovement.script.inBossRoom = false;
		Camera.main.orthographicSize = CameraMovement.defaultCamSize;
		Control.script.saveManager.canSave = true;
		Player_Movement.canMove = true;
		Destroy(transform.parent.gameObject);
		Destroy(healthIndicator.gameObject);
		Canvas_Renderer.script.DisplayInfoDelayed("Great job, lets perform a quick scan to see if we resolved the problem.", "Initiating...", 2);
	}
}
