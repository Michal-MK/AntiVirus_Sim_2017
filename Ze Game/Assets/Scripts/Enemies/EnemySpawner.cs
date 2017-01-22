using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {


	RectTransform killerblockBG;
	RectTransform arrowtrapBG;
	RectTransform killerWallBG;
	Transform enemy;
	public GameObject foundation;
	public GameObject[] arrowtrap;
	public GameObject deathBlock;
	public M_Player player;
	public bool amIHere = false;
	public bool forTheFirstTime = true;



	float scale;


	void Start () {
		killerblockBG = GameObject.Find ("Background_Start").GetComponent <RectTransform> ();
		arrowtrapBG = GameObject.Find ("Background_room_2a").GetComponent <RectTransform> ();
		killerWallBG = GameObject.Find ("Background_room_1").GetComponent <RectTransform> ();
		enemy = GameObject.Find ("Enemies").transform;
	}

	public void spawnArrowTrap() {
		if (forTheFirstTime == false) {
			print(forTheFirstTime + " Normal");

			foreach (GameObject zone in CameraMovement.loadedZones) {

				if (zone.Equals(arrowtrapBG.gameObject) && amIHere == true) {
					break;
				}

				else if (zone.Equals(arrowtrapBG.gameObject) && amIHere == false) {

					Vector3 pos = new Vector3(arrowtrapBG.position.x, arrowtrapBG.position.y, 0);
					float bgx = arrowtrapBG.sizeDelta.x / 2;
					float bgy = arrowtrapBG.sizeDelta.y / 2;
					arrowtrap = new GameObject[4];

					arrowtrap[0] = (GameObject)Instantiate(foundation, pos + new Vector3(bgx / 2, bgy / 2, 0), Quaternion.identity);
					arrowtrap[1] = (GameObject)Instantiate(foundation, pos + new Vector3(-bgx / 2, bgy / 2, 0), Quaternion.identity);
					arrowtrap[2] = (GameObject)Instantiate(foundation, pos + new Vector3(bgx / 2, -bgy / 2, 0), Quaternion.identity);
					arrowtrap[3] = (GameObject)Instantiate(foundation, pos + new Vector3(-bgx / 2, -bgy / 2, 0), Quaternion.identity);
					amIHere = true;

					foreach (GameObject trap in arrowtrap) {
						trap.name = "arrowtrap";
						trap.transform.SetParent(enemy);
					}
					break;
				}
				else {
					foreach (GameObject deltrap in arrowtrap) {
						Destroy(deltrap.gameObject);
					}
					amIHere = false;
				}
			}
		}
	}

	public void spawnAvoidance() {
		print("Avoidance");
		Vector3 pos = new Vector3(arrowtrapBG.position.x, arrowtrapBG.position.y, 0);
		float bgx = arrowtrapBG.sizeDelta.x / 2;
		float bgy = arrowtrapBG.sizeDelta.y / 2;
		arrowtrap = new GameObject[4];

		arrowtrap[0] = (GameObject)Instantiate(foundation, pos + new Vector3(bgx / 2, bgy / 2, 0), Quaternion.identity);
		arrowtrap[1] = (GameObject)Instantiate(foundation, pos + new Vector3(-bgx / 2, bgy / 2, 0), Quaternion.identity);
		arrowtrap[2] = (GameObject)Instantiate(foundation, pos + new Vector3(bgx / 2, -bgy / 2, 0), Quaternion.identity);
		arrowtrap[3] = (GameObject)Instantiate(foundation, pos + new Vector3(-bgx / 2, -bgy / 2, 0), Quaternion.identity);

		foreach (GameObject trap in arrowtrap) {
			trap.name = "arrowtrap";
			trap.transform.SetParent(enemy);
		}
		StartCoroutine("hold");
	}
	
	private IEnumerator hold() {
		yield return new WaitForSeconds(30);
		forTheFirstTime = false;
		foreach (GameObject deltrap in arrowtrap) {
			Destroy(deltrap.gameObject);

		}
	}

	public void spawnKillerBlock(){

		for (int count = 0; count < (int)(Spike.spikesCollected + 5 * difficultySlider.difficulty); count++) {
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
	public void spawnKillerWall(){
		for(int i = 0; i < 3 ; i++){
			GameObject wallShot = ObjectPooler.script.GetPool ();
			wallShot.transform.rotation = Quaternion.AngleAxis (90, Vector3.back);
			wallShot.transform.position = new Vector3(killerWallBG.position.x - 2 + killerWallBG.sizeDelta.x/2, Random.Range ((int)killerWallBG.position.y - killerWallBG.sizeDelta.y / 2,(int)killerWallBG.position.y + killerWallBG.sizeDelta.y / 2),0);//
			wallShot.transform.SetParent (enemy);
			wallShot.SetActive (true);

				


			if (wallShot == null) {
				return;
			}
		}
	}
	public void InvokeRepeatingScript(string name){
		InvokeRepeating (name,0.5f,0.5f);
	}
	public void CancelInvoking(){
		CancelInvoke ();
	}
}
