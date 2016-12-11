using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {
	public M_Player scr;


	void OnTriggerEnter2D (Collider2D coll){
		//Debug.Log ("enter " + coll.name);

		if (coll.name == "1") {
			
			scr.stepDown = 0;

		}
		if (coll.name == "3") {
			scr.stepUp = 0;

		}
		if (coll.name == "2") {
			scr.stepRight = 0;

		}
		if (coll.name == "4") {
			scr.stepLeft = 0;

		}

		return;
	}

	void OnTriggerExit2D (Collider2D coll) {
		//Debug.Log ("exit " + coll.name);

		if(coll.name == "1"){
			scr.stepDown = 1;

		}
		if (coll.name == "3") {
			scr.stepUp = 1;

		}
		if (coll.name == "2") {
			scr.stepRight = 1;

		}
		if (coll.name == "4") {
			scr.stepLeft = 1;

		}
		return;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
