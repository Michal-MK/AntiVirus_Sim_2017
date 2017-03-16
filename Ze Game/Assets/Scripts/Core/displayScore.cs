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
				results[i].GetComponent<Text>().text = "No time found.";
			}
			else {
				results[i].GetComponent<Text>().text = PlayerPrefs.GetFloat(current) + " s";
			}
		}

		for (int i = 10; i < 20; i++) {
			string current = i.ToString();

			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity) {
				results[i].GetComponent<Text>().text = "No time found.";
			}
			else {
				results[i].GetComponent<Text>().text = PlayerPrefs.GetFloat(current) + " s";
			}
		}

		for (int i = 20; i < 30; i++) {
			string current = i.ToString();

			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity) {
				results[i].GetComponent<Text>().text = "No time found.";
			}
			else {
				results[i].GetComponent<Text>().text = PlayerPrefs.GetFloat(current) + " s";
			}
		}

		for (int i = 30; i < 40; i++) {
			string current = i.ToString();

			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity) {
				results[i].GetComponent<Text>().text = "No time found.";
			}
			else {
				results[i].GetComponent<Text>().text = PlayerPrefs.GetFloat(current) + " s";
			}
		}

		for (int i = 40; i < 50; i++) {
			string current = i.ToString();

			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity) {
				results[i].GetComponent<Text>().text = "No time found.";
			}
			else {
				results[i].GetComponent<Text>().text = PlayerPrefs.GetFloat(current) + " s";
			}
		}
	}
}

