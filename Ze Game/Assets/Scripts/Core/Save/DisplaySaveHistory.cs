using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DisplaySaveHistory : MonoBehaviour {

	public GameObject historyRep;

	public Transform content;
	public List<SaveData> selfHistory;

	public void Display() {
		foreach (SaveData s in selfHistory) {
			GameObject g = Instantiate(historyRep, content);
			g.name = s.core.fileLocation;
			g.transform.SetAsLastSibling();
			g.GetComponent<SaveFileScript>().associatedData = s;
			Text t = g.transform.Find("SaveInfo").GetComponent<Text>();
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
				t.text ="Loaction: " + BGName + "\n" + "Attempt " +
						"Time: " + string.Format("{0:00}:{1:00}.{2:00} minutes", (int)s.core.time / 60, s.core.time % 60, s.core.time.ToString().Remove(0, s.core.time.ToString().Length - 2)) + "\n" +
						"Spikes: " + s.player.spikesCollected + " Bullets: " + s.player.bullets + "\n" +
						"Coins: " + s.player.coinsCollected + " Bombs: " + s.player.bombs;
			}
			else {
				t.text ="Loaction: " + BGName + "\n" +
						"Time: 00:00:00 minutes" + "\n" + 
						"New Game";
			}
		}
	}
}
