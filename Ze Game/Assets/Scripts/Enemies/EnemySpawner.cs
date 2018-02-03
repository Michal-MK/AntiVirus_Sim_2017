using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Igor.Constants.Strings;

public class EnemySpawner : MonoBehaviour {

	#region Prefabs
	public GameObject turretBase;
	public GameObject deathBlock;
	private ObjectPool pool_Enemy_Icicle;
	#endregion

	private RectTransform arrowtrapBG;
	private RectTransform killerWallBG;

	public TurretAttack[] turrets;

	private List<GameObject> killerBlocks = new List<GameObject>();

	private bool isInvokingKillerWall = false;

	public List<GameObject> KWProjectiles = new List<GameObject>();


	private void Awake() {
		SignPost.OnAvoidanceBegin += SpawnAvoidance;
		M_Player.OnRoomEnter += M_Player_OnRoomEnter;
		M_Player.OnCoinPickup += M_Player_OnCoinPickup;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void Start() {
		UpdatePrefabs(MapData.script.currentMapMode);
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		if (data.player.currentBGName == MapData.script.GetBackground(1).name) {
			for (int i = 0; i <= data.player.coinsCollected - 2; i++) {
				SpawnKillerBlock();
			}
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
		turrets = new TurretAttack[4];
		for (int i = 0; i < turrets.Length; i++) {
			turrets[i] = Instantiate(turretBase, pos + (Vector3)positions[i], Quaternion.identity, transform).GetComponent<TurretAttack>();
			turrets[i].target = M_Player.player.gameObject;
			turrets[i].useDefaultTiming = true;
			turrets[i].applyRandomness = true;
			turrets[i].randomnessMultiplier = 20;
		}
		ClearKillerBlocks();
	}

	public void DespawnAvoidance() {
		foreach (TurretAttack turret in turrets) {
			Destroy(turret.gameObject);
		}
	}

	public void SpawnKillerBlock() {

		int totalBlocks = ((Coin.coinsCollected + 5) * (1 + Control.currDifficulty));

		for (int count = 0; count < totalBlocks; count++) {
			float scale = Random.Range(0.5f, 1f);

			GameObject block = Instantiate(deathBlock);

			block.transform.position = -Vector2.one * 1000;
			block.transform.localScale = new Vector3(scale, scale, 1);
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
		int diff = Control.currDifficulty;

		while (isInvokingKillerWall) {
			yield return new WaitForSeconds(spawnDelay);
			if (diff == 0 || diff == 1) {
				Projectile wallShot = pool_Enemy_Icicle.getNext.GetComponent<Projectile>();
				wallShot.gameObject.tag = Tags.ENEMY;
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
					wallShot.gameObject.tag = Tags.ENEMY;
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
					wallShot.gameObject.tag = Tags.ENEMY;
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

	public void UpdatePrefabs(MapData.MapMode mode) {
		switch (mode) {
			case MapData.MapMode.LIGHT: {
				pool_Enemy_Icicle = new ObjectPool(Resources.Load(PrefabNames.ENEMY_PROJECTILE_ICICLE) as GameObject);
				deathBlock = Resources.Load(PrefabNames.ENEMY_KILLERBLOCK) as GameObject;
				turretBase = Resources.Load(PrefabNames.ENEMY_TURRET) as GameObject;
				return;
			}
			case MapData.MapMode.DARK: {
				pool_Enemy_Icicle = new ObjectPool(Resources.Load(PrefabNames.ENEMY_PROJECTILE_ICICLE + "_Dark") as GameObject);
				deathBlock = Resources.Load(PrefabNames.ENEMY_KILLERBLOCK + "_Dark") as GameObject;
				turretBase = Resources.Load(PrefabNames.ENEMY_TURRET + "_Dark") as GameObject;
				return;
			}
		}
	}

	private void OnDestroy() {
		SignPost.OnAvoidanceBegin -= SpawnAvoidance;
		M_Player.OnRoomEnter -= M_Player_OnRoomEnter;
		M_Player.OnCoinPickup -= M_Player_OnCoinPickup;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		StopAllCoroutines();
	}
}
