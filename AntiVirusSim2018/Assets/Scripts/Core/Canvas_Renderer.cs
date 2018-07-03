using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Igor.Constants.Strings;

public class Canvas_Renderer : MonoBehaviour {

	public Text slideInInfo;
	private Animator slideAnim;

	public GameObject infoPanel;
	private Animator infoPanel_anim;
	private Text infoPanel_Text;

	public Text coinCounter;
	public Text spikeCounter;

	private GameObject topDirectionArrows;
	private GameObject rightDirectionArrows;
	private GameObject bottomDirectionArrows;
	private GameObject leftDirectionArrows;

	public bool isRunning = false;

	private string tempDisplayedText;

	public static Canvas_Renderer script;

	private Coroutine displayingInfoRoutine;

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(gameObject);
		}
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		Camera.main.orthographicSize = data.core.camSize;
		DisplayInfo(null, data.shownHints.currentlyDisplayedSideInfo);
	}

	private void Start() {
		slideAnim = slideInInfo.GetComponent<Animator>();
		infoPanel_anim = infoPanel.GetComponent<Animator>();
		infoPanel_Text = infoPanel.GetComponentInChildren<Text>();

		Transform dirArrows = transform.Find("_DirectionArrows");
		topDirectionArrows = dirArrows.Find("Up").gameObject;
		rightDirectionArrows = dirArrows.Find("Right").gameObject;
		bottomDirectionArrows = dirArrows.Find("Down").gameObject;
		leftDirectionArrows = dirArrows.Find("Left").gameObject;
	}

	public void DisplayInfoDelayed(string mainInfo, string secondaryInfo, float seconds) {
		StartCoroutine(_Delay(mainInfo, secondaryInfo, seconds));
	}

	private IEnumerator _Delay(string mainInfo, string secondaryInfo, float seconds) {
		yield return new WaitForSeconds(seconds);
		DisplayInfo(mainInfo, secondaryInfo);
	}

	public void DisplayInfo(string displayedTextMain, string displayedTextSide) {
		if (isRunning) {
			StartCoroutine(RetryLater(displayedTextMain, displayedTextSide));
			return;
		}

		tempDisplayedText = displayedTextSide;

		if (displayedTextMain != null) {
			StartCoroutine(ReplaceText(displayedTextMain));
			infoPanel_anim.SetTrigger("Down");
			isRunning = true;
			Time.timeScale = 0;
			displayingInfoRoutine = StartCoroutine(ResumeFromInfoPanel());
		}
		else if (displayedTextMain == null && displayedTextSide != null) {
			StartCoroutine(SlideInfo(displayedTextSide));
		}
	}

	private IEnumerator RetryLater(string main, string slide) {
		yield return new WaitWhile(() => isRunning == true);
		DisplayInfo(main, slide);
	}

	private IEnumerator ResumeFromInfoPanel() {
		yield return new WaitUntil(() => Input.GetButtonDown(InputNames.SUBMIT));
		yield return null; // So we don't run though them all at the same time.
		infoPanel_anim.SetTrigger("Up");
		isRunning = false;
		Time.timeScale = 1;
		StartCoroutine(SlideInfo(tempDisplayedText));
		StopCoroutine(displayingInfoRoutine);
		displayingInfoRoutine = null;
	}

	private IEnumerator SlideInfo(string textToDisplay) {
		slideAnim.SetTrigger("SlideOut");
		yield return new WaitForSeconds(1);
		slideInInfo.text = textToDisplay;
		slideAnim.SetTrigger("SlideIn");
	}

	private IEnumerator ReplaceText(string newText) {
		yield return new WaitForSecondsRealtime(.15f);
		infoPanel_Text.text = newText;
	}

	public void DisplayDirection(Directions dir) {
		switch (dir) {
			case Directions.TOP: {
				StartCoroutine(Pulse(topDirectionArrows));
				break;
			}
			case Directions.RIGHT: {
				StartCoroutine(Pulse(rightDirectionArrows));
				break;
			}
			case Directions.BOTTOM: {
				StartCoroutine(Pulse(bottomDirectionArrows));
				break;
			}
			case Directions.LEFT: {
				StartCoroutine(Pulse(leftDirectionArrows));
				break;
			}
		}
	}

	private IEnumerator Pulse(GameObject info) {
		for (int i = 0; i < 3; i++) {
			info.SetActive(true);
			yield return new WaitForSecondsRealtime(1);
			info.SetActive(false);
			yield return new WaitForSecondsRealtime(1);
		}
	}

	public void UpdateCounters() {
		coinCounter.text = "x " + Coin.coinsCollected;
		spikeCounter.text = "x " + Spike.spikesCollected;

		if (Coin.coinsCollected == 5) {
			coinCounter.text = coinCounter.text + " Completed?";
		}
	}

	private void OnDestroy() {
		script = null;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}


