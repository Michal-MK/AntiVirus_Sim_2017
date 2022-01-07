using UnityEngine;
using System.Collections;

public class AdvanceInGame : MonoBehaviour {

	public int currentStage = 0;
	private int movementTypeCounter = 0;

	private bool canAdvance = true;
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.F2)) {
			if (!canAdvance) return;
			if (currentStage < 10) {
				currentStage++;
				switch (currentStage) {
					case 1: {
						for (int i = 0; i < 5; i++) {
							FindObjectOfType<Coin>().OnCoinPickup(Player.Instance, null);
						}
						MapData.Instance.GetRoomLink(1, 2).OpenDoor();
						FindObjectOfType<Spike>().transform.position = Vector3.zero;
						Player.Instance.transform.position = Vector3.zero;
						return;
					}
					case 2: {
						FindObjectOfType<BlockScript>().transform.position = FindObjectOfType<PressurePlate>().transform.position;
						MapData.Instance.GetRoomLink(2, 3).OpenDoor();
						Player.Instance.transform.position = FindObjectOfType<PressurePlate>().transform.position - new Vector3(20, 0);
						GameObject.Find("Collectibles").transform.Find("Spike").gameObject.SetActive(true);
						FindObjectOfType<Spike>().transform.position = Player.Instance.transform.position;
						return;
					}
					case 3: {
						Avoidance av = FindObjectOfType<Avoidance>();
						av.AvoidanceDuration = 4f;
						av.StartAvoidance();
						MapData.Instance.GetRoomLink(2, 3).OpenDoor();
						GameObject.Find("Collectibles").transform.Find("Spike").gameObject.SetActive(true);
						FindObjectOfType<Spike>().transform.position = Player.Instance.transform.position;
						return;
					}
					case 4: {
						Vector3 pos = FindObjectOfType<MazeEntrance>().transform.position;
						FindObjectOfType<MazeEntrance>().gameObject.SetActive(false);
						GameObject.Find("Collectibles").transform.Find("Spike").gameObject.SetActive(true);
						FindObjectOfType<Spike>().transform.position = pos;
						Player.Instance.transform.position = pos;
						FindObjectOfType<MazeEscape>().FromMazeTrans();
						return;
					}
					case 5: {
						Vector3 pos = FindObjectOfType<BossEntrance>().transform.position;
						GameObject.Find("Collectibles").transform.Find("Spike").gameObject.SetActive(true);
						FindObjectOfType<Spike>().transform.position = pos + Vector3.down * 10;
						Player.Instance.transform.position = pos + Vector3.down * 10;
						GameObject.Find("BombPickup").transform.position = pos + Vector3.down * 10;
						return;
					}
					case 6: {
						StartCoroutine(FirstBoss());
						return;
					}	
					case 7: {
						LeverRelay relay = FindObjectOfType<LeverRelay>();
						Player.Instance.transform.position = relay.transform.position + Vector3.left * 10;
						relay.Interact();
						return;
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.PageUp)) {
			PlayerMovementType pmt = (PlayerMovementType)movementTypeCounter;
			Player.Instance.pMovement.SetMovementMode(pmt);
			print($"Setting movement mode to {pmt}");
			movementTypeCounter++;
			movementTypeCounter %= 4;
		}
	}

	private IEnumerator FirstBoss() {
		canAdvance = false;
		Player.Instance.transform.position = FindObjectOfType<BossEntrance>().transform.position;
		yield return new WaitForSeconds(2);
		StartCoroutine(FindObjectOfType<BossHealth>().Death());
		FindObjectOfType<BossBehaviour>().StopAllCoroutines();
		MapData.Instance.GetRoomLink(5, 6).OpenDoor();
		yield return new WaitUntil(() => CameraMovement.Instance.CameraStill);
		Camera.main.orthographicSize = CameraMovement.DEFAULT_CAM_SIZE;
		canAdvance = true;
	}
}