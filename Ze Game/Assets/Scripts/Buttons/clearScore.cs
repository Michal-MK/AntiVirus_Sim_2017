using UnityEngine;
using System.Collections;

public class clearScore : MonoBehaviour {

	public void resetTimes () {
		for (int i = 0; i < 10; i++) {
			PlayerPrefs.SetFloat (i.ToString(), 500f);
		
		}	
	}	
}
