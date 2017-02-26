using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPage : MonoBehaviour {

	// Use this for initialization
	public void OpenIt() {
		if (gameObject.name == "Source") {
			Application.OpenURL("https://github.com/MoonKill/Unity-Game");
		}
		else if (gameObject.name == "School"){
			Application.OpenURL("http://www.sportgym.cz/");
		}
	}
}
