using UnityEngine;

interface IEnemyControler {

	GameObject EnemyPrefab { get; }
	Room ActiveRoom { get; }
	bool RespawnOnReEntry { get; }

	void Spawn();
	void Despawn();
	void Clear();
}
