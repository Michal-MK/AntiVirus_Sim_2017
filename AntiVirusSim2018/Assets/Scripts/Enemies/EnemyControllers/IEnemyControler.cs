using UnityEngine;

interface IEnemyControler {

	GameObject enemyPrefab { get; }

	Room activeRoom { get; }
	bool respawnOnReEntry { get; }

	void Spawn();
	void Despawn();
	void Clear();
}
