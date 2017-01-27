using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public GameObject KBPooler;
	public GameObject EPPooler;



	void Start() {
		killerblockBG = GameObject.Find("Background_Start").GetComponent<RectTransform>();
		arrowtrapBG = GameObject.Find("Background_room_2a").GetComponent<RectTransform>();
		killerWallBG = GameObject.Find("Background_room_1").GetComponent<RectTransform>();
		enemy = GameObject.Find("Enemies").transform;
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

	List<GameObject> Blocks = new List<GameObject>();
	float scale;
	Vector2 killerblockpos;
	bool CRunning = false;

	public void spawnKillerBlock() {

		for (int count = 0; count < (int)(Spike.spikesCollected + 5 * difficultySlider.difficulty); count++) {

			scale = Random.Range(0.8f, 5);
			GameObject block = KBPooler.GetComponent<ObjectPooler>().GetPool();
			block.transform.position = GeneratePosition();
			block.transform.localScale = new Vector3(scale, scale, 0);
			block.name = "killerblock";
			block.transform.SetParent(enemy);
			block.SetActive(true);
			Blocks.Add(block);

			if (CRunning == false) {
				StartCoroutine("KBCycle");
			}
		}
	}


	public IEnumerator KBCycle() {
		CRunning = true;
		while (true) {
			yield return new WaitForSeconds(2);
			for (int i = 0; i < Blocks.Count; i++) {
				Blocks[i].GetComponent<Animator>().SetTrigger("Despawn");
			}
			yield return new WaitForSeconds(0.2f);
			for (int i = 0; i < Blocks.Count; i++) {
				Blocks[i].GetComponent<Animator>().SetTrigger("Reset");
				Blocks[i].SetActive(false);
				Blocks[i].transform.position = GeneratePosition();
				Blocks[i].SetActive(true);
			}
			if (M_Player.currentBG_name != killerblockBG.name) {
				for (int i = 0; i < Blocks.Count; i++) {
					Blocks[i].GetComponent<Animator>().SetTrigger("Reset");
					Blocks[i].SetActive(false);
					CRunning = false;
					StopCoroutine("KBCycle");
				}
			}
		}
	}



	public Vector3 GeneratePosition() {
		killerblockpos = player.transform.position;
		while (Vector2.Distance(player.transform.position, killerblockpos) < 12) {

			float x = Random.Range(-killerblockBG.sizeDelta.x / 2 + scale, killerblockBG.sizeDelta.x / 2 - scale);
			float y = Random.Range(-killerblockBG.sizeDelta.y / 2 + scale, killerblockBG.sizeDelta.y / 2 - scale);
			killerblockpos = new Vector2(x, y);
		}
		return killerblockpos;
	}


	List<GameObject> KWProjectiles = new List<GameObject>();

	public void spawnKillerWall() {

			for (int i = 0; i < 3; i++) {
				GameObject wallShot = EPPooler.GetComponent<ObjectPooler>().GetPool();
				wallShot.transform.rotation = Quaternion.AngleAxis(90, Vector3.back);
				wallShot.transform.position = new Vector3(killerWallBG.position.x - 2 + killerWallBG.sizeDelta.x / 2, Random.Range((int)killerWallBG.position.y - killerWallBG.sizeDelta.y / 2, (int)killerWallBG.position.y + killerWallBG.sizeDelta.y / 2), 0);//
				wallShot.transform.SetParent(enemy);
				wallShot.SetActive(true);
				KWProjectiles.Add(wallShot);
			}

		if(M_Player.currentBG_name != killerWallBG.name) {
			foreach(GameObject p in KWProjectiles) {
				p.SetActive(false);
			}
			CancelInvoke();

		}
	}
	public void InvokeRepeatingScript(string name) {
		InvokeRepeating(name, 0.5f, 0.5f);
	}
}
