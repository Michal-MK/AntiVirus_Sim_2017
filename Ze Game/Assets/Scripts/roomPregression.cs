using UnityEngine;
using System.Collections;


public class roomPregression : MonoBehaviour {

	public GameObject[] door;
	public Sprite sprtOff;
	public Sprite sprtOn;

	void Start(){

	}



	public void Progress () {

		if (M_Player.gameProgression == 1) {
			door [0].SetActive (false);
			door[1].SetActive (false);


			GameObject indicatecolor = GameObject.Find ("Indicator_status_1");

			indicatecolor.GetComponent <SpriteRenderer>().sprite = sprtOn;
		}

		if (M_Player.gameProgression == 2) {
			door [2].SetActive (false);
			door [3].SetActive (false);
		
		}
		if (M_Player.gameProgression == 3) {
			door [4].SetActive (false);
			door [5].SetActive (false);

		}
		if (M_Player.gameProgression == 4) {
			door [6].SetActive (false);
			door [7].SetActive (false);

		}
			
	}
	
	// Update is called once per frame
	void Update () {

	}
}
