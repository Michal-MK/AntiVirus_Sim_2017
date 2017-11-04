using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour {

	public static Control script;
	public SaveManager saveManager;
	public LoadManager loadManager;
	public MapData mapData;

	public int chosenDifficulty;
	private int attempt;
	private int localAttempt;

	public GameObject spikeInScene;
	public GameObject authentication;
	public bool isNewGame = true;
	public bool isRestarting = false;

	private bool load = false;

	public static SaveData currSaveFile;

	public static int currAttempt = 0;
	public static int currDifficulty = 0;

	void Awake() {
		if (script == null) {
			script = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (script != this) {
			Destroy(gameObject);
		}

		if (!Directory.Exists(Application.dataPath + "/Saves")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D0")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D0");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D1")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D1");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D2")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D2");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D3")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D3");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D4")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D4");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D0/Resources")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D0/Resources");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D1/Resources")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D1/Resources");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D2/Resources")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D2/Resources");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D3/Resources")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D3/Resources");
		}
		if (!Directory.Exists(Application.dataPath + "/Saves/D4/Resources")) {
			Directory.CreateDirectory(Application.dataPath + "/Saves/D4/Resources");
		}
		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}

	public IEnumerator SetName() {
		if (PlayerPrefs.GetString("player_name") == null || PlayerPrefs.GetString("player_name") == "") {
			yield return new WaitForSeconds(1f);
			Wrapper wrp = GameObject.Find("Canvas").GetComponent<Wrapper>();
			wrp.ToggleButtonInteractivity();
			GameObject auth = Instantiate(authentication);
			auth.transform.SetParent(GameObject.Find("Canvas").transform);
			auth.transform.localPosition = Vector3.zero;
			yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
			wrp.ToggleButtonInteractivity();

		}
	}

	private IEnumerator ScreenShot(int currAttempt) {
		GameObject saveButton = GameObject.Find("saveGame");
		if (saveButton != null) {
			yield return new WaitUntil(() => !saveButton.activeInHierarchy);
		}
		print("Captured");
		ScreenCapture.CaptureScreenshot(Application.dataPath + "/Saves/D" + chosenDifficulty + "/Resources/Save-D" + chosenDifficulty + "_" + currAttempt.ToString("000") + ".png");
	}

	public IEnumerator StartNewGame(int difficulty) {

		PlayerPrefs.SetInt("difficulty", difficulty);
		chosenDifficulty = difficulty;
		CamFadeOut.script.PlayTransition(CamFadeOut.CameraModeChanges.TRANSITION_SCENES);
		yield return new WaitForFixedUpdate();
		load = false;
		yield return new WaitUntil(() => CamFadeOut.script.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f);
		AsyncOperation loading = SceneManager.LoadSceneAsync(1);
		CamFadeOut.script.anim.speed = 0;

		yield return new WaitUntil(() => load == true);
		loading.allowSceneActivation = true;
		CamFadeOut.script.anim.speed = 1;
		load = false;
	}


	public void Restart() {
		M_Player.doNotMove = false;
		Spike.spikesCollected = 0;
		Coins.coinsCollected = 0;
		PlayerAttack.bombs = 0;
		PlayerAttack.bullets = 0;
		M_Player.gameProgression = 0;
		Projectile.projectileSpeed = 15;
		Timer.time = 0f;
		Time.timeScale = 1;
		script.isRestarting = true;
		print(script.isRestarting);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		CamFadeOut.script.anim.SetTrigger("UnDim");
	}

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
		if (scene.buildIndex == 0 && PlayerPrefs.GetString("player_name") == null) {
			SetName();
		}

		if (scene.buildIndex == 1 && isNewGame) {
			CamFadeOut.script.anim.speed = 1;
		}
		else if (isRestarting) {
			isNewGame = false;
		}
	}

	private void OnDestroy() {
		SceneManager.sceneLoaded -= OnSceneFinishedLoading;
	}
}