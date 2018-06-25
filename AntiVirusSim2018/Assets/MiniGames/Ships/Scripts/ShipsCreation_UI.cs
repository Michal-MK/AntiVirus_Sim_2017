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
		GameObject.Find("Ship_HP").GetComponent<InputField>().text = ShipEdit.getCurrentShip.totalShipParts.ToString();
	}

	public void GetSaveInfo() {
		_shipName = GameObject.Find("Ship_Name").GetComponent<InputField>().text;
		bool success = int.TryParse(GameObject.Find("Ship_HP").GetComponent<InputField>().text,out _shipHP);
		if (!success) {
			_shipHP = ShipEdit.getCurrentShip.totalShipParts;
		}
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
