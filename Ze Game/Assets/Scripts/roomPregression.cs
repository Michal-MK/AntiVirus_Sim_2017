using UnityEngine;
using System.Collections;


public class roomPregression : MonoBehaviour {

	public GameObject[] doors;
	public Sprite sprtOff;
	public Sprite sprtOn;
	public static roomPregression script;

	SpriteRenderer ic_S;
	SpriteRenderer ic_1a;
	SpriteRenderer ic_1b;
	SpriteRenderer ic_2;
	SpriteRenderer ic_3;

	void Awake(){
		script = this;
	}


	void Start(){

		ic_S = GameObject.Find ("Door_status_S").GetComponent <SpriteRenderer>();
		ic_1a = GameObject.Find ("Door_status_1a").GetComponent <SpriteRenderer>();
		ic_1b = GameObject.Find ("Door_status_1b").GetComponent <SpriteRenderer>();
		ic_2 = GameObject.Find ("Door_status_2").GetComponent <SpriteRenderer>();
		ic_3 = GameObject.Find ("Door_status_3").GetComponent <SpriteRenderer>();


	}

	public void Progress () {

		if (M_Player.gameProgression == 1) {
			doors [0].SetActive (false);
			doors [1].SetActive (false);
			ic_S.sprite = sprtOn;
//			print (ic_S);
		}

		if (M_Player.gameProgression == 2) {
			doors [2].SetActive (false);
			doors [3].SetActive (false);
			ic_1a.sprite = sprtOn;
		
		}
		if (M_Player.gameProgression == 3) {
			doors [4].SetActive (false);
			doors [5].SetActive (false);
			ic_1b.sprite = sprtOn;

		}
		if (M_Player.gameProgression == 4) {
			doors [6].SetActive (false);
			doors [7].SetActive (false);
			ic_2.sprite = sprtOn;
		}
		if (M_Player.gameProgression == 10) {
			print ("Entering Boss Arena!");
			
			CameraMovement cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent <CameraMovement> ();

			cam.bossFightCam (1);
		}
	}
}
