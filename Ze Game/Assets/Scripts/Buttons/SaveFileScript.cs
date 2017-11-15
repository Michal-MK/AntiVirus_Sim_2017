using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveFileScript : MonoBehaviour {

	public SaveData associatedData;

	public GameObject save;
	public RawImage rawImage;

	public void DeleteFile() {
		string savePath = "";
		string imgPath = "";
		string[] saveName = save.name.Split('.', '\\');

		for (int i = 0; i < saveName.Length; i++) {
			if (i <= saveName.Length - 3) {
				savePath += saveName[i] + '\\';
			}
			else {
				imgPath = savePath + "Resources\\" + saveName[i];
				savePath += saveName[i];
				break;
			}
		}

		print(savePath + ".Kappa");
		File.Delete(savePath + ".Kappa");
		print(imgPath + ".png");
		File.Delete(imgPath + ".png");

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void LoadData() {
		Control.script.loadManager.Load(associatedData);
		MenuMusic.script.StopMusicWrapper();
	}
}
