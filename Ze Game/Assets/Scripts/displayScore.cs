using UnityEngine;
using UnityEngine.UI;


public class displayScore : MonoBehaviour {

	public GameObject textWScore;
	GameObject textField;
	public GameObject[] results = new GameObject[55];

	private void Start() {
		Display();
	}


	public void Display() {

		Transform sort = GameObject.Find(("Results")).transform;



		for (int i = 0; i < 10; i++) {

			if (PlayerPrefs.GetFloat(i.ToString()) != 500 && PlayerPrefs.GetFloat(i.ToString()) != 0f) {
				//print("Score is not 500");
				results[i] = (GameObject)Instantiate(textWScore, new Vector3(1, 1, 1), Quaternion.identity);
				results[i].transform.SetParent(sort);
				results[i].GetComponent<Text>().text = PlayerPrefs.GetFloat(i.ToString()) + " s  ";

				results[i].transform.localScale = new Vector3(1, 1, 1);

				//Debug.Log(i + "  " + PlayerPrefs.GetFloat(i.ToString()));

			}
			else if (PlayerPrefs.GetFloat(i.ToString()) == 500) {
				//print("Score is not 500");
				results[i] = (GameObject)Instantiate(textWScore, new Vector3(1, 1, 1), Quaternion.identity);
				results[i].transform.SetParent(sort);
				results[i].GetComponent<Text>().text = "No time found.";

				results[i].transform.localScale = new Vector3(1, 1, 1);

				//Debug.Log(i + "  " + PlayerPrefs.GetFloat(i.ToString()));
			}
		}


		for (int i = 11; i < 21; i++) {

			if (PlayerPrefs.GetFloat(i.ToString()) != 500 && PlayerPrefs.GetFloat(i.ToString()) != 0f) { 
				results[i] = (GameObject)Instantiate(textWScore, new Vector3(1, 1, 1), Quaternion.identity);
				results[i].transform.SetParent(sort);
				results[i].GetComponent<Text>().text = PlayerPrefs.GetFloat(i.ToString()) + " s  ";

				results[i].transform.localScale = new Vector3(1, 1, 1);

				//Debug.Log(i + "  " + PlayerPrefs.GetFloat(i.ToString()));

			}
			else if (PlayerPrefs.GetFloat(i.ToString()) == 500) {
				results[i] = (GameObject)Instantiate(textWScore, new Vector3(1, 1, 1), Quaternion.identity);
				results[i].transform.SetParent(sort);
				results[i].GetComponent<Text>().text = "No time found.";

				results[i].transform.localScale = new Vector3(1, 1, 1);

				//Debug.Log(i + "  " + PlayerPrefs.GetFloat(i.ToString()));
			}
		}


		for (int i = 22; i < 32; i++) {

			if (PlayerPrefs.GetFloat(i.ToString()) != 500 && PlayerPrefs.GetFloat(i.ToString()) != 0f) {
				results[i] = (GameObject)Instantiate(textWScore, new Vector3(1, 1, 1), Quaternion.identity);
				results[i].transform.SetParent(sort);
				results[i].GetComponent<Text>().text = PlayerPrefs.GetFloat(i.ToString()) + " s  ";

				results[i].transform.localScale = new Vector3(1, 1, 1);

				//Debug.Log(i + "  " + PlayerPrefs.GetFloat(i.ToString()));

			}
			else if (PlayerPrefs.GetFloat(i.ToString()) == 500) {
				results[i] = (GameObject)Instantiate(textWScore, new Vector3(1, 1, 1), Quaternion.identity);
				results[i].transform.SetParent(sort);
				results[i].GetComponent<Text>().text = "No time found.";

				results[i].transform.localScale = new Vector3(1, 1, 1);

				//Debug.Log(i + "  " + PlayerPrefs.GetFloat(i.ToString()));
			}
		}


		for (int i = 33; i < 43; i++) {

			if (PlayerPrefs.GetFloat(i.ToString()) != 500 && PlayerPrefs.GetFloat(i.ToString()) != 0f) {
				results[i] = (GameObject)Instantiate(textWScore, new Vector3(1, 1, 1), Quaternion.identity);
				results[i].transform.SetParent(sort);
				results[i].GetComponent<Text>().text = PlayerPrefs.GetFloat(i.ToString()) + " s  ";

				results[i].transform.localScale = new Vector3(1, 1, 1);

				//Debug.Log(i + "  " + PlayerPrefs.GetFloat(i.ToString()));

			}
			else if (PlayerPrefs.GetFloat(i.ToString()) == 500) {
				results[i] = (GameObject)Instantiate(textWScore, new Vector3(1, 1, 1), Quaternion.identity);
				results[i].transform.SetParent(sort);
				results[i].GetComponent<Text>().text = "No time found.";

				results[i].transform.localScale = new Vector3(1, 1, 1);

				//Debug.Log(i + "  " + PlayerPrefs.GetFloat(i.ToString()));
			}
		}


		for (int i = 44; i < 54; i++) {

			if (PlayerPrefs.GetFloat(i.ToString()) != 500 && PlayerPrefs.GetFloat(i.ToString()) != 0f) {
				results[i] = (GameObject)Instantiate(textWScore, new Vector3(1, 1, 1), Quaternion.identity);
				results[i].transform.SetParent(sort);
				results[i].GetComponent<Text>().text = PlayerPrefs.GetFloat(i.ToString()) + " s  ";

				results[i].transform.localScale = new Vector3(1, 1, 1);

				//Debug.Log(i + "  " + PlayerPrefs.GetFloat(i.ToString()));

			}
			else if (PlayerPrefs.GetFloat(i.ToString()) == 500) {
				results[i] = (GameObject)Instantiate(textWScore, new Vector3(1, 1, 1), Quaternion.identity);
				results[i].transform.SetParent(sort);

				results[i].GetComponent<Text>().text = "No time found.";

				results[i].transform.localScale = new Vector3(1, 1, 1);

				//Debug.Log(i + "  " + PlayerPrefs.GetFloat(i.ToString()));
			}
		}
	}
}

