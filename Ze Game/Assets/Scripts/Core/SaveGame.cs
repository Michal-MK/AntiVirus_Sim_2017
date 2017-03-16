using UnityEngine;

public class SaveGame : MonoBehaviour {

	public void saveScore() {


		int difficulty = PlayerPrefs.GetInt("difficulty");
		float currentTime = Mathf.Round(timer.time * 100) * 0.01f;

		print(difficulty + " " + currentTime);


		if (difficulty == 0) {

			int Min = 0;
			int toMax = 9;

			for (int i = toMax; i >= Min; i--) {
				string current = i.ToString();
				string oneInFrontOfCurrent = (i - 1).ToString();

				if (PlayerPrefs.HasKey(current)) {
					if (PlayerPrefs.GetFloat(current) == Mathf.Infinity && i == toMax) {
						PlayerPrefs.SetFloat(current, currentTime);
					}
					if (i != Min) {
						if (PlayerPrefs.GetFloat(oneInFrontOfCurrent) > PlayerPrefs.GetFloat(current)) {

							float previousTime = PlayerPrefs.GetFloat(oneInFrontOfCurrent);
							PlayerPrefs.SetFloat(oneInFrontOfCurrent, PlayerPrefs.GetFloat(current));
							PlayerPrefs.SetFloat(current, previousTime);
						}
					}
				}
			}
		}

		if (difficulty == 1) {

			int Min = 10;
			int toMax = 19;


			for (int i = toMax; i >= Min; i--) {
				string current = i.ToString();
				string oneInFrontOfCurrent = (i - 1).ToString();

				if (PlayerPrefs.HasKey(current)) {
					if (PlayerPrefs.GetFloat(current) == Mathf.Infinity && i == toMax) {
						PlayerPrefs.SetFloat(current, currentTime);
					}
					if (i != Min) {
						if (PlayerPrefs.GetFloat(oneInFrontOfCurrent) > PlayerPrefs.GetFloat(current)) {

							float previousTime = PlayerPrefs.GetFloat(oneInFrontOfCurrent);
							PlayerPrefs.SetFloat(oneInFrontOfCurrent, PlayerPrefs.GetFloat(current));
							PlayerPrefs.SetFloat(current, previousTime);
						}
					}
				}
			}
		}
	
		if (difficulty == 2) {

			int Min = 20;
			int toMax = 29;


			for (int i = toMax; i >= Min; i--) {
				string current = i.ToString();
				string oneInFrontOfCurrent = (i - 1).ToString();

				if (PlayerPrefs.HasKey(current)) {
					if (PlayerPrefs.GetFloat(current) == Mathf.Infinity && i == toMax) {
						PlayerPrefs.SetFloat(current, currentTime);
					}
					if (i != Min) {
						if (PlayerPrefs.GetFloat(oneInFrontOfCurrent) > PlayerPrefs.GetFloat(current)) {

							float previousTime = PlayerPrefs.GetFloat(oneInFrontOfCurrent);
							PlayerPrefs.SetFloat(oneInFrontOfCurrent, PlayerPrefs.GetFloat(current));
							PlayerPrefs.SetFloat(current, previousTime);
						}
					}
				}
			}
		}
		
		if (difficulty == 3) {

			int Min = 30;
			int toMax = 39;


			for (int i = toMax; i >= Min; i--) {
				string current = i.ToString();
				string oneInFrontOfCurrent = (i - 1).ToString();

				if (PlayerPrefs.HasKey(current)) {
					if (PlayerPrefs.GetFloat(current) == Mathf.Infinity && i == toMax) {
						PlayerPrefs.SetFloat(current, currentTime);
					}
					if (i != Min) {
						if (PlayerPrefs.GetFloat(oneInFrontOfCurrent) > PlayerPrefs.GetFloat(current)) {

							float previousTime = PlayerPrefs.GetFloat(oneInFrontOfCurrent);
							PlayerPrefs.SetFloat(oneInFrontOfCurrent, PlayerPrefs.GetFloat(current));
							PlayerPrefs.SetFloat(current, previousTime);
						}
					}
				}
			}
		}

		if (difficulty == 4) {

			int Min = 40;
			int toMax = 49;


			for (int i = toMax; i >= Min; i--) {
				string current = i.ToString();
				string oneInFrontOfCurrent = (i - 1).ToString();

				if (PlayerPrefs.HasKey(current)) {
					if (PlayerPrefs.GetFloat(current) == Mathf.Infinity && i == toMax) {
						PlayerPrefs.SetFloat(current, currentTime);
					}
					if (i != Min) {
						if (PlayerPrefs.GetFloat(oneInFrontOfCurrent) > PlayerPrefs.GetFloat(current)) {

							float previousTime = PlayerPrefs.GetFloat(oneInFrontOfCurrent);
							PlayerPrefs.SetFloat(oneInFrontOfCurrent, PlayerPrefs.GetFloat(current));
							PlayerPrefs.SetFloat(current, previousTime);
						}
					}
				}
			}
		}
	}
}


