using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Igor.Minigames.Ships {
	public class CustomShips : MonoBehaviour {

		public RectTransform scrollView;
		public RectTransform panel;
		private Ships_UI ui;

		public GameObject preview;

		private void Start() {
			ui = GameObject.Find("Canvas").GetComponent<Ships_UI>();
			DirectoryInfo d = new DirectoryInfo(Application.dataPath + Path.DirectorySeparatorChar + "Ships" + Path.DirectorySeparatorChar);
			foreach (FileInfo f in d.GetFiles("*.txt")) {
				CustomShipPreview customShipPrew = Instantiate(preview, scrollView).GetComponent<CustomShipPreview>();
				using (StreamReader read = File.OpenText(f.FullName)) {
					customShipPrew.transform.Find("Name").GetComponent<Text>().text = read.ReadLine();
					//int hp = int.Parse(read.ReadLine());
					string boolean = read.ReadLine();
					bool canRotate = false;
					if(boolean == "true") {
						canRotate = true;
					}
					string[,] s = new string[9, 9];
					for (int i = 0; i < 9; i++) {
						string str = read.ReadLine();
						for (int j = 0; j < 9; j++) {
							s[i, j] = str[j].ToString();
						}
					}
					customShipPrew.file_grid = s;
					customShipPrew.canRotate = canRotate;
					customShipPrew.main_UI = this.ui;
				}
			}
		}

		public Ships_UI ship_UI {
			get { return ui; }
			set { ui = value; }
		}
	}
}