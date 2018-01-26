using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {
	public Sprite ON;
	public Sprite OFF;
	public GameObject intercationIndicator;

	private SpriteRenderer selfRender;

	public delegate void LeverState(Lever sender, bool isOn);
	public event LeverState OnLeverSwitch;

	private bool awaitingInput = false;
	private Coroutine routine;
	private bool _isOn = false;

	void Start() {
		selfRender = GetComponent<SpriteRenderer>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.transform.name == "Player") {
			awaitingInput = true;
			intercationIndicator.SetActive(true);
			routine = StartCoroutine(Interaction());
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.transform.name == "Player") {
			awaitingInput = false;
			intercationIndicator.SetActive(false);
			StopCoroutine(routine);
		}
	}

	private IEnumerator Interaction() {
		while (awaitingInput) {
			yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
			_isOn = !_isOn;
			if (OnLeverSwitch != null) {
				OnLeverSwitch(this, _isOn);
			}
			selfRender.sprite = _isOn ? ON : OFF;
			yield return new WaitForSeconds(0.5f);
		}
	}

	public bool isOn {
		get { return _isOn; }
		set { _isOn = value; }
	}
}
