using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {


	RectTransform killerblockBG;
	RectTransform arrowtrapBG;
	Transform enemy;
	public GameObject foundation;
	public GameObject[] arrowtrap;
	public GameObject deathBlock;
	public M_Player player;
	public bool amIHere = false;



	float scale;


	void Start () {
		killerblockBG = GameObject.Find ("Background_Start").GetComponent <RectTransform> ();
		arrowtrapBG = GameObject.Find ("Background_room_2a").GetComponent <RectTransform> ();
		enemy = GameObject.Find ("Enemies").transform;
	}

	public void spawnArrowTrap () {
		foreach (GameObject zone in CameraMovement.loadedZones) {

			if (zone.Equals (arrowtrapBG.gameObject) && amIHere == true) {
				print ("Do Nothing!");
				break;
			}

			else if (zone.Equals (arrowtrapBG.gameObject) && amIHere == false) {

				Vector3 pos = new Vector3 (arrowtrapBG.position.x, arrowtrapBG.position.y, 0);
				float bgx = arrowtrapBG.sizeDelta.x / 2;
				float bgy = arrowtrapBG.sizeDelta.y / 2;
				arrowtrap = new GameObject[4];

				arrowtrap [0] = (GameObject)Instantiate (foundation, pos + new Vector3 (bgx / 2, bgy / 2, 0), Quaternion.identity);
				arrowtrap [1] = (GameObject)Instantiate (foundation, pos + new Vector3 (-bgx / 2, bgy / 2, 0), Quaternion.identity);
				arrowtrap [2] = (GameObject)Instantiate (foundation, pos + new Vector3 (bgx / 2, -bgy / 2, 0), Quaternion.identity);
				arrowtrap [3] = (GameObject)Instantiate (foundation, pos + new Vector3 (-bgx / 2, -bgy / 2, 0), Quaternion.identity);
				amIHere = true;

				foreach (GameObject trap in arrowtrap) {
					trap.name = "arrowtrap";
					trap.transform.SetParent (enemy);
				}
				break;
			}
			else {
				foreach (GameObject deltrap in arrowtrap) {
					Destroy (deltrap.gameObject);
				}
				amIHere = false;
			}
		}
	}

	public void spawnKillerBlock(){

		for (int count = 0; count < (int)(Spike.i + 5 * difficultySlider.difficulty); count++) {
			scale = Random.Range (0.5f, 2);

			Vector2 couldpos = (Vector2)player.transform.position;
			while (Vector2.Distance (player.transform.position, couldpos) < 10) {

				float x = Random.Range (-killerblockBG.sizeDelta.x / 2 + scale, killerblockBG.sizeDelta.x / 2 - scale);
				float y = Random.Range (-killerblockBG.sizeDelta.y / 2 + scale, killerblockBG.sizeDelta.y / 2 - scale);
				couldpos = new Vector2 (x, y);

			}


			GameObject newBlock = (GameObject)Instantiate (deathBlock, new Vector3 (couldpos.x, couldpos.y, 0), Quaternion.identity);
			newBlock.transform.localScale = new Vector3 (scale, scale, 1);
			newBlock.name = "killerblock";
			newBlock.transform.SetParent (enemy);
		}
	}
}
