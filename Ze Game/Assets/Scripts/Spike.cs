using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spike : MonoBehaviour {
	public CameraMovement cam;
	public RectTransform BGS;
	public RectTransform BG1;
	public RectTransform BG2a;
	public GameObject player;
	public Guide guide;
	public EnemySpawner spawn;
	public GameObject teleporter;
	Animator anim;

	public static int spikesCollected = 0;



	void Start() {

		player = GameObject.Find("Player");
		anim = GameObject.Find("scoreText").GetComponent<Animator>();
		anim.Play("TransformPos");

		if (PlayerPrefs.HasKey("difficulty") == false) {
			PlayerPrefs.SetInt("difficluty", 0);
		}
	}

	private void OnEnable() {
		guide.enableGuide();
		guide.Recalculate(gameObject, true);
	}

	void OnTriggerEnter2D(Collider2D col) {

		if (col.tag == "Player") {

			spikesCollected = spikesCollected + 1;

			M_Player.gameProgression = spikesCollected;

			gameObject.SetActive(false);

			guide.disableGuide();
		}
		if (spikesCollected >= 0 || spikesCollected <= 4) {
			anim.Play("Highlight Text");
			
		}


		if (spikesCollected == 5) {
			anim.Play("TransformPos");
			RectTransform lastBG = GameObject.Find("Background_room_3").GetComponent<RectTransform>();
			M_Player.gameProgression = 1;
			GameObject bossTeleporter = Instantiate(teleporter, new Vector3(lastBG.position.x, lastBG.position.y, 0), Quaternion.identity);
			bossTeleporter.transform.SetParent(gameObject.transform.parent);
			bossTeleporter.name = "Boss1_teleporter";
			guide.disableGuide();
		}
	}
	public void SetPosition() {
		int stage = M_Player.gameProgression;
		

		float Xscale = gameObject.transform.lossyScale.x / 2;
		float Yscale = gameObject.transform.lossyScale.y / 2;

		if (stage == 0) {

			float x = BGS.position.x;
			float y = BGS.position.y;
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.SetActive(true);
			guide.enableGuide();
			guide.Recalculate(gameObject, true);

		}
		if (stage == 1) {


			float x = Random.Range(BG1.position.x + (-BG1.sizeDelta.x / 2) + Xscale, BG1.position.x + (BG1.sizeDelta.x / 2) - Xscale);
			float y = Random.Range(BG1.position.y + (-BG1.sizeDelta.y / 2) + Yscale, BG1.position.y + (BG1.sizeDelta.y / 2) - Yscale);
			float z = 0f;


			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.SetActive(true);
			guide.enableGuide();
			guide.Recalculate(gameObject, true);

		}
		if (stage == 2) {

			float x = Random.Range(BG2a.position.x + (-BG2a.sizeDelta.x / 2) + Xscale, BG2a.position.x + (BG2a.sizeDelta.x / 2) - Xscale);
			float y = Random.Range(BG2a.position.y + (-BG2a.sizeDelta.y / 2) + Yscale, BG2a.position.y + (BG2a.sizeDelta.y / 2) - Yscale);
			float z = 0f;

			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.SetActive(true);
			guide.enableGuide();
			guide.Recalculate(gameObject, true);

		}
		//if (stage == 3) {
		//	print(stage);
		//	float x = Random.Range(-BG.sizeDelta.x / 2 + Xscale, BG.sizeDelta.x / 2 - Xscale);
		//	float y = Random.Range(-BG.sizeDelta.y / 2 + Yscale, BG.sizeDelta.y / 2 - Yscale);
		//	float z = 0f;

		//	gameObject.transform.position = new Vector3(x, y, z);
		//	gameObject.SetActive(true);
		//	guide.enableGuide();
		//	guide.Recalculate(gameObject, true);

		//}
		//if (stage == 4) {
		//	print(stage);
		//	float x = Random.Range(-BG.sizeDelta.x / 2 + Xscale, BG.sizeDelta.x / 2 - Xscale);
		//	float y = Random.Range(-BG.sizeDelta.y / 2 + Yscale, BG.sizeDelta.y / 2 - Yscale);
		//	float z = 0f;

		//	gameObject.transform.position = new Vector3(x, y, z);
		//	gameObject.SetActive(true);
		//	guide.enableGuide();
		//	guide.Recalculate(gameObject, true);

		//}
		if (stage == 5) {
			print(stage);
			gameObject.SetActive(false);
		}
	}
}

