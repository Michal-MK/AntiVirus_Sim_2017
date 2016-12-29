using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {
	public M_Player scr;
	public Spike count;
	public GameObject Door;


	public void clearPassageToRoom1(){
		Destroy(Door);

	}	
}