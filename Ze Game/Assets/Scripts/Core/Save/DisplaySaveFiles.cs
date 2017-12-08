using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Igor.Constants.Strings;

public class DisplaySaveFiles : MonoBehaviour {
	string BGName;
	public GameObject SaveObj;
	public GameObject NoSaves;

	public RectTransform content;

	public static int selectedAttempt;

	void Start() {
		DisplaySaves();
	}

	public void DisplaySaves() {
		DirectoryInfo dir = new DirectoryInfo(Application.dataPath + Path.DirectorySeparatorChar + "Saves" + Path.DirectorySeparatorChar);
		DirectoryInfo[] info = dir.GetDirectories();

		foreach (DirectoryInfo saveDirectory in info) {

			FileInfo[] saveFiles = saveDirectory.GetFiles("*.Kappa");
			for (int i = 0; i < saveFiles.Length; i++) {

				using (FileStream file = new FileStream(saveFiles[i].FullName, FileMode.Open)) {
					BinaryFormatter br = new BinaryFormatter();

					SaveFile saveInfo = (SaveFile)br.Deserialize(file);

					GameObject save = Instantiate(SaveObj, content.transform);
					Text saveInfoText = save.transform.Find("SaveInfo").GetComponent<Text>();
					RawImage saveImg = save.transform.Find("SaveImage").GetComponent<RawImage>();
					Button showHistory = save.transform.Find("ShowHistory").GetComponent<Button>();
					Button deleteSave = save.transform.Find("DeleteButton").GetComponent<Button>();
					Button confirmDel = save.transform.Find("Confirm").GetComponent<Button>();
					Button cancelDel = save.transform.Find("Cancel").GetComponent<Button>();

					save.name = saveFiles[i].FullName;
					byte[] img = File.ReadAllBytes(saveInfo.data.core.imgFileLocation);
					Texture2D tex = new Texture2D(800, 600);
					tex.LoadImage(img);
					saveImg.texture = tex;
					confirmDel.GetComponent<SaveFileScript>().saveFile = saveInfo;

					try {
						save.transform.Find("ShowHistory").GetComponent<DisplaySaveHistory>().selfHistory = saveInfo.saveHistory.previousSaves;
					}
					catch {
						print("Failed to test");
					}

					switch (saveInfo.data.player.currentBGName) {
						case BackgroundNames.BACKGROUND1_1: {
							BGName = "Electical Hall";
							break;
						}
						case BackgroundNames.BACKGROUND1_2: {
							BGName = "Icy Plains";
							break;
						}
						case BackgroundNames.BACKGROUND1_3: {
							BGName = "Danger Zone";
							break;
						}
						case BackgroundNames.BACKGROUND1_4: {
							BGName = "Peaceful Corner";
							break;
						}
						case BackgroundNames.BACKGROUND_BOSS_ + "1": {
							BGName = "Boss Area";
							break;
						}
						case BackgroundNames.BACKGROUND1_MAZE: {
							BGName = "Labirinthian";
							break;
						}
						default: {
							BGName = "Intersection";
							break;
						}

					}
					if (saveInfo.data.core.time != 0) {
						saveInfoText.text = "Difficulty: " + (saveInfo.data.core.difficulty + 1) + "\n" +
																	"Loaction: " + BGName + "\n" + "Attempt " +
																	"Time: " + string.Format("{0:00}:{1:00}.{2:00} minutes", (int)saveInfo.data.core.time / 60, saveInfo.data.core.time % 60, saveInfo.data.core.time.ToString().Remove(0, saveInfo.data.core.time.ToString().Length - 2)) + "\n" +
																	"Spikes: " + saveInfo.data.player.spikesCollected + " Bullets: " + saveInfo.data.player.bullets + "\n" +
																	"Coins: " + saveInfo.data.player.coinsCollected + " Bombs: " + saveInfo.data.player.bombs;
					}
					else {
						saveInfoText.text = "Difficulty: " + (saveInfo.data.core.difficulty + 1) + "\n" +
																	"Loaction: " + BGName + "\n" +
																	"Time: 00:00:00 minutes";

					}
					deleteSave.onClick.AddListener(
						delegate {
							EventSystem.current.SetSelectedGameObject(cancelDel.gameObject);
							showHistory.interactable = false;
						}	
					);
					cancelDel.onClick.AddListener(
						delegate {
							EventSystem.current.SetSelectedGameObject(deleteSave.gameObject);
							showHistory.interactable = true;
						}
					);
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
