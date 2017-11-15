using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	#region Prefabs
	public GameObject foundation;
	public GameObject deathBlock;
	public GameObject warningObj;
	#endregion

	public M_Player player;

	public float timeKillerBlocksActive = 1.8f;
	public float timeCycleIdle = 0.2f;
	public float timeWarning = 1.5f;

	private RectTransform killerblockBG;
	private RectTransform arrowtrapBG;
	private RectTransform killerWallBG;

	private Transform enemy;

	public GameObject[] arrowTraps;
	public GameObject ICEPooler;

	public List<GameObject> warningSigns = new List<GameObject>();
	private List<BoxCollider2D> killerBlocks = new List<BoxCollider2D>();

	private float scale;
	private Vector2 killerblockpos;
	private bool KBCycleRunning = false;
	private bool isInvokingKillerWall = false;

	public List<GameObject> KWProjectiles = new List<GameObject>();

	private void Awake() {
		SignPost.OnAvoidanceBegin += SpawnAvoidance;
		M_Player.OnRoomEnter += M_Player_OnRoomEnter;
		M_Player.OnCoinPickup += M_Player_OnCoinPickup;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		for (int i = 0; i <= data.player.coinsCollected - 2; i++) {
			SpawnKillerBlock();
		}
	}

	private void M_Player_OnCoinPickup(M_Player sender, GameObject coinObj) {
		SpawnKillerBlock();
	}

	private void M_Player_OnRoomEnter(RectTransform background, M_Player sender) {
		if (background.name == "Background_Start") {
			if (M_Player.gameProgression != 0 && !KBCycleRunning) {
				//StartCoroutine(KBCycle());
			}
		}
		if (background.name == "Background_room_1") {
			if (!isInvokingKillerWall) {
				StartCoroutine(SpawnKillerWall(0.7f));
			}
		}
		else {
			isInvokingKillerWall = false;
		}
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
		arrowTraps = new GameObject[4];

		arrowTraps[0] = Instantiate(foundation, pos + new Vector3(bgx - 10, bgy - 10, 0), Quaternion.identity, enemy);

		arrowTraps[1] = Instantiate(foundation, pos + new Vector3(-bgx + 10, bgy - 10, 0), Quaternion.identity, enemy);

		arrowTraps[2] = Instantiate(foundation, pos + new Vector3(bgx - 10, -bgy + 10, 0), Quaternion.identity, enemy);

		arrowTraps[3] = Instantiate(foundation, pos + new Vector3(-bgx + 10, -bgy + 10, 0), Quaternion.identity, enemy);
	}

	public void DespawnAvoidance() {
		foreach (GameObject deltrap in arrowTraps) {
			Destroy(deltrap);
		}
	}

	public void SpawnKillerBlock() {

		if(player == null) {
			return;
		}

		int totalBlocks = ((Coins.coinsCollected + 5) * (1 + Control.currDifficulty));

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

			killerBlocks.Add(block.GetComponent<BoxCollider2D>());
			warningSigns.Add(warn);

		}
		if (KBCycleRunning == false) {
			StartCoroutine(KBCycle());
		}
	}

	public IEnumerator KBCycle() {

		KBCycleRunning = true;

		while (true) {
			for (int i = 0; i < killerBlocks.Count; i++) {
				killerBlocks[i].enabled = false;
				warningSigns[i].SetActive(false);
				killerBlocks[i].gameObject.SetActive(true);
			}

			yield return new WaitForSeconds(0.2f);

			for (int i = 0; i < killerBlocks.Count; i++) {
				killerBlocks[i].enabled = true;
			}
			yield return new WaitForSeconds(timeKillerBlocksActive);

			for (int i = 0; i < killerBlocks.Count; i++) {

				killerBlocks[i].GetComponent<Animator>().SetTrigger("Despawn");
				killerBlocks[i].enabled = false;
			}
			yield return new WaitForSeconds(timeCycleIdle);

			for (int i = 0; i < warningSigns.Count; i++) {

				killerBlocks[i].gameObject.SetActive(false);
				Vector3 pos = KBPositions();
				killerBlocks[i].transform.position = pos;
				warningSigns[i].transform.position = pos;
				warningSigns[i].SetActive(true);
			}
			yield return new WaitForSeconds(timeWarning);

			if (M_Player.currentBG_name != killerblockBG.name) {
				for (int i = 0; i < killerBlocks.Count; i++) {
					killerBlocks[i].gameObject.SetActive(false);
					KBCycleRunning = false;
					StopCoroutine(KBCycle());
				}
			}
		}
	}

	public IEnumerator SpawnKillerWall(float spawnDelay) {
		isInvokingKillerWall = true;
		ObjectPooler Icicle = ICEPooler.GetComponent<ObjectPooler>();
		Projectile.spawnedByKillerWall = true;
		int diff = Control.currDifficulty;

		while (isInvokingKillerWall) {
			yield return new WaitForSeconds(spawnDelay);

			if (diff == 0 || diff == 1) {
				GameObject wallShot = Icicle.GetPool();
				wallShot.transform.rotation = Quaternion.AngleAxis(90, Vector3.back);
				wallShot.transform.position = KWProjectilePositions();
				wallShot.transform.SetParent(enemy);
				wallShot.SetActive(true);
				KWProjectiles.Add(wallShot);
			}
			if (diff == 3 || diff == 2) {
				for (int i = 0; i < 2; i++) {
					GameObject wallShot = Icicle.GetPool();
					wallShot.transform.rotation = Quaternion.AngleAxis(90, Vector3.back);
					wallShot.transform.position = KWProjectilePositions();
					wallShot.transform.SetParent(enemy);
					wallShot.SetActive(true);
					KWProjectiles.Add(wallShot);
				}
			}
			if (diff == 4) {
				for (int i = 0; i < 3; i++) {
					GameObject wallShot = Icicle.GetPool();
					wallShot.transform.rotation = Quaternion.AngleAxis(90, Vector3.back);
					wallShot.transform.position = KWProjectilePositions();
					wallShot.transform.SetParent(enemy);
					wallShot.SetActive(true);
					KWProjectiles.Add(wallShot);
				}
			}
		}
	}

	public Vector3 KBPositions() {
		killerblockpos = player.transform.position;
		while (Vector2.Distance(player.transform.position, killerblockpos) < 12) {

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
		SignPost.OnAvoidanceBegin -= SpawnAvoidance;
		M_Player.OnRoomEnter -= M_Player_OnRoomEnter;
		M_Player.OnCoinPickup -= M_Player_OnCoinPickup;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		StopAllCoroutines();
	}
}
