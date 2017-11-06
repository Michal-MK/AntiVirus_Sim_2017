using UnityEngine;
using System.Collections;


public class SaveGame : MonoBehaviour {

	public void SaveScore() {
		int difficulty = PlayerPrefs.GetInt("difficulty");
		string playerName = PlayerPrefs.GetString("player_name");
		if(playerName == null || playerName == "") {
			PlayerPrefs.SetString("player_name", System.Environment.UserName);
			playerName = System.Environment.UserName;
		}

		string currentTimeString = string.Format("{0:00}:{1:00}.{2:00}",
				(int)Timer.getTime / 60, Timer.getTime % 60, Timer.getTime.ToString().Remove(0, Timer.getTime.ToString().Length - 2));

		WWWForm form = new WWWForm();
		form.AddField("time", currentTimeString);
		form.AddField("player_name", playerName);


		new WWW("http://lestranky.maweb.eu/saveTimes/diff" + difficulty + ".php", form);

		float currentTime = Mathf.Round(Timer.getTime * 100) / 100;

		RearangeTimes(currentTime, difficulty);

	//	if (difficulty == 0) {

	//		int Min = 0;
	//		int toMax = 9;

	//		for (int i = toMax; i >= Min; i--) {
	//			string current = i.ToString();
	//			string oneInFrontOfCurrent = (i - 1).ToString();

	//			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity || PlayerPrefs.GetFloat(current) == 0 && i == toMax) {
	//				PlayerPrefs.SetFloat(current, currentTime);
	//			}
	//			if (i != Min) {
	//				if (PlayerPrefs.GetFloat(oneInFrontOfCurrent) > PlayerPrefs.GetFloat(current)) {

	//					float previousTime = PlayerPrefs.GetFloat(oneInFrontOfCurrent);
	//					PlayerPrefs.SetFloat(oneInFrontOfCurrent, PlayerPrefs.GetFloat(current));
	//					PlayerPrefs.SetFloat(current, previousTime);
	//				}
	//			}
	//		}
	//	}

	//	if (difficulty == 1) {

	//		int Min = 10;
	//		int toMax = 19;


	//		for (int i = toMax; i >= Min; i--) {
	//			string current = i.ToString();
	//			string oneInFrontOfCurrent = (i - 1).ToString();

	//			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity && i == toMax) {
	//				PlayerPrefs.SetFloat(current, currentTime);
	//			}
	//			if (i != Min) {
	//				if (PlayerPrefs.GetFloat(oneInFrontOfCurrent) > PlayerPrefs.GetFloat(current)) {

	//					float previousTime = PlayerPrefs.GetFloat(oneInFrontOfCurrent);
	//					PlayerPrefs.SetFloat(oneInFrontOfCurrent, PlayerPrefs.GetFloat(current));
	//					PlayerPrefs.SetFloat(current, previousTime);
	//				}
	//			}
	//		}
	//	}

	//	if (difficulty == 2) {

	//		int Min = 20;
	//		int toMax = 29;


	//		for (int i = toMax; i >= Min; i--) {
	//			string current = i.ToString();
	//			string oneInFrontOfCurrent = (i - 1).ToString();

	//			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity && i == toMax) {
	//				PlayerPrefs.SetFloat(current, currentTime);
	//			}
	//			if (i != Min) {
	//				if (PlayerPrefs.GetFloat(oneInFrontOfCurrent) > PlayerPrefs.GetFloat(current)) {

	//					float previousTime = PlayerPrefs.GetFloat(oneInFrontOfCurrent);
	//					PlayerPrefs.SetFloat(oneInFrontOfCurrent, PlayerPrefs.GetFloat(current));
	//					PlayerPrefs.SetFloat(current, previousTime);
	//				}
	//			}
	//		}
	//	}

	//	if (difficulty == 3) {

	//		int Min = 30;
	//		int toMax = 39;


	//		for (int i = toMax; i >= Min; i--) {
	//			string current = i.ToString();
	//			string oneInFrontOfCurrent = (i - 1).ToString();

	//			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity && i == toMax) {
	//				PlayerPrefs.SetFloat(current, currentTime);
	//			}
	//			if (i != Min) {
	//				if (PlayerPrefs.GetFloat(oneInFrontOfCurrent) > PlayerPrefs.GetFloat(current)) {

	//					float previousTime = PlayerPrefs.GetFloat(oneInFrontOfCurrent);
	//					PlayerPrefs.SetFloat(oneInFrontOfCurrent, PlayerPrefs.GetFloat(current));
	//					PlayerPrefs.SetFloat(current, previousTime);
	//				}
	//			}
	//		}
	//	}

	//	if (difficulty == 4) {

	//		int Min = 40;
	//		int toMax = 49;


	//		for (int i = toMax; i >= Min; i--) {
	//			string current = i.ToString();
	//			string oneInFrontOfCurrent = (i - 1).ToString();

	//			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity && i == toMax) {
	//				PlayerPrefs.SetFloat(current, currentTime);
	//			}
	//			if (i != Min) {
	//				if (PlayerPrefs.GetFloat(oneInFrontOfCurrent) > PlayerPrefs.GetFloat(current)) {

	//					float previousTime = PlayerPrefs.GetFloat(oneInFrontOfCurrent);
	//					PlayerPrefs.SetFloat(oneInFrontOfCurrent, PlayerPrefs.GetFloat(current));
	//					PlayerPrefs.SetFloat(current, previousTime);
	//				}
	//			}
	//		}
	//	}
	}

	private void RearangeTimes(float currentTime, int difficulty) {
		int minRange;
		int maxRange;
		switch (difficulty) {
			case 0: {
				minRange = 0;
				maxRange = 9;
				break;
			}
			case 1: {
				minRange = 10;
				maxRange = 19;
				break;
			}
			case 2: {
				minRange = 20;
				maxRange = 29;
				break;
			}
			case 3: {
				minRange = 30;
				maxRange = 39;
				break;
			}
			case 4: {
				minRange = 40;
				maxRange = 49;
				break;
			}
			default: {
				throw new System.Exception("Undefined difficulty!!");
			}
		}

		for (int i = maxRange; i >= minRange; i--) {
			string current = i.ToString();
			string oneInFrontOfCurrent = (i - 1).ToString();

			if (PlayerPrefs.GetFloat(current) == Mathf.Infinity && i == maxRange) {
				PlayerPrefs.SetFloat(current, currentTime);
			}
			if (i != minRange) {
				if (PlayerPrefs.GetFloat(oneInFrontOfCurrent) > PlayerPrefs.GetFloat(current)) {

					float previousTime = PlayerPrefs.GetFloat(oneInFrontOfCurrent);
					PlayerPrefs.SetFloat(oneInFrontOfCurrent, PlayerPrefs.GetFloat(current));
					PlayerPrefs.SetFloat(current, previousTime);
				}
			}
		}

	}
}


