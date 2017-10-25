using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DisplayScoreList : MonoBehaviour {

	public RectTransform resultsTransform;
	public GameObject databaseTimes;

	public RectTransform d0;
	public RectTransform d1;
	public RectTransform d2;
	public RectTransform d3;
	public RectTransform d4;

	public GameObject[] results0;
	public List<string> clearTimes0 = new List<string>();
	public List<string> names0 = new List<string>();


	public GameObject[] results1;
	public List<string> clearTimes1 = new List<string>();
	public List<string> names1 = new List<string>();

	public GameObject[] results2;
	public List<string> clearTimes2 = new List<string>();
	public List<string> names2 = new List<string>();

	public GameObject[] results3;
	public List<string> clearTimes3 = new List<string>();
	public List<string> names3 = new List<string>();

	public GameObject[] results4;
	public List<string> clearTimes4 = new List<string>();
	public List<string> names4 = new List<string>();

	private int count;
	//private int longestScoreList;


	private void Start() {
		StartCoroutine(RetrieveData());
	}




	public IEnumerator RetrieveData() {
		string[] values;

		string link = "http://lestranky.maweb.eu/times.php";
		WWW times = new WWW(link);
		yield return times;

		if (times == null) {
			print("There was an error checking admin: " + times.error);
			yield break;
		}
		else {
			string data = times.text;
			values = data.Split('_', '%', '~');
		}
		//foreach (var item in values) {
		//	print(item);
		//}

		int currentDiff = 0;
		for (int i = 0; i < values.Length; i++) {

			if (values[i].IndexOf('#') == 0) {
				string edited = values[i].Remove(0, 1);
				currentDiff = int.Parse(edited);
				print(currentDiff);
			}

			else if (values[i].IndexOf('@') == 0) {
				string edited = values[i].Remove(0, 1);

				switch (currentDiff) {
					case 0: {
						names0.Add(edited);
						break;
					}
					case 1: {
						names1.Add(edited);
						break;
					}
					case 2: {
						names2.Add(edited);
						break;
					}
					case 3: {
						names3.Add(edited);
						break;
					}
					case 4: {
						names4.Add(edited);
						break;
					}
				}
			}
			else if (values[i].IndexOf(':') == 2) {
				switch (currentDiff) {
					case 0: {
						clearTimes0.Add(values[i]);
						break;
					}
					case 1: {
						clearTimes1.Add(values[i]);
						break;
					}
					case 2: {
						clearTimes2.Add(values[i]);
						break;
					}
					case 3: {
						clearTimes3.Add(values[i]);
						break;
					}
					case 4: {
						clearTimes4.Add(values[i]);
						break;
					}
				}
			}
		}
		CreateBoard();
	}

	public void CreateBoard() {

		for (int i = 0; i < 5; i++) {

			if (i == 0) {
				count = clearTimes0.Count;

				results0 = new GameObject[count];
				if (count != 0) {
					for (int j = 0; j < count; j++) {
						if (count >= 9) {
							d0.sizeDelta += new Vector2(0,40);
						}
						GameObject textBox = Instantiate(databaseTimes, Vector3.zero, Quaternion.identity, d0);
						textBox.name = "Result " + j;
						results0[j] = textBox;
					}
				}
			}
			else if (i == 1) {
				count = clearTimes1.Count;

				results1 = new GameObject[count];
				if (count != 0) {
					for (int j = 0; j < count; j++) {
						GameObject textBox = Instantiate(databaseTimes, Vector3.zero, Quaternion.identity, d1);
						textBox.name = "Result " + j;


						results1[j] = textBox;



					}
				}
			}
			else if (i == 2) {
				count = clearTimes2.Count;

				results2 = new GameObject[count];
				if (count != 0) {
					for (int j = 0; j < count; j++) {
						GameObject textBox = Instantiate(databaseTimes, Vector3.zero, Quaternion.identity, d2);
						textBox.name = "Result " + j;


						results2[j] = textBox;


					}
				}
			}
			else if (i == 3) {
				count = clearTimes3.Count;

				results3 = new GameObject[count];
				if (count != 0) {
					for (int j = 0; j < count; j++) {
						GameObject textBox = Instantiate(databaseTimes, Vector3.zero, Quaternion.identity, d3);
						textBox.name = "Result " + j;

						results3[j] = textBox;

					}
				}
			}

			else if (i == 4) {
				count = clearTimes4.Count;

				results4 = new GameObject[count];
				if (count != 0) {
					for (int j = 0; j < count; j++) {
						GameObject textBox = Instantiate(databaseTimes, Vector3.zero, Quaternion.identity, d4);
						textBox.name = "Result " + j;

						results4[j] = textBox;

					}
				}
			}
		}
		Display();
	}

	public void Display() {

		for (int i = 0; i < clearTimes0.Count; i++) {
			if (results0.Length >= i) {
				//results0[i].GetComponent<Text>().text = names0[i] + " - " + clearTimes0[i];
				RectTransform[] res = results0[i].GetComponentsInChildren<RectTransform>();
				foreach(RectTransform Obj in res) {
					if(Obj.name == "Name") {
						Obj.GetComponent<Text>().text = names0[i];
					}
					else if (Obj.name == "Time") {
						Obj.GetComponent<Text>().text = clearTimes0[i];
					}
				}
			}
		}

		for (int i = 0; i < clearTimes1.Count; i++) {
			if (results1.Length >= i) {
				//results1[i].GetComponent<Text>().text = names1[i] + "\t" + clearTimes1[i];
				RectTransform[] res = results1[i].GetComponentsInChildren<RectTransform>();
				foreach (RectTransform Obj in res) {
					if (Obj.name == "Name") {
						Obj.GetComponent<Text>().text = names1[i];
					}
					else if (Obj.name == "Time") {
						Obj.GetComponent<Text>().text = clearTimes1[i];
					}
				}
			}
		}

		for (int i = 0; i < clearTimes2.Count; i++) {
			if (results2.Length >= i) {
				//results2[i].GetComponent<Text>().text = names2[i] + "\t" + clearTimes2[i];
				RectTransform[] res = results2[i].GetComponentsInChildren<RectTransform>();
				foreach (RectTransform Obj in res) {
					if (Obj.name == "Name") {
						Obj.GetComponent<Text>().text = names2[i];
					}
					else if (Obj.name == "Time") {
						Obj.GetComponent<Text>().text = clearTimes2[i];
					}
				}
			}
		}

		for (int i = 0; i < clearTimes3.Count; i++) {
			if (results3.Length >= i) {
				//results3[i].GetComponent<Text>().text = names3[i] + "\t" + clearTimes3[i];
				RectTransform[] res = results3[i].GetComponentsInChildren<RectTransform>();
				foreach (RectTransform Obj in res) {
					if (Obj.name == "Name") {
						Obj.GetComponent<Text>().text = names3[i];
					}
					else if (Obj.name == "Time") {
						Obj.GetComponent<Text>().text = clearTimes3[i];
					}
				}
			}
		}

		for (int i = 0; i < clearTimes4.Count; i++) {
			if (results4.Length >= i) {
				//results4[i].GetComponent<Text>().text = names4[i] + "\t" + clearTimes4[i];
				RectTransform[] res = results4[i].GetComponentsInChildren<RectTransform>();
				foreach (RectTransform Obj in res) {
					if (Obj.name == "Name") {
						Obj.GetComponent<Text>().text = names4[i];
					}
					else if (Obj.name == "Time") {
						Obj.GetComponent<Text>().text = clearTimes4[i];
					}
				}
			}
		}
	}
}
