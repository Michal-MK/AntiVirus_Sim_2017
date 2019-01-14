using System.Collections.Generic;
using UnityEngine;

public class ElecticBlockController : IEnemyControler {


	public Room activeRoom => MapData.script.GetRoom(1);

	public bool respawnOnReEntry => true;

	public GameObject enemyPrefab { get; }

	public List<ElecticalBlock> blocks = new List<ElecticalBlock>();

	private Transform parentTransform;

	public ElecticBlockController(GameObject elPrefab) {
		M_Player.OnRoomEnter += M_Player_OnRoomEnter;
		enemyPrefab = elPrefab;
		parentTransform = GameObject.Find("Enemies").transform;
	}

	private void M_Player_OnRoomEnter(M_Player sender, RectTransform background, RectTransform previous) {
		if (background != activeRoom.background && background != MapData.script.GetRoomLink(1,2).transition) {
			Despawn();
		}
		if (background == MapData.script.GetRoomLink(1, 2).transition && previous != activeRoom.background) {
			foreach (ElecticalBlock block in blocks) {
				block.gameObject.SetActive(true);
				block.StartCoroutine(block.Cycle());
			}
		}
	}


	public void Spawn() {
		int totalBlocks = ((Coin.coinsCollected + 5) * (1 + Control.currDifficulty));

		for (int count = 0; count < totalBlocks; count++) {
			float scale = Random.Range(0.5f, 1f);

			GameObject block = Object.Instantiate(enemyPrefab);

			block.transform.position = -Vector2.one * 1000;
			block.transform.localScale = new Vector3(scale, scale, 1);
			block.name = "Electical Block";
			block.transform.SetParent(parentTransform);
			blocks.Add(block.GetComponent<ElecticalBlock>());
		}
	}

	public void Despawn() {
		foreach (ElecticalBlock b in blocks) {
			b.DespawnElBlock();
		}
	}

	public void Clear() {
		foreach (ElecticalBlock b in blocks) {
			Object.Destroy(b.gameObject);
		}
	}
}
