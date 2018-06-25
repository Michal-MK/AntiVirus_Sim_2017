using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFileScript : MonoBehaviour {

	public SaveData associatedData;
	public SaveFile saveFile;
	public GameObject save;

	public void DeleteFile() {
		Directory.Delete(new FileInfo(saveFile.data.core.fileLocation).Directory.FullName, true);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	public void LoadData() {
		Control.script.loadManager.Load(associatedData);
		MenuMusic.script.StopMusic();
	}
}
