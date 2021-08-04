using System.Collections.Generic;
using UnityEngine;

public class ElecticBlockController : IEnemyControler {

	public Room ActiveRoom => MapData.Instance.GetRoom(1);

	public bool RespawnOnReEntry => true;

	public GameObject EnemyPrefab { get; }

	public List<ElecticalBlock> blocks = new List<ElecticalBlock>();

	private Transform parentTransform;

	public Coin coin;

	public ElecticBlockController(GameObject elPrefab) {
		Player.OnRoomEnter += M_Player_OnRoomEnter;
		EnemyPrefab = elPrefab;
		parentTransform = GameObject.Find("Enemies").transform;
	}

	private void M_Player_OnRoomEnter(Player sender, RectTransform background, RectTransform previous) {
		if (background != ActiveRoom.Background && background != MapData.Instance.GetRoomLink(1,2).Transition) {
			Despawn();
		}
		if (background == MapData.Instance.GetRoomLink(1, 2).Transition && previous != ActiveRoom.Background) {
			foreach (ElecticalBlock block in blocks) {
				block.gameObject.SetActive(true);
				block.StartCoroutine(block.Cycle());
			}
		}
	}


	public void Spawn() {
		int totalBlocks = (coin.CoinsCollected + 5) * (1 + Control.currDifficulty);

		for (int count = 0; count < totalBlocks; count++) {
			float scale = Random.Range(0.5f, 1f);

			GameObject block = Object.Instantiate(EnemyPrefab);

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
		Player.OnRoomEnter -= M_Player_OnRoomEnter;
	}
}
