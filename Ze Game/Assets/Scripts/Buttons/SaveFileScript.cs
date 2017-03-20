using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SaveFileScript : MonoBehaviour {

	public GameObject save;

	public void DeleteFile() {

		string[] saveName = save.name.Split('.', '\\');

		string savePath = saveName[0] + '\\' + saveName[1] + '\\' + saveName[2] + '\\' + saveName[3] + '\\' + saveName[4] + '\\' + saveName[5];
		string imgPath = savePath + "\\Resources\\";

		print(savePath +'\\'+ saveName[6] + ".Kappa");
		File.Delete(savePath + '\\' + saveName[6] + ".Kappa");
		print(imgPath + saveName[6] + ".png");
		File.Delete(imgPath + saveName[6] + ".png");

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
