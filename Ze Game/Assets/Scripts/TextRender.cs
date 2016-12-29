using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextRender : MonoBehaviour {
	
	public Text textToScreen;
	public Canvas canvas;


	void OnTriggerEnter2D(Collider2D col){

		if (Spike.i == 0) {
			textToScreen.text = "You have not touched the spike yet!";
		}
		if (Spike.i == 1) {
			textToScreen.text = "You have touched the spike once!";
		}
		if (Spike.i == 2) {
			textToScreen.text = "You have touched the spike twice!";
		}




		if (Spike.i == 3 || Spike.i == 4) {
			textToScreen.text = "You have touched the spike " + Spike.i + " times!";
		}

		if (Spike.i == 5) {
			textToScreen.transform.position = new Vector3 (canvas.transform.position.x,canvas.transform.position.y+ 100, 0);
			textToScreen.text = "The Spike is gone!";
			textToScreen.fontSize = 52;
			textToScreen.alignment = TextAnchor.MiddleCenter;
		}
	} 
}


