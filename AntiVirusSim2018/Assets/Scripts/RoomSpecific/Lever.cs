using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {
	public Sprite toggleOnSpr;
	public Sprite toggleOffSpr;

	public AudioClip toggleOn;
	public AudioClip toggleOff;

	private SpriteRenderer selfRender;

	public delegate void LeverState(Lever sender, bool isOn);
	public event LeverState OnLeverSwitch;

	public bool isOn { get; set; } = false;

	void Start() {
		selfRender = GetComponent<SpriteRenderer>();
	}

	public void Interact() {
		isOn = !isOn;
		if (OnLeverSwitch != null) {
			OnLeverSwitch(this, isOn);
		}
		selfRender.sprite = isOn ? toggleOnSpr : toggleOffSpr;
		SoundFXHandler.script.PlayFX(isOn ? toggleOn : toggleOff);
	}
}
