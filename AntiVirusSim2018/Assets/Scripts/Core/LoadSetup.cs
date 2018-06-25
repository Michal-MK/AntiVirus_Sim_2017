using UnityEngine;

public class LoadSetup : MonoBehaviour {
	public SaveManager sm;
	public MapData md;

	void Start () {
		Control c = Control.script;
		c.saveManager = sm;
		c.mapData = md;
		Destroy(this);
	}
}
