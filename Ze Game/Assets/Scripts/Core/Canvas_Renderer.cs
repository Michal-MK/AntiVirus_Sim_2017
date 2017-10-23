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

	public bool isRunning = false;
	private string tempDisplayedText;
	private Color32 defaultColor;

	private void Awake() {
		Statics.canvasRenderer = this;
		defaultColor = new Color32(255, 255, 255, 100);
	}

	private void Start() {
		Slide = info_S.gameObject.GetComponent<Animator>();
		defaultColor = InfoPanelImg.color;
	}

	public void infoRenderer(string displayedTextPanel, string displayedTextSide, Color32? color = null) {
		if (isRunning) {
			StartCoroutine(ReturnLater(displayedTextPanel,displayedTextSide,color));
			return;
		}
		//print(displayedTextSide);
		if (color != null) {
			InfoPanelImg.color = (Color32)color;
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
	
	private IEnumerator ReturnLater(string s,string ss, Color32? color = null) {
		yield return new WaitWhile(() => isRunning == true);
		infoRenderer(s, ss, color);
	}

	private void Update() {
		if (isRunning && Input.GetButtonDown("Submit")) {
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
		Wrapper wrp = GameObject.Find("Collectibles").GetComponent<Wrapper>();
		if (!wrp.Objects[0].activeInHierarchy) {
			StartCoroutine("Pulse", directions[i]);
		}
		else {
			Statics.guide.Recalculate(wrp.Objects[0].gameObject, true);
		}
	}



	private IEnumerator Pulse(GameObject info) {
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
			CoinC.text = "x " + Coins.coinsCollected;

			if (Coins.coinsCollected == 5) {
				CoinC.transform.localPosition = CoinC.transform.localPosition + new Vector3(50, 0, 0);
				CoinC.text = CoinC.text + " Completed!";
			}
		}
		if (name == "Spike") {
			SpikeC.text = "x " + (Spike.spikesCollected);
		}
		if (name == "Update") {
			CoinC.text = "x " + Coins.coinsCollected;
			SpikeC.text = "x " + Spike.spikesCollected;
		}
	}
	private void OnDestroy() {
		Statics.canvasRenderer = null;
	}
}


