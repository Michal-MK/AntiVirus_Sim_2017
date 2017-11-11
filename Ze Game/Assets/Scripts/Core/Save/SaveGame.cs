using UnityEngine;
using System.Collections;


public class SaveGame : MonoBehaviour {

	public void SaveScore() {

		int difficulty = Control.currDifficulty;
		string playerName = Control.currProfile.getProfileName;

		if(playerName == null || playerName == "") {
			playerName = System.Environment.UserName;
		}

		WWWForm form = new WWWForm();
		form.AddField("time", Timer.getTimeFormated);
		form.AddField("player_name", playerName);

		new WWW("http://lestranky.maweb.eu/saveTimes/diff" + difficulty + ".php", form);

		//Not sure what this code is doing...
		//RearangeTimes(Mathf.Round(Timer.getTime * 100) / 100, difficulty);
	}

	//private void RearangeTimes(float currentTime, int difficulty) {
	//	int minRange;
	//	int maxRange;
	//	switch (difficulty) {
	//		case 0: {
	//			minRange = 0;
	//			maxRange = 9;
	//			break;
	//		}
	//		case 1: {
	//			minRange = 10;
	//			maxRange = 19;
	//			break;
	//		}
	//		case 2: {
	//			minRange = 20;
	//			maxRange = 29;
	//			break;
	//		}
	//		case 3: {
	//			minRange = 30;
	//			maxRange = 39;
	//			break;
	//		}
	//		case 4: {
	//			minRange = 40;
	//			maxRange = 49;
	//			break;
	//		}
	//		default: {
	//			throw new System.Exception("Undefined difficulty!!");
	//		}
	//	}
	//	print("What the **** is this???!!");
	//	for (int i = maxRange; i >= minRange; i--) {
	//		string current = i.ToString();
	//		string oneInFrontOfCurrent = (i - 1).ToString();

	//		if (PlayerPrefs.GetFloat(current) == Mathf.Infinity && i == maxRange) {
	//			PlayerPrefs.SetFloat(current, currentTime);
	//		}
	//		if (i != minRange) {
	//			if (PlayerPrefs.GetFloat(oneInFrontOfCurrent) > PlayerPrefs.GetFloat(current)) {

	//				float previousTime = PlayerPrefs.GetFloat(oneInFrontOfCurrent);
	//				PlayerPrefs.SetFloat(oneInFrontOfCurrent, PlayerPrefs.GetFloat(current));
	//				PlayerPrefs.SetFloat(current, previousTime);
	//			}
	//		}
	//	}

	//}
}


