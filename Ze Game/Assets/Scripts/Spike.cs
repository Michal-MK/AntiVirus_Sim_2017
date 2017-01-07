using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spike : MonoBehaviour {
	public CameraMovement cam;
	public GameObject deathBlock;
	public RectTransform BG;
	public GameObject player;
	public Guide guide;
	public EnemySpawner spawn;

	public static int i = 0;



	void Start(){
		player = GameObject.Find ("Player");

		if (PlayerPrefs.HasKey ("difficulty") == false) {
			PlayerPrefs.SetInt ("difficluty", 0);
		}
	}


	void OnTriggerEnter2D (Collider2D col){
		timer.run = true;
		Vector3 old_pos = transform.position;
		float Xscale = gameObject.transform.lossyScale.x / 2;
		float Yscale = gameObject.transform.lossyScale.y / 2;


		if (col.name == player.name) {
			Vector3 newpos = transform.position;

			while (Mathf.Abs(Vector3.Distance(newpos, old_pos)) < 30) {

				float x = Random.Range (-BG.sizeDelta.x/2 + Xscale , BG.sizeDelta.x/2 - Xscale);
				float y = Random.Range (-BG.sizeDelta.y/2 + Yscale, BG.sizeDelta.y/2 - Yscale);
				float z = 0f;



				newpos = new Vector3(x, y, z);


			}
				
			gameObject.transform.position = newpos;


			guide.SendMessage ("init");

			i = i+1;





			if (i == 1) {
				M_Player.gameProgression = 1;
				player.GetComponent<roomPregression>().Progress();
				cam.Progress ();
				spawn.spawnKillerBlock ();
			}

			if (i == 2) {
				M_Player.gameProgression = 2;
				player.GetComponent<roomPregression>().Progress();
				spawn.spawnKillerBlock ();
			}

			if (i == 3) {
				M_Player.gameProgression = 3;
				player.GetComponent<roomPregression>().Progress();
				spawn.spawnKillerBlock ();
			}


			if (i == 4) {
				M_Player.gameProgression = 4;
				player.GetComponent<roomPregression>().Progress();
				spawn.spawnKillerBlock ();
			}

			if (i == 5) {
				
				//M_Player.gameProgression = 10;
				player.SendMessage ("GameOver");
				gameObject.SetActive (false);
			}
		}
	}

	public void saveScore(){
		int difficulty = PlayerPrefs.GetInt ("difficulty");

		if (difficulty == 0) {


			int q = 0;
			int count = 10;


			PlayerPrefs.SetFloat (count.ToString (), Mathf.Round (timer.time_er * 1000) / 1000);
			Debug.Log (PlayerPrefs.GetFloat ("10"));
			while (q < count) {

				if (PlayerPrefs.HasKey ((count - 1).ToString ()) == true) {

					if (PlayerPrefs.GetFloat ((count - 1).ToString ()) > PlayerPrefs.GetFloat (count.ToString ())) {

						float temp = PlayerPrefs.GetFloat ((count - 1).ToString ()); 
						PlayerPrefs.SetFloat ((count - 1).ToString (), PlayerPrefs.GetFloat (count.ToString ()));
						PlayerPrefs.SetFloat (count.ToString (), temp);
					}
					count -= 1;
					PlayerPrefs.SetFloat ("10", 500f);
				} else {
					count = -1;
				}
			}
		}

		if (difficulty == 1) {

			int q = 11;
			int count = 21;


			PlayerPrefs.SetFloat (count.ToString (), Mathf.Round (timer.time_er * 1000) / 1000);

			while (q < count) {

				if (PlayerPrefs.HasKey ((count - 1).ToString ()) == true) {

					if (PlayerPrefs.GetFloat ((count - 1).ToString ()) > PlayerPrefs.GetFloat (count.ToString ())) {

						float temp = PlayerPrefs.GetFloat ((count - 1).ToString ()); 
						PlayerPrefs.SetFloat ((count - 1).ToString (), PlayerPrefs.GetFloat (count.ToString ()));
						PlayerPrefs.SetFloat (count.ToString (), temp);
					}
					count -= 1;
					PlayerPrefs.SetFloat ("21", 500f);
				} else {
					count = -1;
				}
			}
	
		}

		if (difficulty == 2) {

			int q = 22;
			int count = 32;


			PlayerPrefs.SetFloat (count.ToString (), Mathf.Round (timer.time_er * 1000) / 1000);

			while (q < count) {

				if (PlayerPrefs.HasKey ((count - 1).ToString ()) == true) {

					if (PlayerPrefs.GetFloat ((count - 1).ToString ()) > PlayerPrefs.GetFloat (count.ToString ())) {

						float temp = PlayerPrefs.GetFloat ((count - 1).ToString ()); 
						PlayerPrefs.SetFloat ((count - 1).ToString (), PlayerPrefs.GetFloat (count.ToString ()));
						PlayerPrefs.SetFloat (count.ToString (), temp);
					}
					count -= 1;
					PlayerPrefs.SetFloat ("32", 500f);
				} else {
					count = -1;
				}
			}

		}

		if (difficulty == 3) {

			int q = 33;
			int count = 43;


			PlayerPrefs.SetFloat (count.ToString (), Mathf.Round (timer.time_er * 1000) / 1000);

			while (q < count) {

				if (PlayerPrefs.HasKey ((count - 1).ToString ()) == true) {

					if (PlayerPrefs.GetFloat ((count - 1).ToString ()) > PlayerPrefs.GetFloat (count.ToString ())) {

						float temp = PlayerPrefs.GetFloat ((count - 1).ToString ()); 
						PlayerPrefs.SetFloat ((count - 1).ToString (), PlayerPrefs.GetFloat (count.ToString ()));
						PlayerPrefs.SetFloat (count.ToString (), temp);
					}
					count -= 1;
					PlayerPrefs.SetFloat ("43", 500f);
				} else {
					count = -1;
				}
			}


		}

		if (difficulty == 4) {
			
			int q = 44;
			int count = 54;


			PlayerPrefs.SetFloat (count.ToString (), Mathf.Round (timer.time_er * 1000) / 1000);

			while (q < count) {

				if (PlayerPrefs.HasKey ((count - 1).ToString ()) == true) {

					if (PlayerPrefs.GetFloat ((count - 1).ToString ()) > PlayerPrefs.GetFloat (count.ToString ())) {

						float temp = PlayerPrefs.GetFloat ((count - 1).ToString ()); 
						PlayerPrefs.SetFloat ((count - 1).ToString (), PlayerPrefs.GetFloat (count.ToString ()));
						PlayerPrefs.SetFloat (count.ToString (), temp);
					}
					count -= 1;
					PlayerPrefs.SetFloat ("54", 500f);
				} else {
					count = -1;
				}
			}
		
		}
//		for (int i = 9; i > 0; i--) {
//			Debug.Log (i.ToString () + "    " + PlayerPrefs.GetFloat (i.ToString ()));
//		}
	}
}

