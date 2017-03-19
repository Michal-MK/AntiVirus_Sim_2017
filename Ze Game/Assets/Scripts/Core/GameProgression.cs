using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgression : MonoBehaviour {

	public GameObject[] doors;
	public Sprite sprtOff;
	public Sprite sprtOn;
	public Canvas_Renderer canvas_Renderer;
	public GameObject Block;


	public SpriteRenderer DoorStatus_Start;
	public SpriteRenderer DoorStatus_Room1;
	public SpriteRenderer DoorStatus_Room2A;


	public float currentPositionPlayerX;
	public float currentPositionPlayerY;
	public float currentPositionPlayerZ;

	public float currentPositionBoxX;
	public float currentPositionBoxY;
	public float currentPositionBoxZ;

	public float ZRotationBlock;


	private void Awake() {
		Statics.gameProgression = this;
	}

	public void GetValues() {
		Vector3 PlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
		currentPositionPlayerX = PlayerPos.x;
		currentPositionPlayerY = PlayerPos.y;
		currentPositionPlayerZ = PlayerPos.z;

		Vector3 BlockPos = Block.transform.position;
		currentPositionBoxX = BlockPos.x;
		currentPositionBoxY = BlockPos.y;
		currentPositionBoxZ = BlockPos.z;
		Vector3 rot = Block.transform.rotation.eulerAngles;
		ZRotationBlock = rot.z;
		print(rot.z);
	}

	public void Progress() {

		if (M_Player.gameProgression == 1) {
			foreach (GameObject door in doors) {
				door.SetActive(true);
			}
			DoorStatus_Room1.sprite = sprtOff;
			doors[0].SetActive(false);
			doors[1].SetActive(false);
			DoorStatus_Start.sprite = sprtOn;
			canvas_Renderer.DisplayDirection(1);
		}

		if (M_Player.gameProgression == 2) {
			foreach (GameObject door in doors) {
				door.SetActive(true);
			}
			doors[0].SetActive(false);
			doors[1].SetActive(false);
			doors[2].SetActive(false);
			doors[3].SetActive(false);
			DoorStatus_Room1.sprite = sprtOn;
			canvas_Renderer.DisplayDirection(0);

		}
		if (M_Player.gameProgression == 3) {
			foreach (GameObject door in doors) {
				door.SetActive(true);
			}
			doors[0].SetActive(false);
			doors[1].SetActive(false);
			doors[2].SetActive(false);
			doors[3].SetActive(false);
			doors[4].SetActive(false);
			doors[5].SetActive(false);
			DoorStatus_Room2A.sprite = sprtOn;
			canvas_Renderer.DisplayDirection(2);


		}
		Camera.main.GetComponent<CameraMovement>().raycastForRooms();
	}
	private void OnDestroy() {
		Statics.gameProgression = null;
	}
}
