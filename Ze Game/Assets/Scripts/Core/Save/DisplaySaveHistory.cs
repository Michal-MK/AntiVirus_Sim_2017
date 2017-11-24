using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class DisplaySaveHistory : MonoBehaviour {
	private bool isDisplaying = false;

	public GameObject historyRep;

	private Transform content;
	public List<SaveData> selfHistory;

	public void Display() {
		if (!isDisplaying) {
			content = GameObject.Find("SaveSceneReferenceHolder").GetComponent<SaveSceneReferenceHolder>().content;
			foreach (SaveData s in selfHistory) {
				GameObject g = Instantiate(historyRep, content);
				g.name = s.core.fileLocation;
				g.transform.SetAsLastSibling();
				g.GetComponent<SaveFileScript>().associatedData = s;
				Text t = g.transform.Find("SaveInfo").GetComponent<Text>();
				RawImage ri = g.transform.Find("SaveImage").GetComponent<RawImage>();
				string BGName;

				switch (s.player.currentBGName) {
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

				if (s.core.time != 0) {
					t.text = "Loaction: " + BGName + "\n" + "Attempt " +
							"Time: " + string.Format("{0:00}:{1:00}.{2:00} minutes", (int)s.core.time / 60, s.core.time % 60, s.core.time.ToString().Remove(0, s.core.time.ToString().Length - 2)) + "\n" +
							"Spikes: " + s.player.spikesCollected + " Bullets: " + s.player.bullets + "\n" +
							"Coins: " + s.player.coinsCollected + " Bombs: " + s.player.bombs;
				}
				else {
					t.text = "Loaction: " + BGName + "\n" +
							"Time: 00:00:00 minutes" + "\n" +
							"New Game";
				}
				Texture2D tex = new Texture2D(800, 600);
				bool success = tex.LoadImage(File.ReadAllBytes(s.core.imgFileLocation));
				if (success) {
					ri.texture = tex;
				}
				else {
					ri.texture = null;
				}
			}
			transform.GetComponentInChildren<Text>().text = "Hide History";
			isDisplaying = true;
		}
		else {
			foreach (Transform g in content.GetComponentsInChildren<Transform>()) {
				if (g != content) {
					Destroy(g.gameObject);
				}
			}
			transform.GetComponentInChildren<Text>().text = "Show History";
			isDisplaying = false;
		}
	}
}
