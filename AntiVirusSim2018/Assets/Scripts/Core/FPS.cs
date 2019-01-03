using UnityEngine;

public class FPS : MonoBehaviour {

	private float deltaTime = 0.0f;
	private GUIStyle style;
	private int w, h;
	private Rect rect;


	void Start() {
#if UNITY_STANDALONE
		int quality = QualitySettings.GetQualityLevel();
		if (quality != 6) {
			enabled = false;
			return;
		}
#endif
		style = new GUIStyle();
		w = Screen.width;
		h = Screen.height;
		rect = new Rect(w / 2, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 50;
		style.normal.textColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	}


	void Update() {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

	void OnGUI() {
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		float currdelta = Time.deltaTime;
		string text = string.Format("{0:00.0} ms ({1:000} fps) ({2:00.0000} Delta)", msec, fps, currdelta);
		GUI.Label(rect, text, style);
	}
}
