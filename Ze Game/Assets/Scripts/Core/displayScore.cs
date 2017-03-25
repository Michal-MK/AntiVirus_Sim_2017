using UnityEngine;
using UnityEngine.UI;


public class displayScore : MonoBehaviour {


	public Transform resultsTransform;
	public GameObject textWScore;
	GameObject textField;
	public GameObject[] results = new GameObject[49];

	private void Start() {
		CreateBoard();
		Display();
	}
	public void CreateBoard() {
		for (int i = 0; i < 50; i++) {
			GameObject textBox = Instantiate(textWScore, new Vector3(1, 1, 1), Quaternion.identity, resultsTransform);
			textBox.name = "Result " + i;
			results[i] = textBox;
		}
	}


	public void Display() {

		

		for (int i = 0; i < 10; i++) {
			string current = i.ToString();

			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity) {
				results[i].GetComponent<Text>().text = "";
			}
			else {
				float time = PlayerPrefs.GetFloat(current);
				if (time != 0) {
					string formatedTime = string.Format("{0:00}:{1:00}.{2:00} minutes", (int)time / 60, time % 60, time.ToString().Remove(0, time.ToString().Length - 2));
					results[i].GetComponent<Text>().text = formatedTime;
				}
			}
		}

		for (int i = 10; i < 20; i++) {
			string current = i.ToString();

			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity) {
				results[i].GetComponent<Text>().text = "";
			}
			else {
				float time = PlayerPrefs.GetFloat(current);
				if (time != 0) {
					string formatedTime = string.Format("{0:00}:{1:00}.{2:00} minutes", (int)time / 60, time % 60, time.ToString().Remove(0, time.ToString().Length - 2));
					results[i].GetComponent<Text>().text = formatedTime;
				}
			}
		}

		for (int i = 20; i < 30; i++) {
			string current = i.ToString();

			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity) {
				results[i].GetComponent<Text>().text = "";
			}
			else {
				float time = PlayerPrefs.GetFloat(current);
				if (time != 0) {
					string formatedTime = string.Format("{0:00}:{1:00}.{2:00} minutes", (int)time / 60, time % 60, time.ToString().Remove(0, time.ToString().Length - 2));
					results[i].GetComponent<Text>().text = formatedTime;
				}
			}
		}

		for (int i = 30; i < 40; i++) {
			string current = i.ToString();

			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity) {
				results[i].GetComponent<Text>().text = "";
			}
			else {
				float time = PlayerPrefs.GetFloat(current);
				if (time != 0) {
					string formatedTime = string.Format("{0:00}:{1:00}.{2:00} minutes", (int)time / 60, time % 60, time.ToString().Remove(0, time.ToString().Length - 2));
					results[i].GetComponent<Text>().text = formatedTime;
				}
			}
		}

		for (int i = 40; i < 50; i++) {
			string current = i.ToString();

			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity) {
				results[i].GetComponent<Text>().text = "";
			}
			else {
				float time = PlayerPrefs.GetFloat(current);
				if (time != 0) {
					string formatedTime = string.Format("{0:00}:{1:00}.{2:00} minutes", (int)time / 60, time % 60, time.ToString().Remove(0, time.ToString().Length - 2));
					results[i].GetComponent<Text>().text = formatedTime;
				}
			}
		}
	}
}

