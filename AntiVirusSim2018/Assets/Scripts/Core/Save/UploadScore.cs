using UnityEngine;
using UnityEngine.Networking;

public class UploadScore {

	public UploadScore() {
		int difficulty = Control.currDifficulty;
		string playerName = "";

		if (playerName == "") {
			playerName = System.Environment.UserName;
		}

		WWWForm form = new WWWForm();
		form.AddField("time", Timer.Instance.ElapsedStr);
		form.AddField("player_name", playerName);

		UnityWebRequest.Post("http://lestranky.maweb.eu/saveTimes/diff" + difficulty + ".php", form);
	}
}


