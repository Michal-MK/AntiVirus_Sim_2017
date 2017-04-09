using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgression : MonoBehaviour {

	public GameObject[] doors;
	public Sprite sprtOff;
	public Sprite sprtOn;
	public Canvas_Renderer canvas_Renderer;
	public GameObject Block;

	public float currentPositionPlayerX;
	public float currentPositionPlayerY;
	public float currentPositionPlayerZ;

	public float currentPositionBoxX;
	public float currentPositionBoxY;
	public float currentPositionBoxZ;

	public float currentPositionSpikeX;
	public float currentPositionSpikeY;
	public float currentPositionSpikeZ;

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

		if(GameObject.Find("Spike") != null && GameObject.Find("Spike").activeInHierarchy) {
			Transform spike = GameObject.Find("Spike").GetComponent<Transform>();

			currentPositionSpikeX = spike.position.x;
			currentPositionSpikeY = spike.position.y;
			currentPositionSpikeZ = spike.position.z;
		}
	}

	public void Progress() {

		if (M_Player.gameProgression == 1) {
			foreach (GameObject door in doors) {
				door.SetActive(true);
			}
			doors[0].SetActive(false);
			doors[1].SetActive(false);

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

			canvas_Renderer.DisplayDirection(2);


		}
		if (Statics.cameraMovement != null) {
			Statics.cameraMovement.raycastForRooms();
		}
	}
	private void OnDestroy() {
		Statics.gameProgression = null;
	}
}
