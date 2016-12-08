using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class displayScore : MonoBehaviour {
	public GameObject textWScore;


	public void Start() {


		for (int i = 0; i <= 9; i++) {
			
			if (PlayerPrefs.GetFloat (i.ToString ()) != 500f && PlayerPrefs.GetFloat (i.ToString ()) != 0f) {

				GameObject textField = (GameObject)Instantiate (textWScore, new Vector3 (10, 0 + 92 - i * 20, 0), Quaternion.identity);
				textField.transform.parent = transform.parent;
				textField.GetComponent<Text> ().text = "No. " + (i + 1) + "\t \t \t \t" + PlayerPrefs.GetFloat (i.ToString ()) + " s";
			
			} else {
				GameObject textField = (GameObject)Instantiate (textWScore, new Vector3 (10, 0 + 92 - i * 20, 0), Quaternion.identity);
				textField.transform.parent = transform.parent;
				textField.GetComponent<Text> ().text = "No. " + (i + 1) + "\t \t \t \t" + "No score found.";
			
			
			
			
			}
		}	
	}
}

