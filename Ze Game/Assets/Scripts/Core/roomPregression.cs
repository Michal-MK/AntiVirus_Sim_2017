using UnityEngine;


public class roomPregression : MonoBehaviour {

	public GameObject[] doors;
	public Sprite sprtOff;
	public Sprite sprtOn;
	public Canvas_Renderer canvas_Renderer;
	public static roomPregression script;


	SpriteRenderer ic_S;
	SpriteRenderer ic_1a;
	SpriteRenderer ic_1b;


	void Awake(){
		script = this;
	}


	void Start(){
		ic_S = GameObject.Find ("Door_status_S").GetComponent <SpriteRenderer>();
		ic_1a = GameObject.Find ("Door_status_1a").GetComponent <SpriteRenderer>();
		ic_1b = GameObject.Find ("Door_status_1b").GetComponent <SpriteRenderer>();
	}

	public void Progress () {

		if (M_Player.gameProgression == 1) {
			foreach (GameObject door in doors) {
				door.SetActive (true);
			}
			ic_1a.sprite = sprtOff;
			doors [0].SetActive (false);
			doors [1].SetActive (false);
			ic_S.sprite = sprtOn;
			canvas_Renderer.DisplayDirection(1);
		}

		if (M_Player.gameProgression == 2) {
			foreach (GameObject door in doors) {
				door.SetActive (true);
			}
			doors [0].SetActive (false);
			doors [1].SetActive (false);
			doors [2].SetActive (false);
			doors [3].SetActive (false);
			ic_1a.sprite = sprtOn;
			canvas_Renderer.DisplayDirection(0);

		}
		if (M_Player.gameProgression == 3) {
			foreach (GameObject door in doors) {
				door.SetActive (true);
			}
			doors [0].SetActive (false);
			doors [1].SetActive (false);
			doors [2].SetActive (false);
			doors [3].SetActive (false);
			doors [4].SetActive (false);
			doors [5].SetActive (false);
			ic_1b.sprite = sprtOn;
			canvas_Renderer.DisplayDirection(2);


		}
		Camera.main.GetComponent <CameraMovement>().raycastForRooms ();
	}

	//public static void resetGame() {
	//	M_Player.gameProgression = 0;
	//	Spike.spikesCollected = 0;
	//	Coins.coinsCollected = 0;
	//	Projectile.projectileSpeed = 15;

	//}
}
