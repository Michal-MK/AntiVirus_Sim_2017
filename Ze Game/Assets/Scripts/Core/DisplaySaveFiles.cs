using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplaySaveFiles : MonoBehaviour {
	string dataPath;
	string BGName;
	public GameObject SaveObj;
	public GameObject NoSaves;

	public GameObject lastSave;

	public RectTransform content;

	private void Awake() {
		Statics.displaySaves = this;
		dataPath = Application.dataPath + "/Saves/";
	}

	void Start() {
		DisplaySaves();
	}

	public void DisplaySaves() {
		DirectoryInfo dir = new DirectoryInfo(dataPath);
		DirectoryInfo[] info = dir.GetDirectories();

		foreach (DirectoryInfo saveDirectory in info) {
			//print(saveDirectory.FullName);

			FileInfo[] saveFiles = saveDirectory.GetFiles("*.Kappa");
			for (int i = 0; i < saveFiles.Length; i++) {
				string correctName = saveFiles[i].FullName.Replace('\\', '/');
				string savePath = correctName;

				GameObject save = Instantiate(SaveObj, content.transform);
				save.name = saveFiles[i].FullName;
				lastSave = save;
				byte[] img = File.ReadAllBytes(saveDirectory + "/Resources/" + saveFiles[i].Name.Remove(saveFiles[i].Name.Length - 6, 6) + ".png");
				Texture2D tex = new Texture2D(800, 600);
				tex.LoadImage(img);
				Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
				save.transform.Find("SaveImage").GetComponent<Image>().sprite = sp;
				//save.GetComponentInChildren<Image>().sprite = sp;


				FileStream file = new FileStream(savePath, FileMode.Open);
				BinaryFormatter br = new BinaryFormatter();

				SaveData saveInfo = (SaveData)br.Deserialize(file);
				file.Close();

				switch (saveInfo.currentBGName) {
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
				if (saveInfo.time != 0) {
					save.GetComponentInChildren<Text>().text = "Difficulty: " + (saveInfo.difficulty + 1) + "\n" +
																"Loaction: " + BGName + "\n" +
																"Time: " + string.Format("{0:00}:{1:00}.{2:00} minutes", (int)saveInfo.time / 60, saveInfo.time % 60, saveInfo.time.ToString().Remove(0, saveInfo.time.ToString().Length - 2)) + "\n" +
																"Spikes: " + saveInfo.spikesCollected + " Bullets: " + saveInfo.bullets + "\n" +
																"Coins: " + saveInfo.coinsCollected + " Bombs: " + saveInfo.bombs; 
				}
				else {
					save.GetComponentInChildren<Text>().text = "Difficulty: " + (saveInfo.difficulty + 1) + "\n" +
											"Loaction: " + BGName + "\n" +
											"Time: 00:00:00 minutes";

				}
				content.sizeDelta += new Vector2(0, 225);
			}
		}
		if (content.GetComponentsInChildren<RectTransform>().Length == 1) {

			GameObject noSave = Instantiate(NoSaves, GameObject.Find("Scroll View").transform);
			noSave.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
		}
	}

	public void DisableButtons() {
		foreach (Button item in gameObject.GetComponentsInChildren<Button>()) {
			item.interactable = false;
		}
	}
	private void Update() {
		EventSystem e = EventSystem.current;

		if (lastSave != null && e.currentSelectedGameObject == null) {
			e.SetSelectedGameObject(lastSave);
		}
	}


	private void OnDestroy() {
		Statics.displaySaves = null;
	}
}
