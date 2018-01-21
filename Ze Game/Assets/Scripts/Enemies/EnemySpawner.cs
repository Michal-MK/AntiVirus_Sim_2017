using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Igor.Constants.Strings;

public class EnemySpawner : MonoBehaviour {

	#region Prefabs
	public GameObject foundation;
	public GameObject deathBlock;
	#endregion

	public M_Player player;

	private RectTransform arrowtrapBG;
	private RectTransform killerWallBG;

	public GameObject[] arrowTraps;
	public GameObject ICEPooler;

	private List<GameObject> killerBlocks = new List<GameObject>();

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
		if (background.name == BackgroundNames.BACKGROUND1_2) {
			if (!isInvokingKillerWall) {
				StartCoroutine(SpawnKillerWall(0.7f));
			}
		}
		else {
			isInvokingKillerWall = false;
		}
	}

	private void OnEnable() {
		arrowtrapBG = GameObject.Find(BackgroundNames.BACKGROUND1_3).GetComponent<RectTransform>();
		killerWallBG = GameObject.Find(BackgroundNames.BACKGROUND1_2).GetComponent<RectTransform>();
	}

	public void SpawnAvoidance() {
		Vector3 pos = new Vector3(arrowtrapBG.position.x, arrowtrapBG.position.y, 0);
		float bgx = arrowtrapBG.sizeDelta.x / 2;
		float bgy = arrowtrapBG.sizeDelta.y / 2;
		Vector2[] positions = new Vector2[4] {
			new Vector3(bgx - 10, bgy - 10, 0),
			new Vector3(-bgx + 10, bgy - 10, 0),
			new Vector3(bgx - 10, -bgy + 10, 0),
			new Vector3(-bgx + 10, -bgy + 10, 0)
		};
		arrowTraps = new GameObject[4];
		for (int i = 0; i < arrowTraps.Length; i++) {
			arrowTraps[i] = Instantiate(foundation, pos + (Vector3)positions[i], Quaternion.identity, transform);
		}
		ClearKillerBlocks();
	}

	public void DespawnAvoidance() {
		foreach (GameObject deltrap in arrowTraps) {
			Destroy(deltrap);
		}
	}

	public void SpawnKillerBlock() {
		if (player == null) {
			return;
		}

		int totalBlocks = ((Coin.coinsCollected + 5) * (1 + Control.currDifficulty));

		for (int count = 0; count < totalBlocks; count++) {
			float scale = Random.Range(0.5f, 1f);

			GameObject block = Instantiate(deathBlock);

			block.transform.position = -Vector2.one * 1000;
			block.transform.localScale = new Vector3(scale, scale, 0);
			block.name = "Killerblock";
			block.transform.SetParent(transform);
			killerBlocks.Add(block);
		}
	}

	private void ClearKillerBlocks() {
		foreach (GameObject g in killerBlocks) {
			g.GetComponent<ElectricalBlock>().DespawnElBlock();
		}
	}

	public IEnumerator SpawnKillerWall(float spawnDelay) {
		isInvokingKillerWall = true;
		ObjectPool pool_Enemy_Icicle = new ObjectPool(Resources.Load(PrefabNames.ENEMY_PROJECTILE_ICICLE) as GameObject);
		int diff = Control.currDifficulty;

		while (isInvokingKillerWall) {
			yield return new WaitForSeconds(spawnDelay);
			if (diff == 0 || diff == 1) {
				Projectile wallShot = pool_Enemy_Icicle.getNext.GetComponent<Projectile>();
				wallShot.SetSprite(wallShot.Icicle);
				wallShot.gameObject.tag = "Enemy";
				wallShot.transform.rotation = Quaternion.AngleAxis(90, Vector3.back);
				wallShot.transform.position = KWProjectilePositions();
				wallShot.transform.SetParent(transform);
				wallShot.gameObject.SetActive(true);
				wallShot.projectileSpeed = 15f;
				KWProjectiles.Add(wallShot.gameObject);
				wallShot.Fire();
			}
			if (diff == 3 || diff == 2) {
				for (int i = 0; i < 2; i++) {
					Projectile wallShot = pool_Enemy_Icicle.getNext.GetComponent<Projectile>();
					wallShot.SetSprite(wallShot.Icicle);
					wallShot.gameObject.tag = "Enemy";
					wallShot.transform.rotation = Quaternion.AngleAxis(90, Vector3.back);
					wallShot.transform.position = KWProjectilePositions();
					wallShot.transform.SetParent(transform);
					wallShot.gameObject.SetActive(true);
					wallShot.projectileSpeed = 15f;
					KWProjectiles.Add(wallShot.gameObject);
					wallShot.Fire();
				}
			}
			if (diff == 4) {
				for (int i = 0; i < 3; i++) {
					Projectile wallShot = pool_Enemy_Icicle.getNext.GetComponent<Projectile>();
					wallShot.SetSprite(wallShot.Icicle);
					wallShot.gameObject.tag = "Enemy";
					wallShot.transform.rotation = Quaternion.AngleAxis(90, Vector3.back);
					wallShot.transform.position = KWProjectilePositions();
					wallShot.transform.SetParent(transform);
					wallShot.gameObject.SetActive(true);
					KWProjectiles.Add(wallShot.gameObject);
					wallShot.projectileSpeed = 15;
					wallShot.Fire();
				}
			}
		}
	}

	public Vector3 KWProjectilePositions() {
		return new Vector3(killerWallBG.position.x - 5 + killerWallBG.sizeDelta.x / 2,
						   Random.Range(killerWallBG.position.y - killerWallBG.sizeDelta.y / 2, killerWallBG.position.y + killerWallBG.sizeDelta.y / 2));
	}

	private void OnDestroy() {
		SignPost.OnAvoidanceBegin -= SpawnAvoidance;
		M_Player.OnRoomEnter -= M_Player_OnRoomEnter;
		M_Player.OnCoinPickup -= M_Player_OnCoinPickup;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		StopAllCoroutines();
	}
}
