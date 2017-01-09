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
			textToScreen.text = "You have touched the spike once!";
		}
		if (currentStage == 2) {
			textToScreen.text = "You have touched the spike twice!";
		}
		if (currentStage == 3 || currentStage == 4) {
			textToScreen.text = "You have touched the spike " + Spike.i + " times!";
		}

	}
	public void stageComplete(){
		
		textToScreen.transform.position = new Vector3 (canvas.transform.position.x,canvas.transform.position.y+ 170, 0);
		textToScreen.text = "The Spike is gone! " + "Find the teleporter.";
		textToScreen.fontSize = 35;
		textToScreen.alignment = TextAnchor.MiddleCenter;
	}
	public void Disable(){
		textToScreen.gameObject.SetActive (false);
	}
}


