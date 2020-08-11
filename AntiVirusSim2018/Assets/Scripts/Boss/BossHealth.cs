using Igor.Constants.Strings;
using Igor.Conversions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour {

	public GameObject bossHealth;
	public ParticleSystem deathParticles;


	public BossBehaviour behaviour;

	private ParticleSystem instantiatedParticles;
	public Slider healthIndicator;

	public GameObject[] shields = new GameObject[4];
	public Collider2D[] bulletColliders = new Collider2D[4];

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
	}

	public void OnCollision(GameObject bullet, GameObject bulletHitbox) {

		if (bullet.name == ObjNames.BULLET) {
			bullet.SetActive(false);
			healthIndicator.value--;
			shields[(int)bulletHitbox.name.ToDirection()].SetActive(true);
			CheckShields();
			SetDamageable(false);
			behaviour.selfRender.sprite = behaviour.Invincible;
		}
		if (healthIndicator.value == 0) {
			Destroy(bulletHitbox);
			StartCoroutine(Death());
		}
	}

	public void SetDamageable(bool state) {
		for (int i = 0; i < bulletColliders.Length; i++) {
			bulletColliders[i].enabled = state;
		}
	}

	public void CheckShields() {
		if (healthIndicator.value == 1 && once) {
			HUDisplay.script.DisplayInfo("His shields are up ... but we got a bomb!\n " +
												"Switch to it in Attack mode by pressing \"Right Mouse Button\"", //TODO
												"Pressing it again will switch your ammo back to bullets");
			once = false;
		}
	}

	public IEnumerator Death() {
		behaviour.StopAllCoroutines();
		behaviour.enabled = false;

		instantiatedParticles = Instantiate(deathParticles.gameObject, transform.position, transform.rotation).GetComponent<ParticleSystem>();

		SoundFXHandler.script.PlayFXLoop(bossDeathExplosions, 4);
		yield return new WaitForSeconds(4);
		foreach (SpriteRenderer sprR in transform.parent.GetComponentsInChildren<SpriteRenderer>()) {
			for (float f = 0; f <= 1; f += 0.02f) { 
				sprR.color = new Color(1, 1, 1, 1 - f);
				yield return null;
			}
		}
		Destroy(instantiatedParticles.gameObject);

		yield return new WaitForSeconds(1);
		CamFadeOut.Instance.PlayTransition(CameraTransitionModes.TRANSITION_SCENES, 1.2f);
		CamFadeOut.OnCamFullyFaded += CamFadeOut_OnCamFullyFaded;
	}

	private void CamFadeOut_OnCamFullyFaded() {
		MapData.Instance.Progress(Player.GameProgression = 10);

		MusicHandler.script.TransitionMusic(MusicHandler.script.room1_1);

		Player.Instance.transform.position = FindObjectOfType<BossEntrance>().transform.position + new Vector3(0, 10);
		CamFadeOut.OnCamFullyFaded -= CamFadeOut_OnCamFullyFaded;
		CameraMovement.Instance.IsInBossRoom = false;
		Camera.main.orthographicSize = CameraMovement.DEFAULT_CAM_SIZE;
		Control.Instance.saveManager.canSave = true;
		PlayerMovement.CanMove = true;
		Destroy(transform.parent.gameObject);
		Destroy(healthIndicator.gameObject);
		HUDisplay.script.DisplayInfoDelayed("Great job, lets perform a quick scan to see if we resolved the problem.", "Initiating...", 2);
	}
}
