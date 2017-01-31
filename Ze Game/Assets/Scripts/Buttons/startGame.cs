using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour {
	bool exp = false;

	public void StartGame() {
		exp = GameObject.Find("Exp?").GetComponent<Toggle>().isOn;

		if (exp) {
			PlayerPrefs.SetInt("exp.", 1);
		}
		else {
			PlayerPrefs.SetInt("exp.", 0);
		}

		SceneManager.LoadScene(1);
	}

}
