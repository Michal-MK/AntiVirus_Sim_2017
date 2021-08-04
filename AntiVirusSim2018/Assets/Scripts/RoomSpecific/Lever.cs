using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {
	[SerializeField]
	private Sprite toggleOnSpr = null;
	[SerializeField]
	private Sprite toggleOffSpr = null;
	[SerializeField]
	private AudioClip toggleOn = null;
	[SerializeField]
	private AudioClip toggleOff = null;
	[SerializeField]
	private SpriteRenderer selfRender = null;

	public event LeverState OnLeverSwitch;

	public bool IsOn { get; set; }

	public void Interact() {
		IsOn = !IsOn;
		OnLeverSwitch?.Invoke(this, IsOn);
		selfRender.sprite = IsOn ? toggleOnSpr : toggleOffSpr;
		SoundFXHandler.script.PlayFX(IsOn ? toggleOn : toggleOff);
	}
}
