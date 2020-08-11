using UnityEngine;
using System.Collections;
using Igor.Constants.Strings;
using System;
using Random = UnityEngine.Random;

public class ProjectileWallController : MonoBehaviour, IEnemyControler {

	private RectTransform background;
	private ObjectPool pool;

	public Directions origin;
	public float spawnInterval;

	private float width = 0;
	private float height = 0;
	private float spriteRotation;

	private bool spawnLoop = true;

	public GameObject EnemyPrefab { get; private set; }

	public Room ActiveRoom => MapData.Instance.GetRoom(2);

	public bool RespawnOnReEntry => true;


	private void Start() {
		background = transform.parent.GetComponent<RectTransform>();
		width = background.sizeDelta.x / 2;
		height = background.sizeDelta.y / 2;
		Player.OnRoomEnter += M_Player_OnRoomEnter;

		switch (origin) {
			case Directions.TOP: {
				spriteRotation = 0;
				break;
			}
			case Directions.RIGHT: {
				spriteRotation = 90;
				break;
			}
			case Directions.BOTTOM: {
				spriteRotation = 180;
				break;
			}
			case Directions.LEFT: {
				spriteRotation = -90;
				break;
			}
		}

		StartCoroutine(SpawnKillerWall());
	}

	private void OnDestroy() {
		Clear();
	}

	public void SetProjecileType(EnemyType type) {
		switch (type) {
			case EnemyType.PROJECTILE_SIMPLE: {
				EnemyPrefab = Resources.Load(PrefabNames.ENEMY_PROJECTILE + (MapData.Instance.CurrentMapMode == MapMode.DARK ? "_Dark" : "")) as GameObject;
				break;
			}
			case EnemyType.PROJECTILE_ACCURATE: {
				EnemyPrefab = Resources.Load(PrefabNames.ENEMY_PROJECTILE) as GameObject;
				break;
			}
			case EnemyType.PROJECTILE_ICICLE: {
				EnemyPrefab = Resources.Load(PrefabNames.ENEMY_PROJECTILE_ICICLE + (MapData.Instance.CurrentMapMode == MapMode.DARK ? "_Dark" : "")) as GameObject;
				break;
			}
			default: {
				throw new Exception("Not a valid enemy type " + type);
			}
		}
		pool = new ObjectPool(EnemyPrefab);
	}

	public void MapStanceSwitch(MapMode mode) {
		pool.SwitchEnemyIllumination(mode);
	}

	private IEnumerator SpawnKillerWall() {
		int diff = Control.currDifficulty;
		spawnLoop = true;
		while (spawnLoop) {
			yield return new WaitForSeconds(spawnInterval);
			if (diff == 0 || diff == 1) {
				Spawn();
			}
			else if (diff == 2 || diff == 3) {
				for (int i = 0; i < 2; i++) {
					Spawn();
				}
			}
			else if (diff == 4) {
				for (int i = 0; i < 3; i++) {
					Spawn();
				}
			}
		}
	}

	public void Spawn() {
		Projectile wallShot = pool.Next.GetComponent<Projectile>();
		wallShot.IsKillable = true;
		wallShot.gameObject.tag = Tags.ENEMY;
		wallShot.transform.rotation = Quaternion.AngleAxis(spriteRotation, Vector3.back);
		wallShot.transform.position = KWProjectilePositions(origin);
		wallShot.transform.SetParent(transform);
		wallShot.gameObject.SetActive(true);
		wallShot.projectileSpeed = 15f;
		wallShot.Fire();
	}


	private Vector3 KWProjectilePositions(Directions side) {
		if (side == Directions.RIGHT || side == Directions.LEFT) {
			return new Vector3(background.position.x - 5 + (side == Directions.RIGHT ? width : -width), Random.Range(background.position.y - height, background.position.y + height));
		}
		else {
			return new Vector3(Random.Range(background.position.x - width, background.position.x + width), background.position.y + (side == Directions.TOP ? height : -height));
		}
	}

	private void M_Player_OnRoomEnter(Player sender, RectTransform background, RectTransform previous) {
		if (background != ActiveRoom.Background && !MapData.Instance.IsOneOfAdjecentLinks(background, 2)) {
			Despawn();
		}
		if (MapData.Instance.IsOneOfAdjecentLinks(background, 2) && previous != ActiveRoom.Background) {
			StartCoroutine(SpawnKillerWall());
		}
	}

	public void Despawn() {
		spawnLoop = false;
	}

	public void Clear() {
		pool.ClearPool();
		Player.OnRoomEnter -= M_Player_OnRoomEnter;
	}
}
