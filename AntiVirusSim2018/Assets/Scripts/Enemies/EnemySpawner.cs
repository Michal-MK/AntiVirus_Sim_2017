using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Igor.Constants.Strings;

public class EnemySpawner : MonoBehaviour {

	#region Prefabs
	public GameObject turretBase;
	public GameObject deathBlock;
	public GameObject projectileWallPrefab;
	#endregion

	private List<ProjectileWallController> currentProjectileWalls = new List<ProjectileWallController>();

	private RectTransform arrowtrapBG;

	public TurretAttack[] turrets;

	private List<GameObject> killerBlocks = new List<GameObject>();

	private ElecticBlockController electicBlockController;


	private void Awake() {
		Player.OnRoomEnter += M_Player_OnRoomEnter;
		Player.OnCoinPickup += M_Player_OnCoinPickup;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
	}

	private void Start() {
		UpdatePrefabs(MapData.Instance.CurrentMapMode);
		arrowtrapBG = MapData.Instance.GetRoom(3).Background;
		electicBlockController = new ElecticBlockController(deathBlock);
	}

	private void LoadManager_OnSaveDataLoaded(SaveData data) {
		if (data.player.currentBGName == MapData.Instance.GetRoom(1).Background.name) {
			for (int i = 0; i <= data.player.coinsCollected - 2; i++) {
				SpawnElecticalBlocks();
			}
		}
	}

	private void M_Player_OnCoinPickup(Player sender, GameObject coinObj) {
		SpawnElecticalBlocks();
	}

	private void M_Player_OnRoomEnter(Player sender, RectTransform background, RectTransform previous) {
		if (background == MapData.Instance.GetRoom(2).Background) {
			if (currentProjectileWalls.Count == 0) {
				ProjectileWallController currentProjectileWall = projectileWallPrefab.GetComponent<ProjectileWallController>();
				currentProjectileWall.origin = Directions.RIGHT;
				currentProjectileWall.spawnInterval = 0.7f;
				currentProjectileWalls.Add(Instantiate(projectileWallPrefab, background).GetComponent<ProjectileWallController>());

				currentProjectileWalls[0].SetProjecileType(EnemyType.PROJECTILE_ICICLE);
			}
		}
		else if (background == MapData.Instance.GetRoom(9).Background) {
			if (currentProjectileWalls.Count == 0) {
				ProjectileWallController currentProjectileWall = projectileWallPrefab.GetComponent<ProjectileWallController>();
				currentProjectileWall.origin = Directions.TOP;
				currentProjectileWall.spawnInterval = 1.2f;
				currentProjectileWalls.Add(Instantiate(projectileWallPrefab, background).GetComponent<ProjectileWallController>());

				currentProjectileWalls[0].SetProjecileType(EnemyType.PROJECTILE_SIMPLE);

				currentProjectileWall.origin = Directions.BOTTOM;
				currentProjectileWall.spawnInterval = 0.8f;
				currentProjectileWalls.Add(Instantiate(projectileWallPrefab, background).GetComponent<ProjectileWallController>());
				currentProjectileWalls[1].SetProjecileType(EnemyType.PROJECTILE_SIMPLE);
			}
		}
		else if (currentProjectileWalls.Count != 0) {
			foreach (ProjectileWallController wall in currentProjectileWalls) {
				Destroy(wall.gameObject);
			}
			currentProjectileWalls.Clear();
		}
	}

	public TurretAttack[] SpawnAvoidance() {
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
			turrets[i].target = Player.Instance.gameObject;
			turrets[i].useDefaultTiming = true;
			turrets[i].applyRandomness = true;
			turrets[i].randomnessMultiplier = 20;
		}
		return turrets;
	}

	public void SpawnElecticalBlocks() {
		electicBlockController.Spawn();
	}

	private void ClearElecticalBlocks() {
		electicBlockController.Clear();
	}


	public void UpdatePrefabs(MapMode mode) {
		switch (mode) {
			case MapMode.LIGHT: {
				deathBlock = Resources.Load(PrefabNames.ENEMY_KILLERBLOCK) as GameObject;
				turretBase = Resources.Load(PrefabNames.ENEMY_TURRET) as GameObject;
				return;
			}
			case MapMode.DARK: {
				deathBlock = Resources.Load(PrefabNames.ENEMY_KILLERBLOCK + "_Dark") as GameObject;
				turretBase = Resources.Load(PrefabNames.ENEMY_TURRET + "_Dark") as GameObject;
				return;
			}
		}
	}

	private void OnDestroy() {
		Player.OnRoomEnter -= M_Player_OnRoomEnter;
		Player.OnCoinPickup -= M_Player_OnCoinPickup;
		LoadManager.OnSaveDataLoaded += LoadManager_OnSaveDataLoaded;
		StopAllCoroutines();
	}
}
