using UnityEngine;

public class LoadSetup : MonoBehaviour {

	public LoadManager lm;
	public SaveManager sm;
	public MapData md;

	void Start () {
		Control c = Control.script;
		c.loadManager = lm;
		c.saveManager = sm;
		c.mapData = md;
	}
}
