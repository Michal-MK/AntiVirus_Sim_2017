using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextRender : MonoBehaviour {
	
	public Spike score;
	public Text textToScreen;


	void OnTriggerEnter2D(Collider2D col){
//		Debug.Log ("Touched");

		if (col.name == "Asset" && score.i >= 0 && score.i <= 4) {
//			Debug.Log ("You have touched the spike " + score.i + " times");
			textToScreen.text = "You have touched the spike " + score.i + " times";
		}

		if (score.i == 5) {
//			Debug.Log ("You have touched the spike " + score.i + " times");
			textToScreen.text = "The Spike is gone";
			textToScreen.fontSize = 52;
		}
	} 
}


