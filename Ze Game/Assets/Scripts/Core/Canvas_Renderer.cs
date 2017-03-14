using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Canvas_Renderer : MonoBehaviour {

	public Text info_S;
	Animator Slide;


	public Animator InfoPanel;
	public Image InfoPanelImg;
	public Text InfoPanelText;

	public Animator Spikec;
	public Animator Coinc;

	public Text CoinC;
	public Text SpikeC;

	public GameObject[] directions = new GameObject[4];

	public static Canvas_Renderer script;


	private bool first = true;
	public bool isRunning = false;
	private string tempDisplayedText;
	private Color32 defaultColor;
	void Awake() {
		script = this;
	}

	private void Start() {
		Slide = info_S.gameObject.GetComponent<Animator>();
		defaultColor = InfoPanelImg.color;
	}

	public void infoRenderer(string displayedTextPanel, string displayedTextSide, Color32? textColor = null) {
		print(displayedTextSide);
		if (textColor != null) {
			InfoPanelImg.color = (Color32)textColor;
		}
		else {
			InfoPanelImg.color = defaultColor;
		}

		tempDisplayedText = displayedTextSide;
		if (displayedTextPanel != null) {
			InfoPanelText.text = displayedTextPanel;
			InfoPanel.SetTrigger("Down");
			isRunning = true;
			Time.timeScale = 0;
		}
		else if (displayedTextPanel == null && displayedTextSide != null) {
			StartCoroutine(SlideInfo());
		}
	}

	private void Update() {
		if (isRunning && Input.GetKeyDown(KeyCode.Return)) {
			if (first) {
				AudioHandler.script.MusicTransition(AudioHandler.script.room1);
				first = false;
			}
			InfoPanel.SetTrigger("Up");
			isRunning = false;
			Time.timeScale = 1;
			StartCoroutine(SlideInfo());
		}
	}
	private IEnumerator SlideInfo() {
		if (!info_S.text.StartsWith("DEFAULT")) {
			Slide.SetTrigger("SlideOut");
		}
		yield return new WaitForSeconds(1);
		info_S.text = tempDisplayedText;
		Slide.SetTrigger("SlideIn");
	}


	public void DisplayDirection(int i) {
		StartCoroutine("Pulse", directions[i]);
	}


	public IEnumerator Pulse(GameObject info) {
		for (int i = 0; i < 3; i++) {
			info.SetActive(true);
			yield return new WaitForSecondsRealtime(1);
			info.SetActive(false);
			yield return new WaitForSecondsRealtime(1);
		}
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
		if (name == "Spike") {
			SpikeC.text = "x " + (Spike.spikesCollected);
		}
	}
}


