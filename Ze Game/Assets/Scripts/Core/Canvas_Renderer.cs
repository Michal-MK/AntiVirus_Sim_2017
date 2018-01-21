using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Igor.Constants.Strings;

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
	public static Canvas_Renderer script;

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
		defaultColor = new Color32(255, 255, 255, 100);
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		Camera.main.orthographicSize = data.core.camSize;
		Canvas_Renderer.script.InfoRenderer(null, data.shownHints.currentlyDisplayedSideInfo);
	}

	private void Start() {
		Slide = info_S.gameObject.GetComponent<Animator>();
		defaultColor = InfoPanelImg.color;
	}

	public void InfoRenderer(string displayedTextMain, string displayedTextSide, Color32? color = null) {
		//If we are already displaying something, wait for it to finish, and try again then.
		if (isRunning) {
			StartCoroutine(RetryLater(displayedTextMain, displayedTextSide, color));
			return;
		}

		if (color != null) {
			InfoPanelImg.color = (Color32)color;
		}
		else {
			InfoPanelImg.color = defaultColor;
		}

		tempDisplayedText = displayedTextSide;
		if (displayedTextMain != null) {
			InfoPanelText.text = displayedTextMain;
			InfoPanel.SetTrigger("Down");
			isRunning = true;
			Time.timeScale = 0;
		}
		else if (displayedTextMain == null && displayedTextSide != null) {
			StartCoroutine(SlideInfo(displayedTextSide));
		}
	}

	private IEnumerator RetryLater(string main, string side, Color32? color = null) {
		yield return new WaitWhile(() => isRunning == true);
		InfoRenderer(main, side, color);
	}

	private void Update() {
		if (isRunning && Input.GetButtonDown("Submit")) {
			InfoPanel.SetTrigger("Up");
			isRunning = false;
			Time.timeScale = 1;
			StartCoroutine(SlideInfo(tempDisplayedText));
		}
	}
	private IEnumerator SlideInfo(string textToDisplay) {
		if (!info_S.text.StartsWith("DEFAULT")) {
			Slide.SetTrigger("SlideOut");
		}
		yield return new WaitForSeconds(1);
		info_S.text = textToDisplay;
		Slide.SetTrigger("SlideIn");
	}


	public void DisplayDirection(Directions dir) {
		StartCoroutine(Pulse(directions[(int)dir]));
	}

	private IEnumerator Pulse(GameObject info) {
		for (int i = 0; i < 3; i++) {
			info.SetActive(true);
			yield return new WaitForSecondsRealtime(1);
			info.SetActive(false);
			yield return new WaitForSecondsRealtime(1);
		}
	}

	public void UpdateCounters(string name = null) {

		if (name == ObjNames.COIN) {
			CoinC.text = "x " + Coin.coinsCollected;

			if (Coin.coinsCollected == 5) {
				CoinC.transform.localPosition += new Vector3(50, 0, 0);
				CoinC.text = CoinC.text + " Completed!";
			}
		}
		if (name == ObjNames.SPIKE) {
			SpikeC.text = "x " + (Spike.spikesCollected);
		}

		if (string.IsNullOrEmpty(name)) {
			UpdateCounters(ObjNames.SPIKE);
			UpdateCounters(ObjNames.COIN);
		}
	}
	private void OnDestroy() {
		script = null;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}
}


