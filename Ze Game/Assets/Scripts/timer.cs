using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class timer : MonoBehaviour {
	public static float time_er;
	Text text;
	public static bool run;
	public GameObject FYT;
	public GameObject Timer_text;

	void Start () {
		time_er = 0f;
		text = this.GetComponent<Text> ();
		FYT.SetActive(false);

	}


	void FixedUpdate () {

		if (run == true) {
				FYT.SetActive(true);
				time_er = time_er + Time.fixedDeltaTime;
				text.text = "Time: " + (Mathf.Round(time_er * 100) / 100);		

			if (time_er >= 0 && time_er <=9.95) {
				FYT.gameObject.transform.position =  new Vector3(transform.position.x + 30,transform.position.y,0);
			}
		
			if (time_er >= 10 && time_er <= 10.02) {
				FYT.gameObject.transform.position =  new Vector3(transform.position.x + 45,transform.position.y,0);
			}
			if (time_er >= 60 && time_er <= 60.02) {
				run = false;
				FYT.SetActive (false);
//				Vector3 textpos = new Vector3 (text.transform.position.x, text.transform.position.y, 0);
//				GameObject timetext = (GameObject)Instantiate (Timer_text, new Vector3 (textpos.x, textpos.y, 0), Quaternion.identity);
				text.text = "Are you even trying ?";


	
			}
		}	
	}
}