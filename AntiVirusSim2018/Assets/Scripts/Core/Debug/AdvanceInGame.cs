using UnityEngine;
using System.Collections;

public class AdvanceInGame : MonoBehaviour {
	public int currentStage = 0;
	private int movementTypeCounter = 0;

	void Update() {
		if (Input.GetKeyDown(KeyCode.F2)) {
			if (currentStage < 10) {
				currentStage++;
				switch (currentStage) {
					case 1: {
						for (int i = 0; i < 5; i++) {
							FindObjectOfType<Coin>().OnCoinPickup(M_Player.player, null);
						}
						MapData.script.OpenDoor(MapData.script.GetRoomLink(1,2));
						FindObjectOfType<Spike>().transform.position = Vector3.zero;
						M_Player.player.transform.position = Vector3.zero;
						return;
					}
					case 2: {
						FindObjectOfType<BlockScript>().transform.position = FindObjectOfType<PressurePlate>().transform.position;
						MapData.script.OpenDoor(MapData.script.GetRoomLink(2, 3));
						M_Player.player.transform.position = FindObjectOfType<PressurePlate>().transform.position - new Vector3(20,0);
						GameObject.Find("Collectibles").transform.Find("Spike").gameObject.SetActive(true);
						FindObjectOfType<Spike>().transform.position = M_Player.player.transform.position;
						return;
					}
					case 3: {
						FindObjectOfType<Avoidance>().avoidDuration = 6f;
						FindObjectOfType<Avoidance>().GetType().GetMethod("SignPost_OnAvoidanceBegin", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(FindObjectOfType<Avoidance>(),null);
						
						FindObjectOfType<Avoidance>().StartAvoidance();
						MapData.script.OpenDoor(MapData.script.GetRoomLink(2, 3));
						GameObject.Find("Collectibles").transform.Find("Spike").gameObject.SetActive(true);
						FindObjectOfType<Spike>().transform.position = M_Player.player.transform.position;
						return;
					}
					case 4: {
						Vector3 pos = FindObjectOfType<MazeEntrance>().transform.position;
						FindObjectOfType<MazeEntrance>().gameObject.SetActive(false);
						GameObject.Find("Collectibles").transform.Find("Spike").gameObject.SetActive(true);
						FindObjectOfType<Spike>().transform.position = pos;
						M_Player.player.transform.position = pos;
						FindObjectOfType<MazeEscape>().FromMazeTrans();
						return;
					}
					case 5: {
						Vector3 pos = FindObjectOfType<BossEntrance>().transform.position;
						GameObject.Find("Collectibles").transform.Find("Spike").gameObject.SetActive(true);
						FindObjectOfType<Spike>().transform.position = pos + Vector3.down * 10;
						M_Player.player.transform.position = pos + Vector3.down * 10;
						GameObject.Find("BombPickup").transform.position = pos + Vector3.down * 10;
						return;
					}
					case 6: {
						StartCoroutine(FirstBoss());
						return;
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.PageUp)) {
			print("++++");
			FindObjectOfType<Player_Movement>().SetMovementMode((Player_Movement.PlayerMovement)movementTypeCounter);
			movementTypeCounter++;
			movementTypeCounter = movementTypeCounter % 4;
		}
	}

	private IEnumerator FirstBoss() {
		M_Player.player.transform.position = FindObjectOfType<BossEntrance>().transform.position;
		yield return new WaitForSeconds(2);
		StartCoroutine(FindObjectOfType<BossHealth>().Death());
		FindObjectOfType<BossBehaviour>().StopAllCoroutines();
		MapData.script.OpenDoor(MapData.script.GetRoomLink(5, 6));
		yield return new WaitUntil(() => CameraMovement.script.isCameraDoneMoving);
		Camera.main.orthographicSize = CameraMovement.defaultCamSize;
	}
}
