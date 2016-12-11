using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spike : MonoBehaviour {
	public CameraMovement cam;
	public GameObject deathBlock;
	public RectTransform BG;
	GameObject player;
	public Boundary bounds;
	public Guide guide;



	void Start(){
		player = GameObject.Find ("Asset");


	}

	public int i = 0;

	void OnTriggerEnter2D (Collider2D col){
		timer.run = true;
		Vector3 old_pos = transform.position;
		float Xscale = gameObject.transform.lossyScale.x / 2;
		float Yscale = gameObject.transform.lossyScale.y / 2;


		if (col.name == "Asset") {
			Vector3 newpos = transform.position;

			while (Mathf.Abs(Vector3.Distance(newpos, old_pos)) < 10) {

				float x = Random.Range ((-BG.sizeDelta.x/10)/2 + Xscale , (BG.sizeDelta.x/10)/2) - Xscale;
				float y = Random.Range ((-BG.sizeDelta.y/10)/2 + Yscale, (BG.sizeDelta.y/10)/2) - Yscale;
				float z = 0f;

				newpos = new Vector3(x, y, z);

			}
				
			gameObject.transform.position = newpos;

			//Debug.Log ("Current pos: " + newpos.ToString ());

			guide.SendMessage ("init");

			i = i+1;


			for (int count = 0; count < (int)(i + 5 * difficultySlider.difficulty); count++) {
				
				Vector2 couldpos = (Vector2)player.transform.position;
				while (Vector2.Distance (player.transform.position, couldpos) < 10) {
					
					float x = Random.Range ((-BG.sizeDelta.x/10)/2 + Xscale , (BG.sizeDelta.x/10)/2) - Xscale;
					float y = Random.Range ((-BG.sizeDelta.y/10)/2 + Yscale, (BG.sizeDelta.y/10)/2) - Yscale;
					couldpos = new Vector2 (x, y);

				}
					

				GameObject newBlock = (GameObject)Instantiate (deathBlock, new Vector3 (couldpos.x, couldpos.y, 0), Quaternion.identity);
				float scale = Random.Range (0.5f, 2);
				newBlock.transform.localScale = new Vector3 (scale,scale, 1);
				newBlock.name = "killerblock";
				newBlock.transform.parent = transform.parent;
			}

			if (i == 4) {
				bounds.SendMessage ("clearPassage");
			}

			if (i == 5) {
				
				Spike.Destroy (gameObject);
				player.SendMessage ("GameOver");
			}


		}
	}
	public void saveScore(){

		int q = 0;
		int count = 10;


		PlayerPrefs.SetFloat(count.ToString(),Mathf.Round(timer.time_er * 1000) / 1000);

			while(q <= count){

			if (PlayerPrefs.HasKey ((count - 1).ToString()) == true) {
				//Debug.Log("A " + PlayerPrefs.GetFloat ((count - 1).ToString ()) + "    " + PlayerPrefs.GetFloat (count.ToString ()) );

				if (PlayerPrefs.GetFloat ((count - 1).ToString ()) > PlayerPrefs.GetFloat (count.ToString ())) {

					float temp = PlayerPrefs.GetFloat((count - 1).ToString()); 
					PlayerPrefs.SetFloat ((count - 1).ToString (), PlayerPrefs.GetFloat (count.ToString ()));
					PlayerPrefs.SetFloat(count.ToString(),temp);
				}
				count -= 1 ;
				PlayerPrefs.DeleteKey ("10");
			} 
			else {
				count = -1;
			}
		}
	}
}

