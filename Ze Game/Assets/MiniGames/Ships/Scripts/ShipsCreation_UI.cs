using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Igor.Minigames.Ships;

public class ShipsCreation_UI : MonoBehaviour {

	private string _shipName;
	private int _shipHP;
	private bool _allowRotation;

	public void SetCusrorMode(int mode) {
		ShipsCreationMain.script.mode = (ShipsCreationMain.CursorMode)mode;
	}

	public void SaveSetup(GameObject saveWindow) {
		saveWindow.SetActive(!saveWindow.activeInHierarchy);
	}

	public void GetSaveInfo() {
		_shipName = GameObject.Find("Ship_Name").GetComponent<InputField>().text;
		_shipHP = int.Parse(GameObject.Find("Ship_HP").GetComponent<InputField>().text);
		_allowRotation = GameObject.Find("Allow_Rotation").GetComponent<Toggle>().isOn;
	}

	public string shipName {
		get { return _shipName; }
	}

	public int shipHP {
		get { return _shipHP; }
	}

	public bool allowShipRotation {
		get { return _allowRotation; }
	}
}
