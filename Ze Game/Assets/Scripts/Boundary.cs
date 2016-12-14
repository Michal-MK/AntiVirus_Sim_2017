using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boundary : MonoBehaviour {
	public M_Player scr;
	public Spike count;
	public GameObject Door;


	public List<Collider2D> touching  = new List<Collider2D>();



	void Start (){
		
	}



//	void OnTriggerEnter2D (Collider2D coll){
//		Debug.Log ("enter " + coll.name);
//
//		touching.Add (coll);
//
//		if (coll.name == "1") {
//			
//			scr.stepDown = 0;
//
//		}
//		if (coll.name == "3") {
//			scr.stepUp = 0;
//
//		}
//		if (coll.name == "2door" || coll.name == "2"  ) {
//			scr.stepRight = 0;
//		}
//
//		if (coll.name == "4") {
//			scr.stepLeft = 0;
//
//		}
//
//		return;
//	}
//
//	void OnTriggerExit2D (Collider2D coll) {
//		Debug.Log ("exit " + coll.name);
//		touching.Remove (coll);
//
//		if(coll.name == "1"){
//			scr.stepDown = 1;
//
//		}
//		if (coll.name == "3") {
//			scr.stepUp = 1;
//
//		}
//		if (coll.name == "2door" || coll.name == "2") {
//			if (touching.Count > 0) {
//				foreach (Collider2D collider in touching) {
//					if (collider.name == "2door") {
//						Debug.Log ("NAY");
//
//					} else {
//						Debug.Log ("Noice");
//						scr.stepRight = 1;
//					}
//				}
//			} 
//			else {
//				scr.stepRight = 1;
//			}
//		}
//		if (coll.name == "4") {
//			scr.stepLeft = 1;
//
//		}
////		touching.Remove (coll);
//		return;
//
//	}

	public void clearPassageToRoom1(){
			Destroy (Door.gameObject);
		
	}	
}