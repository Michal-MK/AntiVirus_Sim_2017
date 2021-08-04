using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Igor.Constants.Strings;
using System;

public class HUDisplay : MonoBehaviour {

	public static HUDisplay Instance { get; private set; }

	[SerializeField]
	private Text coinCounter = null;
	[SerializeField]
	private Text spikeCounter = null;
	[SerializeField]
	private Text infoPanelText = null;
	[SerializeField]
	private Text slideInText = null;
	public string SlideInText => slideInText.text;
	[SerializeField]
	private bool isRunning = false;
	public bool IsRunning => isRunning;


	private GameObject topDirectionArrows;
	private GameObject rightDirectionArrows;
	private GameObject bottomDirectionArrows;
	private GameObject leftDirectionArrows;

	private Animator infoPanelAnim;
	private Animator slideAnim;

	private string tempDisplayedText;

	private Coroutine displayingInfoRoutine;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		}
		else if (Instance != this) {
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
			infoPanelText.text = displayedTextMain;
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

	public void UpdateSlideTextDirect(string text) {
		slideInText.text = text;
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

	public void UpdateSpikeCounter(int value) {
		spikeCounter.text = $"x {value}";
	}


	public void UpdateCoinCounter(int value) {
		coinCounter.text = $"x {value}";

		if (value == 5) {
			coinCounter.text += " Completed?";
		}
	}

	private void OnDestroy() {
		Instance = null;
		LoadManager.OnSaveDataLoaded -= LoadManager_OnSaveDataLoaded;
	}
}


