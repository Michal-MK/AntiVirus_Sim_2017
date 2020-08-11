using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {
	public Sprite toggleOnSpr;
	public Sprite toggleOffSpr;

	public AudioClip toggleOn;
	public AudioClip toggleOff;

	private SpriteRenderer selfRender;

	public event LeverState OnLeverSwitch;

	public bool IsOn { get; set; }

	void Start() {
		selfRender = GetComponent<SpriteRenderer>();
	}

	public void Interact() {
		IsOn = !IsOn;
		OnLeverSwitch?.Invoke(this, IsOn);
		selfRender.sprite = IsOn ? toggleOnSpr : toggleOffSpr;
		SoundFXHandler.script.PlayFX(IsOn ? toggleOn : toggleOff);
	}
}
