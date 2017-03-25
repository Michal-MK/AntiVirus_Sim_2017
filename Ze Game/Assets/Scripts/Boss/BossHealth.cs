using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour {
	public GameObject heart;
	public SpriteRenderer sprite;
	public Slider theSlider;
	public PlayerAttack atk;

	

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

	bool stop = false;

	private void Awake() {
		Statics.bossHealth = this;
	}

	void Start() {
		theSlider = GameObject.Find("BossHealth").GetComponent<Slider>();
		atk = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
		theSlider.gameObject.SetActive(true);
		theSlider.value = 5;
	}

	public void Collided(Collision2D it, GameObject with) {

		if (it.transform.name == "Bullet") {
			print("With " + with.name);
			it.gameObject.SetActive(false);
			theSlider.value--;
			RaiseShields(with.name);
			for (int i = 0; i < Statics.bossBehaviour.spikeHitboxes.Length; i++) {
				Statics.bossBehaviour.spikeHitboxes[i].enabled = false;
			}
			Statics.bossBehaviour.selfRender.sprite = Statics.bossBehaviour.Invincible;
		}
		if (theSlider.value == 0 && !stop) {
			StartCoroutine(Death());
			stop = true;
		}

	}
	public void CheckShields() {
		print(t + " " + r + " " + b + " " + l);
		if (t && r && b && l && once) {
			Statics.canvasRenderer.infoRenderer("His shields are up ... but we got a bomb!\n " +
												"Switch to it in Attack mode by pressing \"Right Mouse Button\"",
												"Pressing it again will switch your ammo back to bullets");
			once = false;
		}
	}

	public void RaiseShields(string where) {

		switch (where) {
			case "TopHitbox":
			ShieldT.SetActive(true);
			t = true;
			break;

			case "RightHitbox":
			ShieldR.SetActive(true);
			r = true;
			break;


			case "BottomHitbox":
			ShieldB.SetActive(true);
			b = true;
			break;

			case "LeftHitbox":
			ShieldL.SetActive(true);
			l = true;
			break;
		}
	}



	public IEnumerator Death() {

		stopEverything = true;
		GameObject boss = GameObject.Find("Boss");


		boss.GetComponent<Animator>().StopPlayback();
		boss.GetComponent<Animator>().enabled = false;
		boss.GetComponent<BossBehaviour>().enabled = false;
		boss.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		boss.transform.position = new Vector3(0, 0, 10);
		
		Statics.canvasRenderer.infoRenderer("You did it! \n Your time has been saved to the leadreboard. \n Thank you for playing the game.", null);
		M_Player mp = GameObject.FindGameObjectWithTag("Player").GetComponent<M_Player>();
		mp.FloorComplete();
		timer.run = false;
		Statics.music.MusicTransition(null);
		yield return new WaitForSeconds(5);
		GameObject.Find("TransitionBlack").GetComponent<Animator>().Play("CamTransition");

		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(3);
	}

	private void OnDestroy() {
		Statics.bossHealth = null;
	}
}
