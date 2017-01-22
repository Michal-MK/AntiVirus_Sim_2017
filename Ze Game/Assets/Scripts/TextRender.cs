using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextRender : MonoBehaviour {
	
	public Text textToScreen;
	public Canvas canvas;
	public static TextRender script;

	void Awake(){
		script = this;
	}


	public void infoRenderer (int currentStage){

		if (currentStage == 1) {
			textToScreen.text = "You have not touched the spike yet!";
		}
		if (currentStage == 1) {
			textToScreen.text = "You have touched the spike once out of 5!";
		}
		if (currentStage == 2) {
			textToScreen.text = "You have touched the spike twice out of 5!";
		}
		if (currentStage == 3 || currentStage == 4) {
			textToScreen.text = "You have touched the spike " + Spike.spikesCollected + " times out of 5!";
		}
		

	}
	public void stageComplete(){
		textToScreen.text = "The Spike is gone! " + "Find the teleporter.";
		//textToScreen.fontSize = 35;
	}

	public void Disable(){
		textToScreen.gameObject.SetActive (false);
	}
}


