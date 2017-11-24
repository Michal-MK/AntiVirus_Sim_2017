using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySaveFiles : MonoBehaviour {
	string dataPath;
	string BGName;
	public GameObject SaveObj;
	public GameObject NoSaves;

	public RectTransform content;

	public static int selectedAttempt;

	private void Awake() {
		dataPath = Application.dataPath + Path.DirectorySeparatorChar + "Saves" + Path.DirectorySeparatorChar;
	}

	void Start() {
		DisplaySaves();
	}

	public void DisplaySaves() {
		DirectoryInfo dir = new DirectoryInfo(dataPath);
		DirectoryInfo[] info = dir.GetDirectories();

		foreach (DirectoryInfo saveDirectory in info) {

			FileInfo[] saveFiles = saveDirectory.GetFiles("*.Kappa");
			for (int i = 0; i < saveFiles.Length; i++) {

				using (FileStream file = new FileStream(saveFiles[i].FullName, FileMode.Open)) {
					BinaryFormatter br = new BinaryFormatter();

					SaveFile saveInfo = (SaveFile)br.Deserialize(file);

					GameObject save = Instantiate(SaveObj, content.transform);
					save.name = saveFiles[i].FullName;
					byte[] img = File.ReadAllBytes(saveInfo.data.core.imgFileLocation);
					Texture2D tex = new Texture2D(800, 600);
					tex.LoadImage(img);
					save.transform.Find("SaveImage").GetComponent<RawImage>().texture = tex;


					try {
						save.transform.Find("ShowHistory").GetComponent<DisplaySaveHistory>().selfHistory = saveInfo.saveHistory.saveHistory;
					}
					catch {
						print("Failed to test");
					}

					switch (saveInfo.data.player.currentBGName) {
						case "Background_Start": {
							BGName = "Electical Hall";
							break;
						}
						case "Background_room_1": {
							BGName = "Icy Plains";
							break;
						}
						case "Background_room_2a": {
							BGName = "Danger Zone";
							break;
						}
						case "Background_room_2b": {
							BGName = "Peaceful Corner";
							break;
						}
						case "Background_room_Boss_1": {
							BGName = "Boss Area";
							break;
						}
						case "MazeBG": {
							BGName = "Labirinthian";
							break;
						}
						default: {
							BGName = "Intersection";
							break;
						}

					}
					if (saveInfo.data.core.time != 0) {
						save.GetComponentInChildren<Text>().text = "Difficulty: " + (saveInfo.data.core.difficulty + 1) + "\n" +
																	"Loaction: " + BGName + "\n" + "Attempt " +
																	"Time: " + string.Format("{0:00}:{1:00}.{2:00} minutes", (int)saveInfo.data.core.time / 60, saveInfo.data.core.time % 60, saveInfo.data.core.time.ToString().Remove(0, saveInfo.data.core.time.ToString().Length - 2)) + "\n" +
																	"Spikes: " + saveInfo.data.player.spikesCollected + " Bullets: " + saveInfo.data.player.bullets + "\n" +
																	"Coins: " + saveInfo.data.player.coinsCollected + " Bombs: " + saveInfo.data.player.bombs;
					}
					else {
						save.GetComponentInChildren<Text>().text = "Difficulty: " + (saveInfo.data.core.difficulty + 1) + "\n" +
																	"Loaction: " + BGName + "\n" +
																	"Time: 00:00:00 minutes";

					}
				}
			}
			if (content.GetComponentsInChildren<RectTransform>().Length == 1) {

				GameObject noSave = Instantiate(NoSaves, GameObject.Find("Canvas").transform);
				noSave.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
			}
		}
	}

	public void DisableButtons() {
		foreach (Button item in gameObject.GetComponentsInChildren<Button>()) {
			item.interactable = false;
		}
	}
}
