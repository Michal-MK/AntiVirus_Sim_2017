using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	#region
	public GameObject foundation;
	public GameObject deathBlock;
	public GameObject warningObj;
	#endregion

	public float timeKillerBlocksActive = 1.8f;
	public float timeCycleIdle = 0.2f;
	public float timeWarning = 1.5f;

	private RectTransform killerblockBG;
	private RectTransform arrowtrapBG;
	private RectTransform killerWallBG;
	private Transform enemy;

	public GameObject[] arrowtrap;

	public GameObject ICEPooler;

	public List<GameObject> Blocks = new List<GameObject>();
	public List<GameObject> Warnings = new List<GameObject>();
	private List<BoxCollider2D> col = new List<BoxCollider2D>();

	float scale;
	Vector2 killerblockpos;
	bool CRunning = false;

	public List<GameObject> KWProjectiles = new List<GameObject>();

	private void Awake() {
		Statics.enemySpawner = this;
	}

	private void OnEnable() {
		killerblockBG = GameObject.Find("Background_Start").GetComponent<RectTransform>();
		arrowtrapBG = GameObject.Find("Background_room_2a").GetComponent<RectTransform>();
		killerWallBG = GameObject.Find("Background_room_1").GetComponent<RectTransform>();
		enemy = GameObject.Find("Enemies").transform;
	}

	public void SpawnAvoidance() {
		Vector3 pos = new Vector3(arrowtrapBG.position.x, arrowtrapBG.position.y, 0);
		float bgx = arrowtrapBG.sizeDelta.x / 2;
		float bgy = arrowtrapBG.sizeDelta.y / 2;
		arrowtrap = new GameObject[4];

		arrowtrap[0] = Instantiate(foundation, pos + new Vector3(bgx - 10, bgy - 10, 0), Quaternion.identity, enemy);

		arrowtrap[1] = Instantiate(foundation, pos + new Vector3(-bgx + 10, bgy - 10, 0), Quaternion.identity, enemy);

		arrowtrap[2] = Instantiate(foundation, pos + new Vector3(bgx - 10, -bgy + 10, 0), Quaternion.identity, enemy);

		arrowtrap[3] = Instantiate(foundation, pos + new Vector3(-bgx + 10, -bgy + 10, 0), Quaternion.identity, enemy);

		StartCoroutine(hold());
	}

	private IEnumerator hold() {
		yield return new WaitForSeconds(55);
		foreach (GameObject deltrap in arrowtrap) {
			Destroy(deltrap);
		}
	}

	public void SpawnKillerBlock() {

		int totalBlocks = ((Coins.coinsCollected + 5) * (1 + Control.script.chosenDifficulty));

		//print(Coins.coinsCollected + " + " + 5 + " * " + (1 + Control.script.chosenDifficulty) + " = " + ((Coins.coinsCollected + 5) * (1 + Control.script.chosenDifficulty)));

		for (int count = 0; count < totalBlocks; count++) {

			scale = Random.Range(0.5f, 1f);

			GameObject warn = Instantiate(warningObj);
			GameObject block = Instantiate(deathBlock);


			Vector3 pos = KBPositions();

			block.transform.position = pos;

			block.transform.localScale = new Vector3(scale, scale, 0);
			warn.transform.localScale = new Vector3(scale / 2, scale / 2, 0);

			block.name = "Killerblock";
			warn.name = "Warning " + count;

			block.transform.SetParent(enemy);
			warn.transform.SetParent(enemy);

			col.Add(block.GetComponent<BoxCollider2D>());
			Blocks.Add(block);
			Warnings.Add(warn);

		}
		if (CRunning == false) {
			StartCoroutine(KBCycle());
		}
	}


	public IEnumerator KBCycle() {

		CRunning = true;

		while (true) {


			for (int i = 0; i < Blocks.Count; i++) {
				col[i].enabled = false;
				Warnings[i].SetActive(false);
				Blocks[i].SetActive(true);
			}
			
			yield return new WaitForSeconds(0.2f);

			for (int i = 0; i < col.Count; i++) {
				col[i].enabled = true;
			}
			yield return new WaitForSeconds(timeKillerBlocksActive);

			for (int i = 0; i < Blocks.Count; i++) {

				Blocks[i].GetComponent<Animator>().SetTrigger("Despawn");
				col[i].enabled = false;
			}
			yield return new WaitForSeconds(timeCycleIdle);

			for (int i = 0; i < Warnings.Count; i++) {

				Blocks[i].SetActive(false);
				Vector3 pos = KBPositions();
				Blocks[i].transform.position = pos;
				Warnings[i].transform.position = pos;
				Warnings[i].SetActive(true);
			}
			yield return new WaitForSeconds(timeWarning);

			if (M_Player.currentBG_name != killerblockBG.name) {
				for (int i = 0; i < Blocks.Count; i++) {
					Blocks[i].SetActive(false);
					CRunning = false;
					StopCoroutine(KBCycle());
				}
			}
		}
	}

	public IEnumerator Warn(Vector3 pos, Vector3 scale) {
		GameObject warning = Instantiate(warningObj);
		warning.transform.position = pos;
		warning.transform.localScale = scale;
		warning.SetActive(true);
		yield return new WaitForSeconds(1);
		warning.SetActive(false);
		StopCoroutine(Warn(pos,scale));
	}

	public void SpawnKillerWall() {
		ObjectPooler Icicle = ICEPooler.GetComponent<ObjectPooler>();
		Projectile.spawnedByKillerWall = true;

		if (PlayerPrefs.GetInt("difficulty") == 0 || PlayerPrefs.GetInt("difficulty") == 1) {


			for (int i = 0; i < 1; i++) {
				GameObject wallShot = Icicle.GetPool();
				wallShot.transform.rotation = Quaternion.AngleAxis(90, Vector3.back);
				wallShot.transform.position = KWProjectilePositions();
				wallShot.transform.SetParent(enemy);
				wallShot.SetActive(true);
				KWProjectiles.Add(wallShot);
			}
		}
		if (PlayerPrefs.GetInt("difficulty") == 3 || PlayerPrefs.GetInt("difficulty") == 2) {
			for (int i = 0; i < 2; i++) {
				GameObject wallShot = Icicle.GetPool();
				wallShot.transform.rotation = Quaternion.AngleAxis(90, Vector3.back);
				wallShot.transform.position = KWProjectilePositions();
				wallShot.transform.SetParent(enemy);
				wallShot.SetActive(true);
				KWProjectiles.Add(wallShot);
			}
		}
		if (PlayerPrefs.GetInt("difficulty") == 4) {
			for (int i = 0; i < 3; i++) {
				GameObject wallShot = Icicle.GetPool();
				wallShot.transform.rotation = Quaternion.AngleAxis(90, Vector3.back);
				wallShot.transform.position = KWProjectilePositions();
				wallShot.transform.SetParent(enemy);
				wallShot.SetActive(true);
				KWProjectiles.Add(wallShot);
			}
		}
		if (M_Player.currentBG_name != killerWallBG.name) {
			foreach (GameObject p in KWProjectiles) {
				p.SetActive(false);
			}
			CancelInvoke();
		}
	}

	public Vector3 KBPositions() {
		killerblockpos = Statics.mPlayer.transform.position;
		while (Vector2.Distance(Statics.mPlayer.transform.position, killerblockpos) < 12) {

			float x = Random.Range(-killerblockBG.sizeDelta.x / 2 + scale, killerblockBG.sizeDelta.x / 2 - scale);
			float y = Random.Range(-killerblockBG.sizeDelta.y / 2 + scale, killerblockBG.sizeDelta.y / 2 - scale);
			killerblockpos = new Vector2(x, y);
		}
		return killerblockpos;
	}

	public Vector3 KWProjectilePositions() {
		return new Vector3(killerWallBG.position.x - 5 + killerWallBG.sizeDelta.x / 2, Random.Range(killerWallBG.position.y - killerWallBG.sizeDelta.y / 2, killerWallBG.position.y + killerWallBG.sizeDelta.y / 2), 0);
	}

	private void OnDestroy() {
		Statics.enemySpawner = null;
	}
}
