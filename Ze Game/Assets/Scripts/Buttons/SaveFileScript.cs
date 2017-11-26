using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveFileScript : MonoBehaviour {

	public SaveData associatedData;
	public SaveFile saveFile;
	public GameObject save;

	public void DeleteFile() {

		File.Delete(saveFile.data.core.fileLocation);
		File.Delete(saveFile.data.core.imgFileLocation);

		foreach (SaveData data in saveFile.saveHistory.saveHistory) {
			File.Delete(data.core.imgFileLocation);
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}


	public void LoadData() {
		Control.script.loadManager.Load(associatedData);
		MenuMusic.script.StopMusicWrapper();
	}
}
