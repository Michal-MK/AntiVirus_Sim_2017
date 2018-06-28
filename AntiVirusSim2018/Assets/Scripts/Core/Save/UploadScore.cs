using UnityEngine;

public class UploadScore {

	public UploadScore() {
		int difficulty = Control.currDifficulty;
		string playerName = "";

		if (playerName == null || playerName == "") {
			playerName = System.Environment.UserName;
		}

		WWWForm form = new WWWForm();
		form.AddField("time", Timer.getTimeFormated);
		form.AddField("player_name", playerName);

		new WWW("http://lestranky.maweb.eu/saveTimes/diff" + difficulty + ".php", form);
	}
}


