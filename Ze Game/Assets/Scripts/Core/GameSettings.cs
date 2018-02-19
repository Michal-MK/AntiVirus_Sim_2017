using UnityEngine;
using System.Collections;
using System.IO;

public class GameSettings : MonoBehaviour {

	public static GameSettings script;

	private static string path;

	private float masterAudio;

	private float musicAudio;
	private float fxAudio;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init() {
		path = Application.dataPath + Path.DirectorySeparatorChar + "Settings" + Path.DirectorySeparatorChar + "config.cfg";
		if (!File.Exists(path)) {
			using (StreamWriter sr = File.CreateText(path)) {
				sr.WriteLine("# This is a configuration file for AVSim game.");
				sr.WriteLine("# This is a coment and will be ingnored by the parser");
				sr.WriteLine("# Everything in here was generated automatically and can be changed from the Ingame interface,\n" +
							 "unless you know what you are doing, you should close this file");
				sr.WriteLine("<audio_settings");
				sr.WriteLine();
				sr.WriteLine("master_audio = 100");
				sr.WriteLine("music_audio = 100");
				sr.WriteLine("effect_audio = 100");
				sr.WriteLine();
				sr.WriteLine("audio_settings/>");
			}
		}
	}

	private void Awake() {
		if (script == null) {
			script = this;
		}
		else if (script != this) {
			Destroy(script.gameObject);
		}

		Validate(File.OpenText(path));
	}

	// Use this for initialization
	void Start() {
	}

	private void Validate(StreamReader sr) {
		while (!sr.EndOfStream) {
			string line = sr.ReadLine();
			if (line.StartsWith("<")) {

			}
		}
	}
}
