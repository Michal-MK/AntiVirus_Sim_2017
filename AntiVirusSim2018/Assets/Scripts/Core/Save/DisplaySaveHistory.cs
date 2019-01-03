using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using Igor.Constants.Strings;

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
				string BGName = BackgroundNames.GetRealName(s.player.currentBGName);

				if (s.core.time != 0) {
					t.text = "Loaction: " + BGName + "\n" + "Attempt " +
							 "Time: " + string.Format("{0:00}:{1:00}.{2:00} {3}", (int)s.core.time / 60, s.core.time % 60, s.core.time.ToString().Remove(0, s.core.time.ToString().Length - 2), (int)s.core.time / 60 == 0 ? "seconds." : "minutes.") + "\n" +
							 "Spikes: " + s.player.spikesCollected + " Bullets: " + s.player.bullets + "\n" +
							 "Coins: " + s.player.coinsCollected + " Bombs: " + s.player.bombs;
				}
				else {
					t.text = "Loaction: " + BGName + "\n" +
							 "Time: 00:00:00" + "\n" +
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
			if (selfHistory.Count <= 0) {
				GameObject noHistory = new GameObject();
				noHistory.transform.SetParent(content);
				noHistory.AddComponent<CanvasRenderer>();
				Text t = noHistory.AddComponent<Text>();
				t.font = historyRep.transform.Find("SaveInfo").GetComponent<Text>().font;
				t.fontSize = 40;
				t.color = Color.red;
				t.alignment = TextAnchor.UpperCenter;
				t.text = "No save under this game yet!";
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
