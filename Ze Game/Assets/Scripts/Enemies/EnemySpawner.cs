using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Igor.Constants.Strings;

public class EnemySpawner : MonoBehaviour {

	#region Prefabs
	public GameObject turretBase;
	public GameObject deathBlock;
	public GameObject projectileWallPrefab;
	private GameObject[] activeKillerWalls;
	#endregion

	private RectTransform arrowtrapBG;

	public TurretAttack[] turrets;

	private List<GameObject> killerBlocks = new List<GameObject>();


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
		if (background == MapData.script.GetBackground(2)) {
			if (activeKillerWalls == null) {
				activeKillerWalls = new GameObject[1];
				ProjectileWall pWall = projectileWallPrefab.GetComponent<ProjectileWall>();
				pWall.origin = Directions.RIGHT;
				pWall.spawnInterval = 0.7f;
				activeKillerWalls[0] = Instantiate(projectileWallPrefab, background);
				activeKillerWalls[0].GetComponent<ProjectileWall>().SetProjecileType(Enemy.EnemyType.PROJECTILE_ICICLE);
			}
		}
		else if (background == MapData.script.GetBackground(9)) {
			if (activeKillerWalls == null) {
				activeKillerWalls = new GameObject[2];
				ProjectileWall pWall = projectileWallPrefab.GetComponent<ProjectileWall>();

				pWall.origin = Directions.TOP;
				pWall.spawnInterval = 1f;
				activeKillerWalls[0] = Instantiate(projectileWallPrefab, background);
				activeKillerWalls[0].GetComponent<ProjectileWall>().SetProjecileType(Enemy.EnemyType.PROJECTILE_SIMPLE);

				pWall.origin = Directions.BOTTOM;
				pWall.spawnInterval = 1f;
				activeKillerWalls[1] = Instantiate(projectileWallPrefab, background);
				activeKillerWalls[1].GetComponent<ProjectileWall>().SetProjecileType(Enemy.EnemyType.PROJECTILE_SIMPLE);
			}
		}
		else if (activeKillerWalls != null) {
			foreach (GameObject wall in activeKillerWalls) {
				Destroy(wall);
			}
			activeKillerWalls = null;
		}
	}

	private void OnEnable() {
		arrowtrapBG = GameObject.Find(BackgroundNames.BACKGROUND1_3).GetComponent<RectTransform>();
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


	public void UpdatePrefabs(MapData.MapMode mode) {
		switch (mode) {
			case MapData.MapMode.LIGHT: {
				deathBlock = Resources.Load(PrefabNames.ENEMY_KILLERBLOCK) as GameObject;
				turretBase = Resources.Load(PrefabNames.ENEMY_TURRET) as GameObject;
				return;
			}
			case MapData.MapMode.DARK: {
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
