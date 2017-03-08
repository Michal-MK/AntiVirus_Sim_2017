using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {
	private void Start() {
		Text t = gameObject.GetComponent<Text>();
		t.text = "The Game!";
		t.text += "\n\n\n";
		t.text += "Created by:\n";
		t.text += "Michal Hazdra\n";
		t.text += "\n";
		t.text += "This game is a result of my seminary project for IT classes at: \n";
		t.text += "Gymnázium Dr. Antona Randy\n Jablonec nad Nisou \n";
		t.text += "\n\n\n";
		t.text += "The project began on the 8th of december 2016 and finished on the 25th of february 2017. \n\n";
		t.text += "Total of 79 days.\n";
		t.text += "\n\n";
		t.text += "Using:\n Unity3D Framework\n by \n Unity Technologies";
		t.text += "\n\n\n";
		t.text += "© 2017";

	}

	void Update() {
		float x = gameObject.transform.position.x;
		float y = gameObject.transform.position.y;
		y += 0.5f;
		gameObject.transform.position = new Vector3(x, y, 0);
		if (y > 0) {
			SceneManager.LoadScene(0);
		}
	}
}