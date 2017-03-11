using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Canvas_Renderer : MonoBehaviour {
	
	public Text info_F;
	public Text info_S;
	public Text SpikeC;
	public Animator Spikec;
	public Text CoinC;
	public Animator Coinc;
	public static Canvas_Renderer script;
	Animator Front;
	Animator Slide;
	string infotext;

	bool st = true;
	bool isRunning = false;

	void Awake(){
		script = this;
	}

	private void Start() {
		Front = info_F.gameObject.GetComponent<Animator>();
		Slide = info_S.gameObject.GetComponent<Animator>();
	}
	
	public void infoRenderer(string text) {
		info_F.text = text;
		infotext = text;
		if (isRunning == false) {
			StartCoroutine("MoveWithText");
		}
		else {
			return;
		}
	}
	
	public IEnumerator MoveWithText() {
		isRunning = true;
		yield return new WaitForSecondsRealtime(0.2f);
		if (st != true) {
			Slide.SetTrigger("Slideout");
		}
		yield return new WaitForSecondsRealtime(1.1f);
		Front.Play("Appear");
		yield return new WaitForSecondsRealtime(2.1f);
		info_S.text = infotext;
		Slide.Play("SlideIN");
		yield return new WaitForSecondsRealtime(0.75f);
		st = false;
		isRunning = false;
		StopCoroutine("MoveWithText");
	}






	public GameObject up;
	public GameObject down;
	public GameObject left;
	public GameObject right;


	public void DisplayDirection(int i) {
		if(i == 1) {
			StartCoroutine("Pulse", right);
		}
		if (i == 2) {
			StartCoroutine("Pulse", up);
		}
		if (i == 3) {
			StartCoroutine("Pulse", down);
		}
		if (i == 4) {
			StartCoroutine("Pulse", right);
		}

	}


	public IEnumerator Pulse(GameObject info) {
		info.SetActive(true);
		yield return new WaitForSecondsRealtime(1);
		info.SetActive(false);
		yield return new WaitForSecondsRealtime(1);
		info.SetActive(true);
		yield return new WaitForSecondsRealtime(1);
		info.SetActive(false);
		yield return new WaitForSecondsRealtime(1);
		info.SetActive(true);
		yield return new WaitForSecondsRealtime(1);
		info.SetActive(false);

		StopCoroutine("Pulse");
	}


	public void Counters(string name) {

		if (name == "Coin") {
			CoinC.text = "x " + (Coins.coinsCollected + 1);

			if (Coins.coinsCollected == 4) {
				CoinC.transform.localPosition = CoinC.transform.localPosition + new Vector3(50, 0, 0);
				CoinC.text = CoinC.text + " Completed!";
			}
		}
		if(name == "Spike") {
			SpikeC.text = "x " + (Spike.spikesCollected);
		}
	}
}


