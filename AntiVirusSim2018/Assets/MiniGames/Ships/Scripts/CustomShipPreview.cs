using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Igor.Minigames.Ships;

public class CustomShipPreview : MonoBehaviour {

	public bool canRotate = true;
	public string[,] file_grid = new string[9, 9];
	private List<GameObject> parts = new List<GameObject>();
	public Ships_UI main_UI;

	public void OnPointerEnter() {
		GameObject g = GameObject.Find("Customs").transform.Find("Background/Holder").gameObject;
		Image[] prevBoxes = g.GetComponentsInChildren<Image>();
		for (int i = 0; i < 81; i++) {
			//print(Mathf.FloorToInt(i / 9) + "  " + i % 9);
			string selected = file_grid[Mathf.FloorToInt(i / 9), i % 9];
			if (selected == "_") {
				prevBoxes[i].color = new Color(1, 1, 1, 0);
			}
			else if (selected == "#") {
				prevBoxes[i].color = new Color(1, 1, 1, 1);
				parts.Add(prevBoxes[i].gameObject);
			}
		}
	}

	public void OnPointerExit() {
		parts.Clear();
	}

	public void OnPointerClick() {
		GameObject visual = main_UI.prefabs.SpawnCustomVisual(file_grid);
		main_UI.SetSelectedShipCustom(visual, canRotate);
		Destroy(transform.parent.parent.parent.parent.gameObject);
	}
}
