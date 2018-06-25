using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {
	public Sprite toggleOnSpr;
	public Sprite toggleOffSpr;

	public AudioClip toggleOn;
	public AudioClip toggleOff;

	private SpriteRenderer selfRender;
	private AudioSource selfSound;

	public delegate void LeverState(Lever sender, bool isOn);
	public event LeverState OnLeverSwitch;

	private bool _isOn = false;

	void Start() {
		selfRender = GetComponent<SpriteRenderer>();
		selfSound = GetComponent<AudioSource>();
	}

	public void Interact() {
		_isOn = !_isOn;
		if (OnLeverSwitch != null) {
			OnLeverSwitch(this, _isOn);
		}
		selfRender.sprite = _isOn ? toggleOnSpr : toggleOffSpr;
		selfSound.clip = _isOn ? toggleOn : toggleOff;
		selfSound.Play();
	}

	public bool isOn {
		get { return _isOn; }
		set { _isOn = value; }
	}
}
