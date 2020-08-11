using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Igor.Constants.Strings;

public class Credits : MonoBehaviour {
	public AudioSource source;

	private bool one = true;

	private void Start() {
		Text t = gameObject.GetComponent<Text>();
		t.text = "AntiVirus Simulator 2017!";
		t.text += "\n\n\n";
		t.text += "Created by:\n";
		t.text += "Michal Hazdra\n";
		t.text += "\n";
		t.text += "This game is a result of my seminary project for IT classes at: \n";
		t.text += "Gymnázium Dr. Antona Randy\n Jablonec nad Nisou \n";
		t.text += "\n\n\n";
		t.text += "Project began on the 8th of December 2016 and finished on the 16th of march 2017.\n\n";
		t.text += "Since then I tried to improve the code and expand on it in my free time.\n";
		t.text += "Total of 129 days.\n";
		t.text += "\n\n";
		t.text += "Using:\n Unity3D Framework\n by \n Unity Technologies\n\n";
		t.text += "Thank you for playing & see you next time!\n";
		t.text += "\n( •ᴗ•)\n\n";
		t.text += "© 2017";

	}

	void FixedUpdate() {
		float x = gameObject.transform.position.x;
		float y = gameObject.transform.position.y;
		y += 2f;
		gameObject.transform.position = new Vector3(x, y, 0);
		if (y > 0 && one) {
			StartCoroutine(LoadScene1());
		}
	}

	private IEnumerator LoadScene1() {
		one = false;
		for (float f = 1; f > -1; f -= Time.unscaledDeltaTime * 0.5f) {
			if (f > 0) {
				source.volume = f;
				yield return null;
			}
			else {
				SceneManager.LoadScene(SceneNames.MENU_SCENE);
				break;
			}
		}
	}
}