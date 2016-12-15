using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class displayScore : MonoBehaviour {
	public GameObject textWScore;
	public RectTransform canvas;
	GameObject textField;
	GameObject[] results = new GameObject[10];

	public void Start() {
		
		for (int i = 0; i <= 9; i++) {
			
			if (PlayerPrefs.GetFloat (i.ToString ()) != 500f && PlayerPrefs.GetFloat (i.ToString ()) != 0f) {
				results[i] = (GameObject)Instantiate (textWScore, new Vector3 (textWScore.transform.position.x,textWScore.transform.position.x - i * 40, 0), Quaternion.identity);
				results[i].transform.SetParent(transform);
				results[i].GetComponent<Text>().text = (i + 1) + "\t\t\t\t\t" + PlayerPrefs.GetFloat (i.ToString ()) + " s  ";

				if (i == 9) {
					results[i].GetComponent<Text>().text = "" +""+ (i + 1) + "\t\t" +""+ PlayerPrefs.GetFloat (i.ToString ()) + " s";
				
				}
				results [i].transform.localScale = new Vector3 (1, 1, 1);

			} else {
				results[i] = (GameObject)Instantiate (textWScore, new Vector3 (textWScore.transform.position.x,textWScore.transform.position.x - i * 40, 0), Quaternion.identity);
				results[i].transform.SetParent(transform);
				results[i].GetComponent<Text>().text = "" +"  "+ (i + 1) + "\t\t" + "No time found.";
				if (i == 9) {
					results[i].GetComponent<Text>().text = "" +""+ (i + 1) + "\t\t" +""+ "No time found.";

				}
				results [i].transform.localScale = new Vector3 (1, 1, 1);
			}
		}	
	}
	public void clearScores(){
		foreach (GameObject delResults in results) {
			Destroy (delResults);
		
		}
	}
}

