using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour {
	public GameObject heart;
	public SpriteRenderer sprite;
	public Slider theSlider;
	public PlayerAttack atk;

	public bool invincible = false;
	bool stop = false;

	private Color32 inv;

	public GameObject ShieldT;
	public GameObject ShieldR;
	public GameObject ShieldB;
	public GameObject ShieldL;
	public bool stopEverything = false;

	private bool t = false;
	private bool r = false;
	private bool b = false;
	private bool l = false;



	void Start() {
		inv = new Color32(255, 255, 255, 100);
		theSlider = GameObject.Find("BossHealth").GetComponent<Slider>();
		atk = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
		theSlider.gameObject.SetActive(true);
		theSlider.value = 5;
	}



	void Update() {
		if (invincible) {
			sprite.color = inv;

		}
		else {
			sprite.color = new Color32(255, 255, 255, 255);

		}
	}

	public void Collided(Collision2D it, GameObject with) {

		if (it.transform.name == "Bullet" && !invincible) {
			print("With " + with.name);
			it.gameObject.SetActive(false);
			theSlider.value--;
			invincible = true;
			StartCoroutine(RaiseShields(with.name));
			}
		if (theSlider.value == 0 && !stop) {
			StartCoroutine(Death());
			stop = true;
		}

	}

	public IEnumerator RaiseShields(string where) {


		yield return new WaitUntil(() => invincible == false);

		if(t && r && b && l) {
			Canvas_Renderer.script.infoRenderer("His shields are up ... but we got a bomb! Switch to it in Attack mode by pressing Right M.B.");
			atk.canSwitch = true;
		}

		switch (where) {
			case "Top":
			ShieldT.SetActive(true);
			t = true;
			yield break;

			case "Right":
			ShieldR.SetActive(true);
			r = true;
			yield break;


			case "Bottom":
			ShieldB.SetActive(true);
			b = true;
			yield break;

			case "Left":
			ShieldL.SetActive(true);
			l = true;
			yield break;
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
		
		Canvas_Renderer.script.infoRenderer("You did it!");
		yield return new WaitForSecondsRealtime(5);
		print("Test2");
		Canvas_Renderer.script.infoRenderer("Your time has been saved to the leadreboard.");
		M_Player mp = GameObject.FindGameObjectWithTag("Player").GetComponent<M_Player>();
		mp.FloorComplete();
		yield return new WaitForSecondsRealtime(5);
		print("Test3");
		Canvas_Renderer.script.infoRenderer("Thank you for playing the game.");
		yield return new WaitForSecondsRealtime(5);
		GameObject.Find("TransitionCam").GetComponent<Animator>().Play("CamTransition");

		print("Test5");
		yield return new WaitForSecondsRealtime(2);
		SceneManager.LoadScene(3);
		
	}



}
