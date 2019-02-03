using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Igor.Constants.Strings;

public class Canvas_Renderer : MonoBehaviour {

	public static Canvas_Renderer script;

	public Text slideInText;
	private Animator slideAnim;

	public Text infoPanelText;
	private Animator infoPanelAnim;

	public Text coinCounter;
	public Text spikeCounter;

	private GameObject topDirectionArrows;
	private GameObject rightDirectionArrows;
	private GameObject bottomDirectionArrows;
	private GameObject leftDirectionArrows;

	public bool isRunning = false;

	private string tempDisplayedText;

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
		slideAnim = slideInText.GetComponent<Animator>();
		infoPanelAnim = infoPanelText.transform.parent.GetComponent<Animator>();
		infoPanelText = infoPanelText.GetComponentInChildren<Text>();

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
			infoPanelAnim.SetTrigger("Down");
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
		infoPanelAnim.SetTrigger("Up");
		isRunning = false;
		Time.timeScale = 1;
		StartCoroutine(SlideInfo(tempDisplayedText));
		StopCoroutine(displayingInfoRoutine);
		displayingInfoRoutine = null;
	}

	private IEnumerator SlideInfo(string textToDisplay) {
		slideAnim.SetTrigger("SlideOut");
		yield return new WaitForSeconds(1);
		slideInText.text = textToDisplay;
		slideAnim.SetTrigger("SlideIn");
	}

	private IEnumerator ReplaceText(string newText) {
		yield return new WaitForSecondsRealtime(.15f);
		infoPanelText.text = newText;
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


