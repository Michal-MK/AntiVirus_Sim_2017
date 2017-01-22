using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

	public Sprite sprtOn;
	public Sprite sprtOff;
	SpriteRenderer PPIndicator;
	public Spike spike;


	void Start(){
		PPIndicator = GameObject.Find ("PP_Indicator 1").GetComponent <SpriteRenderer> ();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player" || col.name == "Block") {
			PPIndicator.sprite = sprtOn;
			spike.SetPosition();
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if (col.tag == "Player" || col.name == "Block") {
				PPIndicator.sprite = sprtOff;
			
		}
	}
}
