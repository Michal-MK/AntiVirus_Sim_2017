using SaveData_Helper;
using System.Collections.Generic;

//Data to be saved
[System.Serializable]
public class SaveData {
	public PlayerData player = new PlayerData();
	public DisplayedHints shownHints = new DisplayedHints();
	public World world = new World();
	public Core core = new Core();
}

[System.Serializable]
public class SaveHistory {
	public List<SaveData> previousSaves = new List<SaveData>();
}

[System.Serializable]
public class SaveFile {
	public SaveData data = new SaveData();
	public SaveHistory saveHistory = new SaveHistory();
}





