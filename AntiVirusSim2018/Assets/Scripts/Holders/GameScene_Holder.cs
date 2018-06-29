using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene_Holder : MonoBehaviour {
	public Button save;
	public Button restart;
	public Button quitToMenu;
	public Button load;
	public Button settings;

	public Button[] getButtonsSave {
		get { return new Button[] { save, restart, quitToMenu }; }
	}

	public Button[] getButtonLoad {
		get { return new Button[] { load, restart, quitToMenu }; }
	}
}
